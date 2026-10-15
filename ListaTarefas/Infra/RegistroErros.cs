using System.Text;

namespace ListaTarefas.Infra
{
    /// <summary>
    /// Grava os detalhes técnicos das falhas em "erros.log" (pasta do executável).
    /// O usuário vê apenas mensagens amigáveis; a pilha de erro fica só no log.
    /// </summary>
    public static class RegistroErros
    {
        private static readonly string CaminhoLog =
            Path.Combine(AppContext.BaseDirectory, "erros.log");

        public static void Registrar(Exception excecao)
        {
            try
            {
                var texto = new StringBuilder();
                texto.AppendLine($"[{DateTime.Now:dd/MM/yyyy HH:mm:ss}]");
                texto.AppendLine(excecao.ToString());
                texto.AppendLine(new string('-', 60));
                File.AppendAllText(CaminhoLog, texto.ToString());
            }
            catch (IOException)
            {
                // Se nem o log puder ser gravado, a aplicação segue funcionando:
                // a mensagem amigável já foi exibida ao usuário.
            }
            catch (UnauthorizedAccessException)
            {
                // Mesmo caso acima: sem permissão de escrita na pasta.
            }
        }
    }
}
