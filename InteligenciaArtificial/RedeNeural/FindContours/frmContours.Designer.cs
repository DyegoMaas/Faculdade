namespace FindContours
{
    partial class FrmContours
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
            this.btnCapture = new System.Windows.Forms.Button();
            this.grpControls = new System.Windows.Forms.GroupBox();
            this.lblThresholdValue = new System.Windows.Forms.Label();
            this.trackbarThreshold = new System.Windows.Forms.TrackBar();
            this.lblThresholdDescription = new System.Windows.Forms.Label();
            this.btnStopCapture = new System.Windows.Forms.Button();
            this.grpColor = new System.Windows.Forms.GroupBox();
            this.grpGray = new System.Windows.Forms.GroupBox();
            this.chkBoxInvert = new System.Windows.Forms.CheckBox();
            this.CameraStreamCapture = new System.Windows.Forms.Timer(this.components);
            this.pictBoxColor = new System.Windows.Forms.PictureBox();
            this.pictBoxGray = new System.Windows.Forms.PictureBox();
            this.grpControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackbarThreshold)).BeginInit();
            this.grpColor.SuspendLayout();
            this.grpGray.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxGray)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCapture
            // 
            this.btnCapture.Location = new System.Drawing.Point(17, 19);
            this.btnCapture.Name = "btnCapture";
            this.btnCapture.Size = new System.Drawing.Size(88, 38);
            this.btnCapture.TabIndex = 0;
            this.btnCapture.Text = "Start Camera Stream";
            this.btnCapture.UseVisualStyleBackColor = true;
            this.btnCapture.Click += new System.EventHandler(this.btnCapture_Click);
            // 
            // grpControls
            // 
            this.grpControls.Controls.Add(this.chkBoxInvert);
            this.grpControls.Controls.Add(this.lblThresholdValue);
            this.grpControls.Controls.Add(this.trackbarThreshold);
            this.grpControls.Controls.Add(this.lblThresholdDescription);
            this.grpControls.Controls.Add(this.btnStopCapture);
            this.grpControls.Controls.Add(this.btnCapture);
            this.grpControls.Location = new System.Drawing.Point(12, 12);
            this.grpControls.Name = "grpControls";
            this.grpControls.Size = new System.Drawing.Size(931, 107);
            this.grpControls.TabIndex = 1;
            this.grpControls.TabStop = false;
            this.grpControls.Text = "Controls";
            // 
            // lblThresholdValue
            // 
            this.lblThresholdValue.AutoSize = true;
            this.lblThresholdValue.Location = new System.Drawing.Point(150, 39);
            this.lblThresholdValue.Name = "lblThresholdValue";
            this.lblThresholdValue.Size = new System.Drawing.Size(25, 13);
            this.lblThresholdValue.TabIndex = 3;
            this.lblThresholdValue.Text = "125";
            // 
            // trackbarThreshold
            // 
            this.trackbarThreshold.Location = new System.Drawing.Point(217, 19);
            this.trackbarThreshold.Maximum = 255;
            this.trackbarThreshold.Name = "trackbarThreshold";
            this.trackbarThreshold.Size = new System.Drawing.Size(708, 45);
            this.trackbarThreshold.TabIndex = 2;
            this.trackbarThreshold.Value = 125;
            this.trackbarThreshold.Scroll += new System.EventHandler(this.trackbarThreshold_Scroll);
            // 
            // lblThresholdDescription
            // 
            this.lblThresholdDescription.AutoSize = true;
            this.lblThresholdDescription.Location = new System.Drawing.Point(127, 19);
            this.lblThresholdDescription.Name = "lblThresholdDescription";
            this.lblThresholdDescription.Size = new System.Drawing.Size(84, 13);
            this.lblThresholdDescription.TabIndex = 1;
            this.lblThresholdDescription.Text = "Threshold Value";
            // 
            // btnStopCapture
            // 
            this.btnStopCapture.Location = new System.Drawing.Point(17, 63);
            this.btnStopCapture.Name = "btnStopCapture";
            this.btnStopCapture.Size = new System.Drawing.Size(88, 38);
            this.btnStopCapture.TabIndex = 0;
            this.btnStopCapture.Text = "Stop Camera Stream";
            this.btnStopCapture.UseVisualStyleBackColor = true;
            this.btnStopCapture.Click += new System.EventHandler(this.btnStopCapture_Click);
            // 
            // grpColor
            // 
            this.grpColor.Controls.Add(this.pictBoxColor);
            this.grpColor.Location = new System.Drawing.Point(13, 128);
            this.grpColor.Name = "grpColor";
            this.grpColor.Size = new System.Drawing.Size(429, 345);
            this.grpColor.TabIndex = 2;
            this.grpColor.TabStop = false;
            this.grpColor.Text = "Color Image";
            // 
            // grpGray
            // 
            this.grpGray.Controls.Add(this.pictBoxGray);
            this.grpGray.Location = new System.Drawing.Point(515, 128);
            this.grpGray.Name = "grpGray";
            this.grpGray.Size = new System.Drawing.Size(429, 345);
            this.grpGray.TabIndex = 2;
            this.grpGray.TabStop = false;
            this.grpGray.Text = "Monochrome";
            // 
            // chkBoxInvert
            // 
            this.chkBoxInvert.AutoSize = true;
            this.chkBoxInvert.Location = new System.Drawing.Point(130, 74);
            this.chkBoxInvert.Name = "chkBoxInvert";
            this.chkBoxInvert.Size = new System.Drawing.Size(85, 17);
            this.chkBoxInvert.TabIndex = 4;
            this.chkBoxInvert.Text = "Invert Image";
            this.chkBoxInvert.UseVisualStyleBackColor = true;
            // 
            // CameraStreamCapture
            // 
            this.CameraStreamCapture.Tick += new System.EventHandler(this.CameraStreamCapture_Tick);
            // 
            // pictBoxColor
            // 
            this.pictBoxColor.Location = new System.Drawing.Point(7, 20);
            this.pictBoxColor.Name = "pictBoxColor";
            this.pictBoxColor.Size = new System.Drawing.Size(416, 319);
            this.pictBoxColor.TabIndex = 0;
            this.pictBoxColor.TabStop = false;
            // 
            // pictBoxGray
            // 
            this.pictBoxGray.Location = new System.Drawing.Point(7, 19);
            this.pictBoxGray.Name = "pictBoxGray";
            this.pictBoxGray.Size = new System.Drawing.Size(416, 319);
            this.pictBoxGray.TabIndex = 0;
            this.pictBoxGray.TabStop = false;
            // 
            // frmContours
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(955, 483);
            this.Controls.Add(this.grpGray);
            this.Controls.Add(this.grpColor);
            this.Controls.Add(this.grpControls);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmContours";
            this.Text = "Contour Extraction";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmContours_FormClosing);
            this.grpControls.ResumeLayout(false);
            this.grpControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackbarThreshold)).EndInit();
            this.grpColor.ResumeLayout(false);
            this.grpGray.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxGray)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCapture;
        private System.Windows.Forms.GroupBox grpControls;
        private System.Windows.Forms.Label lblThresholdValue;
        private System.Windows.Forms.TrackBar trackbarThreshold;
        private System.Windows.Forms.Label lblThresholdDescription;
        private System.Windows.Forms.Button btnStopCapture;
        private System.Windows.Forms.GroupBox grpColor;
        private System.Windows.Forms.GroupBox grpGray;
        private System.Windows.Forms.CheckBox chkBoxInvert;
        private System.Windows.Forms.PictureBox pictBoxColor;
        private System.Windows.Forms.PictureBox pictBoxGray;
        private System.Windows.Forms.Timer CameraStreamCapture;
    }
}

