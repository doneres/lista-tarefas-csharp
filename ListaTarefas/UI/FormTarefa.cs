using System.Globalization;
using ListaTarefas.Data;
using ListaTarefas.Infra;
using ListaTarefas.Models;
using ListaTarefas.Services;

namespace ListaTarefas.UI
{
    /// <summary>
    /// Cadastro e edição de uma tarefa. Faz apenas validação de formato (interface);
    /// as regras de negócio ficam no TarefaService.
    /// </summary>
    public partial class FormTarefa : Form
    {
        private static readonly CultureInfo CulturaBrasil = new CultureInfo("pt-BR");
        private const string FormatoDataTela = "dd/MM/yyyy";

        private readonly TarefaService _servico;
        private readonly Tarefa _tarefa;

        /// <param name="tarefa">null para nova tarefa; a tarefa existente para edição.</param>
        public FormTarefa(TarefaService servico, Tarefa? tarefa)
        {
            InitializeComponent();
            _servico = servico;
            _tarefa = tarefa ?? new Tarefa();
        }

        private void FormTarefa_Load(object? sender, EventArgs e)
        {
            bool novaTarefa = _tarefa.Id == 0;
            Text = novaTarefa ? "Nova tarefa" : "Editar tarefa";

            txtTitulo.Text = _tarefa.Titulo;
            txtDescricao.Text = _tarefa.Descricao ?? string.Empty;
            mtbDataInicio.Text = FormatarData(_tarefa.DataInicio);
            mtbDataFim.Text = FormatarData(_tarefa.DataFim);

            // Foco inicial no primeiro campo.
            ActiveControl = txtTitulo;
        }

        private void btnSalvar_Click(object? sender, EventArgs e)
        {
            if (!ValidarCampos(out DateTime? dataInicio, out DateTime? dataFim))
            {
                return;
            }

            _tarefa.Titulo = txtTitulo.Text;
            _tarefa.Descricao = txtDescricao.Text;
            _tarefa.DataInicio = dataInicio;
            _tarefa.DataFim = dataFim;

            try
            {
                _servico.Salvar(_tarefa);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (RegraNegocioException ex)
            {
                MessageBox.Show(ex.Message, "Verifique os dados",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (AcessoDadosException ex)
            {
                RegistroErros.Registrar(ex);
                MessageBox.Show(ex.Message + "\nTente novamente.", "Erro ao salvar",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Validação de formato: título preenchido e datas válidas, sinalizadas com ErrorProvider.
        /// </summary>
        private bool ValidarCampos(out DateTime? dataInicio, out DateTime? dataFim)
        {
            errValidacao.Clear();
            Control? primeiroCampoInvalido = null;

            if (string.IsNullOrWhiteSpace(txtTitulo.Text))
            {
                errValidacao.SetError(txtTitulo, "Informe o título da tarefa.");
                primeiroCampoInvalido ??= txtTitulo;
            }

            if (!TentarLerData(mtbDataInicio, out dataInicio))
            {
                errValidacao.SetError(mtbDataInicio, "Data inválida. Use o formato dd/mm/aaaa.");
                primeiroCampoInvalido ??= mtbDataInicio;
            }

            if (!TentarLerData(mtbDataFim, out dataFim))
            {
                errValidacao.SetError(mtbDataFim, "Data inválida. Use o formato dd/mm/aaaa.");
                primeiroCampoInvalido ??= mtbDataFim;
            }

            if (primeiroCampoInvalido != null)
            {
                primeiroCampoInvalido.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Campo vazio é válido (data opcional). Campo preenchido precisa ser uma data real.
        /// </summary>
        private static bool TentarLerData(MaskedTextBox campo, out DateTime? data)
        {
            data = null;

            bool campoVazio = !campo.Text.Any(char.IsDigit);
            if (campoVazio)
            {
                return true;
            }

            if (DateTime.TryParseExact(campo.Text, FormatoDataTela, CulturaBrasil,
                    DateTimeStyles.None, out DateTime convertida))
            {
                data = convertida;
                return true;
            }

            return false;
        }

        private static string FormatarData(DateTime? data)
        {
            return data.HasValue ? data.Value.ToString(FormatoDataTela, CulturaBrasil) : string.Empty;
        }
    }
}
