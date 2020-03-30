namespace ContractCreate
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
            this.label1 = new System.Windows.Forms.Label();
            this.contracttextbox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tokentextbox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.publickeylabel = new System.Windows.Forms.Label();
            this.DgbBalancelabel = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.AmountTextBox = new System.Windows.Forms.TextBox();
            this.NoOfDecimalTextBox = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.progressLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("TH SarabunPSK", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label1.Location = new System.Drawing.Point(290, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(234, 43);
            this.label1.TabIndex = 0;
            this.label1.Text = "สัญญาเงินอิเล็กทรอนิกส์";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // contracttextbox
            // 
            this.contracttextbox.BackColor = System.Drawing.SystemColors.Menu;
            this.contracttextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.contracttextbox.Location = new System.Drawing.Point(357, 124);
            this.contracttextbox.Name = "contracttextbox";
            this.contracttextbox.Size = new System.Drawing.Size(240, 26);
            this.contracttextbox.TabIndex = 1;
            this.contracttextbox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("TH SarabunPSK", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label2.ForeColor = System.Drawing.SystemColors.MenuText;
            this.label2.Location = new System.Drawing.Point(224, 120);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 33);
            this.label2.TabIndex = 2;
            this.label2.Text = "ชื่อสัญญา";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("TH SarabunPSK", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label3.ForeColor = System.Drawing.SystemColors.MenuText;
            this.label3.Location = new System.Drawing.Point(224, 167);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 33);
            this.label3.TabIndex = 4;
            this.label3.Text = "หน่วยนับ";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // tokentextbox
            // 
            this.tokentextbox.BackColor = System.Drawing.SystemColors.Menu;
            this.tokentextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.tokentextbox.Location = new System.Drawing.Point(357, 174);
            this.tokentextbox.Name = "tokentextbox";
            this.tokentextbox.Size = new System.Drawing.Size(240, 26);
            this.tokentextbox.TabIndex = 3;
            this.tokentextbox.TextChanged += new System.EventHandler(this.tokentextbox_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("TH SarabunPSK", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label4.ForeColor = System.Drawing.SystemColors.MenuText;
            this.label4.Location = new System.Drawing.Point(224, 216);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 33);
            this.label4.TabIndex = 6;
            this.label4.Text = "วงเงิน";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("TH SarabunPSK", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label5.ForeColor = System.Drawing.SystemColors.MenuText;
            this.label5.Location = new System.Drawing.Point(224, 263);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(108, 33);
            this.label5.TabIndex = 8;
            this.label5.Text = "จำนวนทศนิยม";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.button1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button1.Location = new System.Drawing.Point(357, 328);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(89, 39);
            this.button1.TabIndex = 11;
            this.button1.Text = "สร้าง";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("TH SarabunPSK", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(12, 74);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(128, 33);
            this.label7.TabIndex = 12;
            this.label7.Text = "กุญแจสาธารณะ :";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // publickeylabel
            // 
            this.publickeylabel.AutoSize = true;
            this.publickeylabel.BackColor = System.Drawing.SystemColors.Control;
            this.publickeylabel.Font = new System.Drawing.Font("TH SarabunPSK", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.publickeylabel.ForeColor = System.Drawing.SystemColors.MenuText;
            this.publickeylabel.Location = new System.Drawing.Point(148, 74);
            this.publickeylabel.Name = "publickeylabel";
            this.publickeylabel.Size = new System.Drawing.Size(57, 33);
            this.publickeylabel.TabIndex = 13;
            this.publickeylabel.Text = "NONE";
            this.publickeylabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.publickeylabel.Click += new System.EventHandler(this.label8_Click);
            // 
            // DgbBalancelabel
            // 
            this.DgbBalancelabel.AutoSize = true;
            this.DgbBalancelabel.BackColor = System.Drawing.SystemColors.Control;
            this.DgbBalancelabel.Font = new System.Drawing.Font("TH SarabunPSK", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.DgbBalancelabel.ForeColor = System.Drawing.SystemColors.MenuText;
            this.DgbBalancelabel.Location = new System.Drawing.Point(626, 74);
            this.DgbBalancelabel.Name = "DgbBalancelabel";
            this.DgbBalancelabel.Size = new System.Drawing.Size(24, 33);
            this.DgbBalancelabel.TabIndex = 15;
            this.DgbBalancelabel.Text = "0";
            this.DgbBalancelabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.DgbBalancelabel.Click += new System.EventHandler(this.label9_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("TH SarabunPSK", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label10.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label10.Location = new System.Drawing.Point(533, 74);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(80, 33);
            this.label10.TabIndex = 14;
            this.label10.Text = "Digibyte :";
            this.label10.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // AmountTextBox
            // 
            this.AmountTextBox.BackColor = System.Drawing.SystemColors.Menu;
            this.AmountTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.AmountTextBox.Location = new System.Drawing.Point(357, 223);
            this.AmountTextBox.Name = "AmountTextBox";
            this.AmountTextBox.Size = new System.Drawing.Size(240, 26);
            this.AmountTextBox.TabIndex = 16;
            this.AmountTextBox.TextChanged += new System.EventHandler(this.AmountTextBox_TextChanged);
            this.AmountTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AmountTextBox_KeyDown);
            this.AmountTextBox.Leave += new System.EventHandler(this.AmountTextBox_Leave);
            // 
            // NoOfDecimalTextBox
            // 
            this.NoOfDecimalTextBox.BackColor = System.Drawing.SystemColors.Menu;
            this.NoOfDecimalTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.NoOfDecimalTextBox.Location = new System.Drawing.Point(357, 270);
            this.NoOfDecimalTextBox.Name = "NoOfDecimalTextBox";
            this.NoOfDecimalTextBox.Size = new System.Drawing.Size(240, 26);
            this.NoOfDecimalTextBox.TabIndex = 17;
            this.NoOfDecimalTextBox.TextChanged += new System.EventHandler(this.NoOfDecimalTextBox_TextChanged);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(608, 22);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(157, 23);
            this.progressBar1.TabIndex = 18;
            // 
            // progressLabel
            // 
            this.progressLabel.Font = new System.Drawing.Font("TH SarabunPSK", 12F);
            this.progressLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.progressLabel.Location = new System.Drawing.Point(587, 48);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(201, 30);
            this.progressLabel.TabIndex = 45;
            this.progressLabel.Text = "กำลังสร้างสัญญา";
            this.progressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.progressLabel.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(800, 390);
            this.Controls.Add(this.progressLabel);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.NoOfDecimalTextBox);
            this.Controls.Add(this.AmountTextBox);
            this.Controls.Add(this.DgbBalancelabel);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.publickeylabel);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tokentextbox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.contracttextbox);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "โปรแกรมสร้างสัญญาเงินอิเล็กทรอนิกส์บนบล็อกเชน";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox contracttextbox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tokentextbox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label publickeylabel;
        private System.Windows.Forms.Label DgbBalancelabel;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox AmountTextBox;
        private System.Windows.Forms.TextBox NoOfDecimalTextBox;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label progressLabel;
    }
}

