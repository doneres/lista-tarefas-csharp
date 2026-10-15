namespace ListaTarefas.Services
{
    /// <summary>
    /// Lançada quando uma regra de negócio é violada. A mensagem é exibida ao usuário.
    /// </summary>
    public class RegraNegocioException : Exception
    {
        public RegraNegocioException(string mensagem) : base(mensagem)
        {
        }
    }
}
