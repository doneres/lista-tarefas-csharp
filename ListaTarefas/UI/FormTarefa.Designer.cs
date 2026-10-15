namespace ListaTarefas.UI
{
    partial class FormTarefa
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
            lblTitulo = new Label();
            txtTitulo = new TextBox();
            lblDescricao = new Label();
            txtDescricao = new TextBox();
            lblDataInicio = new Label();
            mtbDataInicio = new MaskedTextBox();
            lblDataFim = new Label();
            mtbDataFim = new MaskedTextBox();
            lblDicaDatas = new Label();
            btnSalvar = new Button();
            btnCancelar = new Button();
            errValidacao = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)errValidacao).BeginInit();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(16, 18);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "&Título:";
            //
            // txtTitulo
            //
            txtTitulo.Location = new Point(16, 38);
            txtTitulo.MaxLength = 100;
            txtTitulo.Name = "txtTitulo";
            txtTitulo.Size = new Size(400, 23);
            txtTitulo.TabIndex = 1;
            //
            // lblDescricao
            //
            lblDescricao.AutoSize = true;
            lblDescricao.Location = new Point(16, 72);
            lblDescricao.Name = "lblDescricao";
            lblDescricao.TabIndex = 2;
            lblDescricao.Text = "&Descrição (opcional):";
            //
            // txtDescricao
            //
            txtDescricao.Location = new Point(16, 92);
            txtDescricao.MaxLength = 500;
            txtDescricao.Multiline = true;
            txtDescricao.Name = "txtDescricao";
            txtDescricao.ScrollBars = ScrollBars.Vertical;
            txtDescricao.Size = new Size(400, 70);
            txtDescricao.TabIndex = 3;
            //
            // lblDataInicio
            //
            lblDataInicio.AutoSize = true;
            lblDataInicio.Location = new Point(16, 174);
            lblDataInicio.Name = "lblDataInicio";
            lblDataInicio.TabIndex = 4;
            lblDataInicio.Text = "Data de &início:";
            //
            // mtbDataInicio
            //
            mtbDataInicio.Location = new Point(16, 194);
            mtbDataInicio.Mask = "00/00/0000";
            mtbDataInicio.Name = "mtbDataInicio";
            mtbDataInicio.Size = new Size(110, 23);
            mtbDataInicio.TabIndex = 5;
            //
            // lblDataFim
            //
            lblDataFim.AutoSize = true;
            lblDataFim.Location = new Point(160, 174);
            lblDataFim.Name = "lblDataFim";
            lblDataFim.TabIndex = 6;
            lblDataFim.Text = "Data de t&érmino:";
            //
            // mtbDataFim
            //
            mtbDataFim.Location = new Point(160, 194);
            mtbDataFim.Mask = "00/00/0000";
            mtbDataFim.Name = "mtbDataFim";
            mtbDataFim.Size = new Size(110, 23);
            mtbDataFim.TabIndex = 7;
            //
            // lblDicaDatas
            //
            lblDicaDatas.AutoSize = true;
            lblDicaDatas.ForeColor = SystemColors.GrayText;
            lblDicaDatas.Location = new Point(16, 224);
            lblDicaDatas.Name = "lblDicaDatas";
            lblDicaDatas.TabIndex = 8;
            lblDicaDatas.Text = "Datas são opcionais (dd/mm/aaaa). Deixe em branco se não houver.";
            //
            // btnSalvar
            //
            btnSalvar.Location = new Point(236, 258);
            btnSalvar.Name = "btnSalvar";
            btnSalvar.Size = new Size(87, 30);
            btnSalvar.TabIndex = 9;
            btnSalvar.Text = "&Salvar";
            btnSalvar.UseVisualStyleBackColor = true;
            btnSalvar.Click += btnSalvar_Click;
            //
            // btnCancelar
            //
            btnCancelar.CausesValidation = false;
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(329, 258);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(87, 30);
            btnCancelar.TabIndex = 10;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            //
            // errValidacao
            //
            errValidacao.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errValidacao.ContainerControl = this;
            //
            // FormTarefa
            //
            AcceptButton = btnSalvar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(440, 304);
            Controls.Add(lblTitulo);
            Controls.Add(txtTitulo);
            Controls.Add(lblDescricao);
            Controls.Add(txtDescricao);
            Controls.Add(lblDataInicio);
            Controls.Add(mtbDataInicio);
            Controls.Add(lblDataFim);
            Controls.Add(mtbDataFim);
            Controls.Add(lblDicaDatas);
            Controls.Add(btnSalvar);
            Controls.Add(btnCancelar);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormTarefa";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Tarefa";
            Load += FormTarefa_Load;
            ((System.ComponentModel.ISupportInitialize)errValidacao).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private TextBox txtTitulo;
        private Label lblDescricao;
        private TextBox txtDescricao;
        private Label lblDataInicio;
        private MaskedTextBox mtbDataInicio;
        private Label lblDataFim;
        private MaskedTextBox mtbDataFim;
        private Label lblDicaDatas;
        private Button btnSalvar;
        private Button btnCancelar;
        private ErrorProvider errValidacao;
    }
}
