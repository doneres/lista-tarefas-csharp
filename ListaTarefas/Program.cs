using System.Configuration;
using ListaTarefas.Data;
using ListaTarefas.Infra;
using ListaTarefas.Services;
using ListaTarefas.UI;

namespace ListaTarefas
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Rede de segurança: qualquer erro não previsto vira mensagem amigável + log.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (_, e) => TratarErroInesperado(e.Exception);

            TarefaService servico;
            try
            {
                var conexaoFactory = new ConexaoFactory();
                new BancoDeDados(conexaoFactory).CriarEstruturaSeNecessario();
                servico = new TarefaService(new TarefaRepository(conexaoFactory));
            }
            catch (ConfigurationErrorsException ex)
            {
                RegistroErros.Registrar(ex);
                MessageBox.Show(
                    "A configuração do banco de dados não foi encontrada.\nVerifique o arquivo de configuração da aplicação.",
                    "Lista de Tarefas", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (AcessoDadosException ex)
            {
                RegistroErros.Registrar(ex);
                MessageBox.Show(ex.Message, "Lista de Tarefas", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Application.Run(new FormPrincipal(servico));
        }

        private static void TratarErroInesperado(Exception excecao)
        {
            RegistroErros.Registrar(excecao);
            MessageBox.Show(
                "Ocorreu um erro inesperado. A operação não foi concluída.\nOs detalhes foram registrados no arquivo erros.log.",
                "Lista de Tarefas", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
