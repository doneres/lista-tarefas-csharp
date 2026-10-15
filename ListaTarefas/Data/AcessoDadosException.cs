namespace ListaTarefas.Data
{
    /// <summary>
    /// Exceção da camada de dados. A mensagem é amigável ao usuário;
    /// os detalhes técnicos ficam na InnerException (gravados no log, nunca exibidos).
    /// </summary>
    public class AcessoDadosException : Exception
    {
        public AcessoDadosException(string mensagem, Exception excecaoOriginal)
            : base(mensagem, excecaoOriginal)
        {
        }
    }
}
