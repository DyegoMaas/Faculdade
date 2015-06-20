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
            this.label1 = new System.Windows.Forms.Label();
            this.caminhoArquivo = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.imagem = new Emgu.CV.UI.ImageBox();
            ((System.ComponentModel.ISupportInitialize)(this.imagem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imagem)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Arquivo:";
            // 
            // caminhoArquivo
            // 
            this.caminhoArquivo.Location = new System.Drawing.Point(65, 10);
            this.caminhoArquivo.Name = "caminhoArquivo";
            this.caminhoArquivo.ReadOnly = true;
            this.caminhoArquivo.Size = new System.Drawing.Size(262, 20);
            this.caminhoArquivo.TabIndex = 1;
            this.caminhoArquivo.TextChanged += caminhoArquivo_TextChanged;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(333, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(25, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // imagem
            // 
            this.imagem.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.imagem.Cursor = System.Windows.Forms.Cursors.Cross;
            this.imagem.Location = new System.Drawing.Point(16, 39);
            this.imagem.Name = "pictureBox1";
            this.imagem.Size = new System.Drawing.Size(540, 334);
            this.imagem.TabIndex = 3;
            this.imagem.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 385);
            this.Controls.Add(this.imagem);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.caminhoArquivo);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.imagem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imagem)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void caminhoArquivo_TextChanged(object sender, System.EventArgs e)
        {
            this.iniciaTratamentoImagem();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox caminhoArquivo;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;

        private Emgu.CV.UI.ImageBox imagem;
    }
}

