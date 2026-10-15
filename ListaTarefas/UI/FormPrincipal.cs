using ListaTarefas.Data;
using ListaTarefas.Infra;
using ListaTarefas.Models;
using ListaTarefas.Services;

namespace ListaTarefas.UI
{
    /// <summary>
    /// Tela inicial: lista as tarefas por fazer, destaca as atrasadas e permite
    /// incluir, editar, concluir, reordenar e excluir. Só conversa com o TarefaService.
    /// </summary>
    public partial class FormPrincipal : Form
    {
        private static readonly Color CorFundoAtrasada = Color.FromArgb(255, 228, 225);
        private static readonly Color CorTextoAtrasada = Color.DarkRed;
        private static readonly Color CorTextoConcluida = Color.Gray;

        private readonly TarefaService _servico;
        private readonly Font _fonteDestaque;

        public FormPrincipal(TarefaService servico)
        {
            InitializeComponent();
            _servico = servico;
            _fonteDestaque = new Font(dgvTarefas.Font, FontStyle.Bold);
            FormClosed += (_, _) => _fonteDestaque.Dispose();
        }

        private void FormPrincipal_Load(object? sender, EventArgs e)
        {
            ttDicas.SetToolTip(btnNova, "Nova tarefa (Ctrl+N)");
            ttDicas.SetToolTip(btnEditar, "Editar tarefa selecionada (Enter ou duplo clique)");
            ttDicas.SetToolTip(btnConcluir, "Marcar como concluída (Ctrl+Enter)");
            ttDicas.SetToolTip(btnSubir, "Mover para cima (Ctrl+↑)");
            ttDicas.SetToolTip(btnDescer, "Mover para baixo (Ctrl+↓)");
            ttDicas.SetToolTip(btnExcluir, "Excluir tarefa (Delete)");

            CarregarTarefas(null);
            ActiveControl = dgvTarefas;
        }

        // ----------------------------------------------------------------- atalhos de teclado

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Control | Keys.N:
                    NovaTarefa();
                    return true;
                case Keys.F5:
                    CarregarTarefas(TarefaSelecionada()?.Id);
                    return true;
            }

            // Os atalhos abaixo agem sobre a linha selecionada, então só valem com o foco na lista.
            if (dgvTarefas.Focused)
            {
                switch (keyData)
                {
                    case Keys.Enter:
                        EditarTarefa();
                        return true;
                    case Keys.Control | Keys.Enter:
                        ConcluirOuReabrirTarefa();
                        return true;
                    case Keys.Control | Keys.Up:
                        MoverTarefa(paraCima: true);
                        return true;
                    case Keys.Control | Keys.Down:
                        MoverTarefa(paraCima: false);
                        return true;
                    case Keys.Delete:
                        ExcluirTarefa();
                        return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        // ----------------------------------------------------------------- eventos

        private void btnNova_Click(object? sender, EventArgs e) => NovaTarefa();

        private void btnEditar_Click(object? sender, EventArgs e) => EditarTarefa();

        private void btnConcluir_Click(object? sender, EventArgs e) => ConcluirOuReabrirTarefa();

        private void btnSubir_Click(object? sender, EventArgs e) => MoverTarefa(paraCima: true);

        private void btnDescer_Click(object? sender, EventArgs e) => MoverTarefa(paraCima: false);

        private void btnExcluir_Click(object? sender, EventArgs e) => ExcluirTarefa();

        private void chkMostrarConcluidas_CheckedChanged(object? sender, EventArgs e)
        {
            CarregarTarefas(TarefaSelecionada()?.Id);
        }

        private void dgvTarefas_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                EditarTarefa();
            }
        }

        private void dgvTarefas_SelectionChanged(object? sender, EventArgs e)
        {
            AtualizarBotoes();
        }

        // ----------------------------------------------------------------- ações

        private void NovaTarefa()
        {
            using var formulario = new FormTarefa(_servico, null);
            if (formulario.ShowDialog(this) == DialogResult.OK)
            {
                CarregarTarefas(null);
                SelecionarUltimaLinha();
            }
        }

        private void EditarTarefa()
        {
            Tarefa? tarefa = TarefaSelecionada();
            if (tarefa == null)
            {
                return;
            }

            using var formulario = new FormTarefa(_servico, tarefa);
            if (formulario.ShowDialog(this) == DialogResult.OK)
            {
                CarregarTarefas(tarefa.Id);
            }
        }

        private void ConcluirOuReabrirTarefa()
        {
            Tarefa? tarefa = TarefaSelecionada();
            if (tarefa == null)
            {
                return;
            }

            ExecutarComTratamento(() =>
            {
                if (tarefa.Concluida)
                {
                    _servico.Reabrir(tarefa);
                }
                else
                {
                    _servico.Concluir(tarefa);
                }

                CarregarTarefas(null);
            });
        }

        private void MoverTarefa(bool paraCima)
        {
            Tarefa? tarefa = TarefaSelecionada();
            if (tarefa == null)
            {
                return;
            }

            ExecutarComTratamento(() =>
            {
                bool moveu = paraCima ? _servico.MoverParaCima(tarefa) : _servico.MoverParaBaixo(tarefa);
                if (moveu)
                {
                    CarregarTarefas(tarefa.Id);
                }
            });
        }

        private void ExcluirTarefa()
        {
            Tarefa? tarefa = TarefaSelecionada();
            if (tarefa == null)
            {
                return;
            }

            DialogResult resposta = MessageBox.Show(
                $"Excluir a tarefa \"{tarefa.Titulo}\"?\nEsta ação não pode ser desfeita.",
                "Confirmar exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (resposta != DialogResult.Yes)
            {
                return;
            }

            ExecutarComTratamento(() =>
            {
                _servico.Excluir(tarefa);
                CarregarTarefas(null);
            });
        }

        // ----------------------------------------------------------------- lista

        private void CarregarTarefas(int? idParaSelecionar)
        {
            List<Tarefa> tarefas;
            try
            {
                tarefas = _servico.Listar(chkMostrarConcluidas.Checked);
            }
            catch (AcessoDadosException ex)
            {
                RegistroErros.Registrar(ex);
                MessageBox.Show(ex.Message, "Lista de Tarefas", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DateTime hoje = DateTime.Today;

            dgvTarefas.SuspendLayout();
            dgvTarefas.Rows.Clear();

            foreach (Tarefa tarefa in tarefas)
            {
                int indice = dgvTarefas.Rows.Add(
                    tarefa.Titulo,
                    tarefa.DataInicio?.ToString("dd/MM/yyyy") ?? "—",
                    tarefa.DataFim?.ToString("dd/MM/yyyy") ?? "—",
                    DescreverSituacao(tarefa, hoje));

                DataGridViewRow linha = dgvTarefas.Rows[indice];
                linha.Tag = tarefa;
                AplicarDestaque(linha, tarefa, hoje);
            }

            dgvTarefas.ResumeLayout();

            SelecionarTarefa(idParaSelecionar);
            AtualizarResumo(tarefas, hoje);
            AtualizarBotoes();
        }

        private void AplicarDestaque(DataGridViewRow linha, Tarefa tarefa, DateTime hoje)
        {
            if (tarefa.EstaAtrasada(hoje))
            {
                linha.DefaultCellStyle.BackColor = CorFundoAtrasada;
                linha.DefaultCellStyle.ForeColor = CorTextoAtrasada;
                linha.DefaultCellStyle.Font = _fonteDestaque;
            }
            else if (tarefa.Concluida)
            {
                linha.DefaultCellStyle.ForeColor = CorTextoConcluida;
            }
        }

        private static string DescreverSituacao(Tarefa tarefa, DateTime hoje)
        {
            if (tarefa.Concluida)
            {
                return tarefa.DataConclusao.HasValue
                    ? $"Concluída {tarefa.DataConclusao.Value:dd/MM}"
                    : "Concluída";
            }

            if (tarefa.EstaAtrasada(hoje))
            {
                return "ATRASADA";
            }

            if (tarefa.DataInicio.HasValue && tarefa.DataInicio.Value.Date > hoje)
            {
                return "Agendada";
            }

            return "A fazer";
        }

        private void AtualizarResumo(List<Tarefa> tarefas, DateTime hoje)
        {
            int pendentes = tarefas.Count(t => !t.Concluida);
            int atrasadas = tarefas.Count(t => t.EstaAtrasada(hoje));

            lblResumo.Text =
                $"{pendentes} tarefa(s) por fazer, {atrasadas} atrasada(s).   " +
                "Atalhos: Ctrl+N nova · Enter editar · Ctrl+Enter concluir · Ctrl+↑/↓ mover · Del excluir · F5 atualizar";
        }

        private void AtualizarBotoes()
        {
            Tarefa? tarefa = TarefaSelecionada();
            bool temSelecao = tarefa != null;
            bool pendente = tarefa != null && !tarefa.Concluida;

            btnEditar.Enabled = temSelecao;
            btnExcluir.Enabled = temSelecao;
            btnConcluir.Enabled = temSelecao;
            btnConcluir.Text = tarefa != null && tarefa.Concluida ? "&Reabrir" : "&Concluir";
            btnSubir.Enabled = pendente;
            btnDescer.Enabled = pendente;
        }

        private Tarefa? TarefaSelecionada()
        {
            return dgvTarefas.CurrentRow?.Tag as Tarefa;
        }

        private void SelecionarTarefa(int? id)
        {
            if (dgvTarefas.Rows.Count == 0)
            {
                return;
            }

            DataGridViewRow? alvo = null;
            if (id.HasValue)
            {
                alvo = dgvTarefas.Rows
                    .Cast<DataGridViewRow>()
                    .FirstOrDefault(r => r.Tag is Tarefa t && t.Id == id.Value);
            }

            alvo ??= dgvTarefas.Rows[0];
            dgvTarefas.CurrentCell = alvo.Cells[0];
        }

        private void SelecionarUltimaLinha()
        {
            // Nova tarefa entra no fim da lista de pendentes.
            DataGridViewRow? ultimaPendente = dgvTarefas.Rows
                .Cast<DataGridViewRow>()
                .LastOrDefault(r => r.Tag is Tarefa t && !t.Concluida);

            if (ultimaPendente != null)
            {
                dgvTarefas.CurrentCell = ultimaPendente.Cells[0];
            }
        }

        /// <summary>
        /// Executa uma ação da tela tratando as exceções previstas com mensagens amigáveis.
        /// </summary>
        private void ExecutarComTratamento(Action acao)
        {
            try
            {
                acao();
            }
            catch (RegraNegocioException ex)
            {
                MessageBox.Show(ex.Message, "Lista de Tarefas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (AcessoDadosException ex)
            {
                RegistroErros.Registrar(ex);
                MessageBox.Show(ex.Message + "\nTente novamente.", "Lista de Tarefas",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
