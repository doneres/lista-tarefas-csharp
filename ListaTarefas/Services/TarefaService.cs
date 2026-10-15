using ListaTarefas.Data;
using ListaTarefas.Models;

namespace ListaTarefas.Services
{
    /// <summary>
    /// Regras de negócio da lista de tarefas. Os formulários conversam apenas com esta classe;
    /// nenhum formulário acessa o banco diretamente.
    /// </summary>
    public class TarefaService
    {
        public const int TamanhoMaximoTitulo = 100;
        public const int TamanhoMaximoDescricao = 500;

        private readonly TarefaRepository _repositorio;

        public TarefaService(TarefaRepository repositorio)
        {
            _repositorio = repositorio;
        }

        public List<Tarefa> Listar(bool incluirConcluidas)
        {
            return incluirConcluidas ? _repositorio.ListarTodas() : _repositorio.ListarPendentes();
        }

        public Tarefa? ObterPorId(int id)
        {
            return _repositorio.ObterPorId(id);
        }

        /// <summary>
        /// Valida e grava a tarefa (insere se for nova, atualiza se já existir).
        /// </summary>
        public void Salvar(Tarefa tarefa)
        {
            tarefa.Titulo = (tarefa.Titulo ?? string.Empty).Trim();
            tarefa.Descricao = string.IsNullOrWhiteSpace(tarefa.Descricao) ? null : tarefa.Descricao.Trim();

            Validar(tarefa);

            if (tarefa.Id == 0)
            {
                _repositorio.Inserir(tarefa);
            }
            else
            {
                _repositorio.Atualizar(tarefa);
            }
        }

        public void Concluir(Tarefa tarefa)
        {
            if (tarefa.Concluida)
            {
                throw new RegraNegocioException("Esta tarefa já está concluída.");
            }

            _repositorio.AlterarConclusao(tarefa.Id, true, DateTime.Now);
        }

        public void Reabrir(Tarefa tarefa)
        {
            if (!tarefa.Concluida)
            {
                throw new RegraNegocioException("Esta tarefa ainda não foi concluída.");
            }

            _repositorio.AlterarConclusao(tarefa.Id, false, null);
        }

        /// <summary>
        /// Sobe a tarefa uma posição entre as pendentes. Retorna false se já estiver no topo.
        /// </summary>
        public bool MoverParaCima(Tarefa tarefa)
        {
            return Mover(tarefa, -1);
        }

        /// <summary>
        /// Desce a tarefa uma posição entre as pendentes. Retorna false se já estiver no fim.
        /// </summary>
        public bool MoverParaBaixo(Tarefa tarefa)
        {
            return Mover(tarefa, +1);
        }

        public void Excluir(Tarefa tarefa)
        {
            _repositorio.Excluir(tarefa.Id);
        }

        // ----------------------------------------------------------------- regras

        private bool Mover(Tarefa tarefa, int deslocamento)
        {
            if (tarefa.Concluida)
            {
                throw new RegraNegocioException("Somente tarefas pendentes podem ter a ordem alterada.");
            }

            List<Tarefa> pendentes = _repositorio.ListarPendentes();
            int indiceAtual = pendentes.FindIndex(t => t.Id == tarefa.Id);
            int indiceDestino = indiceAtual + deslocamento;

            if (indiceAtual < 0 || indiceDestino < 0 || indiceDestino >= pendentes.Count)
            {
                return false;
            }

            _repositorio.TrocarOrdem(tarefa.Id, pendentes[indiceDestino].Id);
            return true;
        }

        private static void Validar(Tarefa tarefa)
        {
            if (string.IsNullOrWhiteSpace(tarefa.Titulo))
            {
                throw new RegraNegocioException("Informe o título da tarefa.");
            }

            if (tarefa.Titulo.Length > TamanhoMaximoTitulo)
            {
                throw new RegraNegocioException(
                    $"O título deve ter no máximo {TamanhoMaximoTitulo} caracteres.");
            }

            if (tarefa.Descricao != null && tarefa.Descricao.Length > TamanhoMaximoDescricao)
            {
                throw new RegraNegocioException(
                    $"A descrição deve ter no máximo {TamanhoMaximoDescricao} caracteres.");
            }

            if (tarefa.DataInicio.HasValue && tarefa.DataFim.HasValue
                && tarefa.DataFim.Value.Date < tarefa.DataInicio.Value.Date)
            {
                throw new RegraNegocioException(
                    "A data de término não pode ser anterior à data de início.");
            }
        }
    }
}
