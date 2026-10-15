namespace ListaTarefas.UI
{
    partial class FormPrincipal
    {
        private System.ComponentModel.IContainer components = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            dgvTarefas = new DataGridView();
            colTitulo = new DataGridViewTextBoxColumn();
            colInicio = new DataGridViewTextBoxColumn();
            colFim = new DataGridViewTextBoxColumn();
            colSituacao = new DataGridViewTextBoxColumn();
            pnlBotoes = new FlowLayoutPanel();
            btnNova = new Button();
            btnEditar = new Button();
            btnConcluir = new Button();
            btnSubir = new Button();
            btnDescer = new Button();
            btnExcluir = new Button();
            chkMostrarConcluidas = new CheckBox();
            lblResumo = new Label();
            ttDicas = new ToolTip(components);
            ((System.ComponentModel.ISupportInitialize)dgvTarefas).BeginInit();
            pnlBotoes.SuspendLayout();
            SuspendLayout();
            //
            // dgvTarefas
            //
            dgvTarefas.AllowUserToAddRows = false;
            dgvTarefas.AllowUserToDeleteRows = false;
            dgvTarefas.AllowUserToResizeRows = false;
            dgvTarefas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvTarefas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTarefas.BackgroundColor = SystemColors.Window;
            dgvTarefas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTarefas.Columns.AddRange(new DataGridViewColumn[] { colTitulo, colInicio, colFim, colSituacao });
            dgvTarefas.Location = new Point(12, 12);
            dgvTarefas.MultiSelect = false;
            dgvTarefas.Name = "dgvTarefas";
            dgvTarefas.ReadOnly = true;
            dgvTarefas.RowHeadersVisible = false;
            dgvTarefas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTarefas.Size = new Size(620, 380);
            dgvTarefas.StandardTab = true;
            dgvTarefas.TabIndex = 0;
            dgvTarefas.CellDoubleClick += dgvTarefas_CellDoubleClick;
            dgvTarefas.SelectionChanged += dgvTarefas_SelectionChanged;
            //
            // colTitulo
            //
            colTitulo.FillWeight = 55F;
            colTitulo.HeaderText = "Tarefa";
            colTitulo.Name = "colTitulo";
            colTitulo.ReadOnly = true;
            colTitulo.SortMode = DataGridViewColumnSortMode.NotSortable;
            //
            // colInicio
            //
            colInicio.FillWeight = 15F;
            colInicio.HeaderText = "Início";
            colInicio.Name = "colInicio";
            colInicio.ReadOnly = true;
            colInicio.SortMode = DataGridViewColumnSortMode.NotSortable;
            //
            // colFim
            //
            colFim.FillWeight = 15F;
            colFim.HeaderText = "Término";
            colFim.Name = "colFim";
            colFim.ReadOnly = true;
            colFim.SortMode = DataGridViewColumnSortMode.NotSortable;
            //
            // colSituacao
            //
            colSituacao.FillWeight = 15F;
            colSituacao.HeaderText = "Situação";
            colSituacao.Name = "colSituacao";
            colSituacao.ReadOnly = true;
            colSituacao.SortMode = DataGridViewColumnSortMode.NotSortable;
            //
            // pnlBotoes
            //
            pnlBotoes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            pnlBotoes.Controls.Add(btnNova);
            pnlBotoes.Controls.Add(btnEditar);
            pnlBotoes.Controls.Add(btnConcluir);
            pnlBotoes.Controls.Add(btnSubir);
            pnlBotoes.Controls.Add(btnDescer);
            pnlBotoes.Controls.Add(btnExcluir);
            pnlBotoes.Controls.Add(chkMostrarConcluidas);
            pnlBotoes.FlowDirection = FlowDirection.TopDown;
            pnlBotoes.Location = new Point(642, 12);
            pnlBotoes.Name = "pnlBotoes";
            pnlBotoes.Size = new Size(150, 380);
            pnlBotoes.TabIndex = 1;
            pnlBotoes.WrapContents = false;
            //
            // btnNova
            //
            btnNova.Name = "btnNova";
            btnNova.Size = new Size(140, 32);
            btnNova.TabIndex = 0;
            btnNova.Text = "&Nova tarefa";
            btnNova.UseVisualStyleBackColor = true;
            btnNova.Click += btnNova_Click;
            //
            // btnEditar
            //
            btnEditar.Name = "btnEditar";
            btnEditar.Size = new Size(140, 32);
            btnEditar.TabIndex = 1;
            btnEditar.Text = "&Editar";
            btnEditar.UseVisualStyleBackColor = true;
            btnEditar.Click += btnEditar_Click;
            //
            // btnConcluir
            //
            btnConcluir.Name = "btnConcluir";
            btnConcluir.Size = new Size(140, 32);
            btnConcluir.TabIndex = 2;
            btnConcluir.Text = "&Concluir";
            btnConcluir.UseVisualStyleBackColor = true;
            btnConcluir.Click += btnConcluir_Click;
            //
            // btnSubir
            //
            btnSubir.Name = "btnSubir";
            btnSubir.Size = new Size(140, 32);
            btnSubir.TabIndex = 3;
            btnSubir.Text = "▲ &Subir";
            btnSubir.UseVisualStyleBackColor = true;
            btnSubir.Click += btnSubir_Click;
            //
            // btnDescer
            //
            btnDescer.Name = "btnDescer";
            btnDescer.Size = new Size(140, 32);
            btnDescer.TabIndex = 4;
            btnDescer.Text = "▼ &Descer";
            btnDescer.UseVisualStyleBackColor = true;
            btnDescer.Click += btnDescer_Click;
            //
            // btnExcluir
            //
            btnExcluir.Name = "btnExcluir";
            btnExcluir.Size = new Size(140, 32);
            btnExcluir.TabIndex = 5;
            btnExcluir.Text = "E&xcluir";
            btnExcluir.UseVisualStyleBackColor = true;
            btnExcluir.Click += btnExcluir_Click;
            //
            // chkMostrarConcluidas
            //
            chkMostrarConcluidas.AutoSize = true;
            chkMostrarConcluidas.Margin = new Padding(3, 16, 3, 3);
            chkMostrarConcluidas.Name = "chkMostrarConcluidas";
            chkMostrarConcluidas.TabIndex = 6;
            chkMostrarConcluidas.Text = "&Mostrar concluídas";
            chkMostrarConcluidas.UseVisualStyleBackColor = true;
            chkMostrarConcluidas.CheckedChanged += chkMostrarConcluidas_CheckedChanged;
            //
            // lblResumo
            //
            lblResumo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblResumo.Location = new Point(12, 400);
            lblResumo.Name = "lblResumo";
            lblResumo.Size = new Size(780, 40);
            lblResumo.TabIndex = 2;
            lblResumo.Text = "";
            //
            // FormPrincipal
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(804, 446);
            Controls.Add(dgvTarefas);
            Controls.Add(pnlBotoes);
            Controls.Add(lblResumo);
            KeyPreview = true;
            MinimumSize = new Size(700, 400);
            Name = "FormPrincipal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Lista de Tarefas";
            Load += FormPrincipal_Load;
            ((System.ComponentModel.ISupportInitialize)dgvTarefas).EndInit();
            pnlBotoes.ResumeLayout(false);
            pnlBotoes.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvTarefas;
        private DataGridViewTextBoxColumn colTitulo;
        private DataGridViewTextBoxColumn colInicio;
        private DataGridViewTextBoxColumn colFim;
        private DataGridViewTextBoxColumn colSituacao;
        private FlowLayoutPanel pnlBotoes;
        private Button btnNova;
        private Button btnEditar;
        private Button btnConcluir;
        private Button btnSubir;
        private Button btnDescer;
        private Button btnExcluir;
        private CheckBox chkMostrarConcluidas;
        private Label lblResumo;
        private ToolTip ttDicas;
    }
}
