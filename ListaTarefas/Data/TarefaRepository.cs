using System.Globalization;
using ListaTarefas.Models;
using Microsoft.Data.Sqlite;

namespace ListaTarefas.Data
{
    /// <summary>
    /// Acesso a dados da tabela Tarefas. Todas as consultas usam parâmetros
    /// (proteção contra SQL Injection) e cada operação abre e descarta sua conexão.
    /// </summary>
    public class TarefaRepository
    {
        private const string FormatoData = "yyyy-MM-dd";
        private const string FormatoDataHora = "yyyy-MM-dd HH:mm:ss";

        private const string ColunasSelect =
            "Id, Titulo, Descricao, DataInicio, DataFim, Concluida, DataConclusao, Ordem";

        private readonly ConexaoFactory _conexaoFactory;

        public TarefaRepository(ConexaoFactory conexaoFactory)
        {
            _conexaoFactory = conexaoFactory;
        }

        public List<Tarefa> ListarPendentes()
        {
            string sql = $"SELECT {ColunasSelect} FROM Tarefas WHERE Concluida = 0 ORDER BY Ordem, Id;";
            return ExecutarConsulta(sql, "Não foi possível carregar as tarefas pendentes.");
        }

        public List<Tarefa> ListarTodas()
        {
            string sql = $"SELECT {ColunasSelect} FROM Tarefas ORDER BY Concluida, Ordem, Id;";
            return ExecutarConsulta(sql, "Não foi possível carregar a lista de tarefas.");
        }

        public Tarefa? ObterPorId(int id)
        {
            try
            {
                using SqliteConnection conexao = _conexaoFactory.CriarConexaoAberta();
                using SqliteCommand comando = conexao.CreateCommand();
                comando.CommandText = $"SELECT {ColunasSelect} FROM Tarefas WHERE Id = @id;";
                comando.Parameters.AddWithValue("@id", id);

                using SqliteDataReader leitor = comando.ExecuteReader();
                return leitor.Read() ? MapearTarefa(leitor) : null;
            }
            catch (SqliteException ex)
            {
                throw new AcessoDadosException("Não foi possível localizar a tarefa.", ex);
            }
        }

        /// <summary>
        /// Insere a tarefa no final da lista. O cálculo da próxima ordem e o INSERT
        /// ocorrem na mesma transação para não gerar ordens duplicadas.
        /// </summary>
        public int Inserir(Tarefa tarefa)
        {
            try
            {
                using SqliteConnection conexao = _conexaoFactory.CriarConexaoAberta();
                using SqliteTransaction transacao = conexao.BeginTransaction();

                int proximaOrdem;
                using (SqliteCommand comandoOrdem = conexao.CreateCommand())
                {
                    comandoOrdem.Transaction = transacao;
                    comandoOrdem.CommandText = "SELECT COALESCE(MAX(Ordem), 0) + 1 FROM Tarefas;";
                    proximaOrdem = Convert.ToInt32(comandoOrdem.ExecuteScalar());
                }

                int novoId;
                using (SqliteCommand comandoInsert = conexao.CreateCommand())
                {
                    comandoInsert.Transaction = transacao;
                    comandoInsert.CommandText = @"
                        INSERT INTO Tarefas (Titulo, Descricao, DataInicio, DataFim, Concluida, Ordem)
                        VALUES (@titulo, @descricao, @dataInicio, @dataFim, 0, @ordem);
                        SELECT last_insert_rowid();";
                    comandoInsert.Parameters.AddWithValue("@titulo", tarefa.Titulo);
                    comandoInsert.Parameters.AddWithValue("@descricao", ValorOuNulo(tarefa.Descricao));
                    comandoInsert.Parameters.AddWithValue("@dataInicio", DataParaBanco(tarefa.DataInicio));
                    comandoInsert.Parameters.AddWithValue("@dataFim", DataParaBanco(tarefa.DataFim));
                    comandoInsert.Parameters.AddWithValue("@ordem", proximaOrdem);
                    novoId = Convert.ToInt32(comandoInsert.ExecuteScalar());
                }

                transacao.Commit();

                tarefa.Id = novoId;
                tarefa.Ordem = proximaOrdem;
                return novoId;
            }
            catch (SqliteException ex)
            {
                throw new AcessoDadosException("Não foi possível salvar a nova tarefa.", ex);
            }
        }

        public void Atualizar(Tarefa tarefa)
        {
            try
            {
                using SqliteConnection conexao = _conexaoFactory.CriarConexaoAberta();
                using SqliteCommand comando = conexao.CreateCommand();
                comando.CommandText = @"
                    UPDATE Tarefas
                       SET Titulo = @titulo,
                           Descricao = @descricao,
                           DataInicio = @dataInicio,
                           DataFim = @dataFim
                     WHERE Id = @id;";
                comando.Parameters.AddWithValue("@titulo", tarefa.Titulo);
                comando.Parameters.AddWithValue("@descricao", ValorOuNulo(tarefa.Descricao));
                comando.Parameters.AddWithValue("@dataInicio", DataParaBanco(tarefa.DataInicio));
                comando.Parameters.AddWithValue("@dataFim", DataParaBanco(tarefa.DataFim));
                comando.Parameters.AddWithValue("@id", tarefa.Id);
                comando.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                throw new AcessoDadosException("Não foi possível salvar as alterações da tarefa.", ex);
            }
        }

        public void AlterarConclusao(int id, bool concluida, DateTime? dataConclusao)
        {
            try
            {
                using SqliteConnection conexao = _conexaoFactory.CriarConexaoAberta();
                using SqliteCommand comando = conexao.CreateCommand();
                comando.CommandText = @"
                    UPDATE Tarefas
                       SET Concluida = @concluida,
                           DataConclusao = @dataConclusao
                     WHERE Id = @id;";
                comando.Parameters.AddWithValue("@concluida", concluida ? 1 : 0);
                comando.Parameters.AddWithValue("@dataConclusao",
                    dataConclusao.HasValue
                        ? dataConclusao.Value.ToString(FormatoDataHora, CultureInfo.InvariantCulture)
                        : DBNull.Value);
                comando.Parameters.AddWithValue("@id", id);
                comando.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                throw new AcessoDadosException("Não foi possível atualizar a situação da tarefa.", ex);
            }
        }

        /// <summary>
        /// Troca a posição de duas tarefas. As duas atualizações são interdependentes,
        /// por isso rodam em uma única transação: ou as duas acontecem, ou nenhuma.
        /// </summary>
        public void TrocarOrdem(int idTarefaA, int idTarefaB)
        {
            try
            {
                using SqliteConnection conexao = _conexaoFactory.CriarConexaoAberta();
                using SqliteTransaction transacao = conexao.BeginTransaction();

                int ordemA = ObterOrdem(conexao, transacao, idTarefaA);
                int ordemB = ObterOrdem(conexao, transacao, idTarefaB);

                AtualizarOrdem(conexao, transacao, idTarefaA, ordemB);
                AtualizarOrdem(conexao, transacao, idTarefaB, ordemA);

                transacao.Commit();
            }
            catch (SqliteException ex)
            {
                throw new AcessoDadosException("Não foi possível alterar a ordem das tarefas.", ex);
            }
        }

        public void Excluir(int id)
        {
            try
            {
                using SqliteConnection conexao = _conexaoFactory.CriarConexaoAberta();
                using SqliteCommand comando = conexao.CreateCommand();
                comando.CommandText = "DELETE FROM Tarefas WHERE Id = @id;";
                comando.Parameters.AddWithValue("@id", id);
                comando.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                throw new AcessoDadosException("Não foi possível excluir a tarefa.", ex);
            }
        }

        // ----------------------------------------------------------------- auxiliares

        private List<Tarefa> ExecutarConsulta(string sql, string mensagemErro)
        {
            try
            {
                var tarefas = new List<Tarefa>();

                using SqliteConnection conexao = _conexaoFactory.CriarConexaoAberta();
                using SqliteCommand comando = conexao.CreateCommand();
                comando.CommandText = sql;

                using SqliteDataReader leitor = comando.ExecuteReader();
                while (leitor.Read())
                {
                    tarefas.Add(MapearTarefa(leitor));
                }

                return tarefas;
            }
            catch (SqliteException ex)
            {
                throw new AcessoDadosException(mensagemErro, ex);
            }
        }

        private static int ObterOrdem(SqliteConnection conexao, SqliteTransaction transacao, int id)
        {
            using SqliteCommand comando = conexao.CreateCommand();
            comando.Transaction = transacao;
            comando.CommandText = "SELECT Ordem FROM Tarefas WHERE Id = @id;";
            comando.Parameters.AddWithValue("@id", id);

            object? resultado = comando.ExecuteScalar();
            if (resultado == null || resultado == DBNull.Value)
            {
                throw new InvalidOperationException($"Tarefa {id} não encontrada.");
            }

            return Convert.ToInt32(resultado);
        }

        private static void AtualizarOrdem(SqliteConnection conexao, SqliteTransaction transacao, int id, int novaOrdem)
        {
            using SqliteCommand comando = conexao.CreateCommand();
            comando.Transaction = transacao;
            comando.CommandText = "UPDATE Tarefas SET Ordem = @ordem WHERE Id = @id;";
            comando.Parameters.AddWithValue("@ordem", novaOrdem);
            comando.Parameters.AddWithValue("@id", id);
            comando.ExecuteNonQuery();
        }

        private static Tarefa MapearTarefa(SqliteDataReader leitor)
        {
            return new Tarefa
            {
                Id = leitor.GetInt32(0),
                Titulo = leitor.GetString(1),
                Descricao = leitor.IsDBNull(2) ? null : leitor.GetString(2),
                DataInicio = LerData(leitor, 3, FormatoData),
                DataFim = LerData(leitor, 4, FormatoData),
                Concluida = leitor.GetInt32(5) == 1,
                DataConclusao = LerData(leitor, 6, FormatoDataHora),
                Ordem = leitor.GetInt32(7)
            };
        }

        private static DateTime? LerData(SqliteDataReader leitor, int indice, string formato)
        {
            if (leitor.IsDBNull(indice))
            {
                return null;
            }

            return DateTime.TryParseExact(leitor.GetString(indice), formato,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime data)
                ? data
                : null;
        }

        private static object DataParaBanco(DateTime? data)
        {
            return data.HasValue
                ? data.Value.ToString(FormatoData, CultureInfo.InvariantCulture)
                : DBNull.Value;
        }

        private static object ValorOuNulo(string? texto)
        {
            return string.IsNullOrWhiteSpace(texto) ? DBNull.Value : texto;
        }
    }
}
