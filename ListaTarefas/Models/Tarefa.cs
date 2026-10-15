namespace ListaTarefas.Models
{
    /// <summary>
    /// Entidade que representa uma tarefa da lista.
    /// </summary>
    public class Tarefa
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public bool Concluida { get; set; }
        public DateTime? DataConclusao { get; set; }
        public int Ordem { get; set; }

        /// <summary>
        /// Uma tarefa está atrasada quando não foi concluída e a data de término já passou.
        /// </summary>
        public bool EstaAtrasada(DateTime hoje)
        {
            return !Concluida && DataFim.HasValue && DataFim.Value.Date < hoje.Date;
        }
    }
}
