using System.Configuration;
using Microsoft.Data.Sqlite;

namespace ListaTarefas.Data
{
    /// <summary>
    /// Cria conexões a partir da connection string do App.config.
    /// Nenhuma string de conexão fica escrita no código-fonte.
    /// </summary>
    public class ConexaoFactory
    {
        private const string NomeConnectionString = "ListaTarefasDb";
        private readonly string _connectionString;

        public ConexaoFactory()
        {
            ConnectionStringSettings? configuracao =
                ConfigurationManager.ConnectionStrings[NomeConnectionString];

            if (configuracao == null || string.IsNullOrWhiteSpace(configuracao.ConnectionString))
            {
                throw new ConfigurationErrorsException(
                    $"A connection string '{NomeConnectionString}' não foi encontrada no App.config.");
            }

            _connectionString = configuracao.ConnectionString;
        }

        /// <summary>
        /// Retorna uma conexão nova e já aberta. Quem chama é responsável por descartá-la (using).
        /// </summary>
        public SqliteConnection CriarConexaoAberta()
        {
            var conexao = new SqliteConnection(_connectionString);
            conexao.Open();
            return conexao;
        }
    }
}
