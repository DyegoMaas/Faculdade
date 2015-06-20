namespace ManipulaImagem
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code


        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.Classificacao = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.imagem = new Emgu.CV.UI.ImageBox();
            this.btArquivoClassificacao = new System.Windows.Forms.Button();
            this.caminhoArquivo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tbDiretorioTreinamento = new System.Windows.Forms.TextBox();
            this.btDiretorioTreinamento = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.Classificacao.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imagem)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Classificacao
            // 
            this.Classificacao.AccessibleDescription = "";
            this.Classificacao.AccessibleName = "";
            this.Classificacao.Controls.Add(this.tabPage1);
            this.Classificacao.Controls.Add(this.tabPage2);
            this.Classificacao.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Classificacao.Location = new System.Drawing.Point(0, 0);
            this.Classificacao.Name = "Classificacao";
            this.Classificacao.SelectedIndex = 0;
            this.Classificacao.Size = new System.Drawing.Size(806, 657);
            this.Classificacao.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.imagem);
            this.tabPage1.Controls.Add(this.btArquivoClassificacao);
            this.tabPage1.Controls.Add(this.caminhoArquivo);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(798, 631);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Classificação";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // imagem
            // 
            this.imagem.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.imagem.Cursor = System.Windows.Forms.Cursors.Cross;
            this.imagem.Location = new System.Drawing.Point(9, 32);
            this.imagem.Name = "imagem";
            this.imagem.Size = new System.Drawing.Size(540, 334);
            this.imagem.TabIndex = 4;
            this.imagem.TabStop = false;
            // 
            // btArquivoClassificacao
            // 
            this.btArquivoClassificacao.Location = new System.Drawing.Point(326, 4);
            this.btArquivoClassificacao.Name = "btArquivoClassificacao";
            this.btArquivoClassificacao.Size = new System.Drawing.Size(25, 23);
            this.btArquivoClassificacao.TabIndex = 3;
            this.btArquivoClassificacao.Text = "...";
            this.btArquivoClassificacao.UseVisualStyleBackColor = true;
            this.btArquivoClassificacao.Click += new System.EventHandler(this.btArquivoClassificacao_Click);
            // 
            // caminhoArquivo
            // 
            this.caminhoArquivo.Location = new System.Drawing.Point(58, 6);
            this.caminhoArquivo.Name = "caminhoArquivo";
            this.caminhoArquivo.ReadOnly = true;
            this.caminhoArquivo.Size = new System.Drawing.Size(262, 20);
            this.caminhoArquivo.TabIndex = 2;
            this.caminhoArquivo.TextChanged += new System.EventHandler(this.caminhoArquivo_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Arquivo:";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tbDiretorioTreinamento);
            this.tabPage2.Controls.Add(this.btDiretorioTreinamento);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(798, 631);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Treinamento";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tbDiretorioTreinamento
            // 
            this.tbDiretorioTreinamento.Location = new System.Drawing.Point(8, 9);
            this.tbDiretorioTreinamento.Name = "tbDiretorioTreinamento";
            this.tbDiretorioTreinamento.ReadOnly = true;
            this.tbDiretorioTreinamento.Size = new System.Drawing.Size(373, 20);
            this.tbDiretorioTreinamento.TabIndex = 3;
            this.tbDiretorioTreinamento.TextChanged += new System.EventHandler(this.tbDiretorioTreinamento_TextChanged);
            // 
            // btDiretorioTreinamento
            // 
            this.btDiretorioTreinamento.Location = new System.Drawing.Point(387, 7);
            this.btDiretorioTreinamento.Name = "btDiretorioTreinamento";
            this.btDiretorioTreinamento.Size = new System.Drawing.Size(31, 23);
            this.btDiretorioTreinamento.TabIndex = 0;
            this.btDiretorioTreinamento.Text = "...";
            this.btDiretorioTreinamento.UseVisualStyleBackColor = true;
            this.btDiretorioTreinamento.Click += new System.EventHandler(this.btDiretorioTreinamento_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 657);
            this.Controls.Add(this.Classificacao);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Classificacao.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imagem)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TabControl Classificacao;
        private System.Windows.Forms.TabPage tabPage1;
        private Emgu.CV.UI.ImageBox imagem;
        private System.Windows.Forms.Button btArquivoClassificacao;
        private System.Windows.Forms.TextBox caminhoArquivo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox tbDiretorioTreinamento;
        private System.Windows.Forms.Button btDiretorioTreinamento;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}

