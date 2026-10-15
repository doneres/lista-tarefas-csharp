using Microsoft.Data.Sqlite;

namespace ListaTarefas.Data
{
    /// <summary>
    /// Garante que a estrutura do banco exista. Executado uma vez na inicialização.
    /// </summary>
    public class BancoDeDados
    {
        private readonly ConexaoFactory _conexaoFactory;

        public BancoDeDados(ConexaoFactory conexaoFactory)
        {
            _conexaoFactory = conexaoFactory;
        }

        public void CriarEstruturaSeNecessario()
        {
            const string sql = @"
                CREATE TABLE IF NOT EXISTS Tarefas (
                    Id            INTEGER PRIMARY KEY AUTOINCREMENT,
                    Titulo        TEXT    NOT NULL,
                    Descricao     TEXT    NULL,
                    DataInicio    TEXT    NULL,
                    DataFim       TEXT    NULL,
                    Concluida     INTEGER NOT NULL DEFAULT 0,
                    DataConclusao TEXT    NULL,
                    Ordem         INTEGER NOT NULL
                );";

            try
            {
                using SqliteConnection conexao = _conexaoFactory.CriarConexaoAberta();
                using SqliteCommand comando = conexao.CreateCommand();
                comando.CommandText = sql;
                comando.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                throw new AcessoDadosException(
                    "Não foi possível preparar o banco de dados da aplicação.", ex);
            }
        }
    }
}
