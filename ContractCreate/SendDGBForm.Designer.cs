namespace ContractCreate
{
    partial class SendDGBForm
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.sendButton = new System.Windows.Forms.Button();
            this.cancleButton = new System.Windows.Forms.Button();
            this.totalwalletlabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.totalDigibyteLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.leftBalanceLabel = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.progressLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("TH SarabunPSK", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label1.Location = new System.Drawing.Point(201, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(163, 43);
            this.label1.TabIndex = 18;
            this.label1.Text = "ส่งเงิน Digibyte";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(69, 184);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(433, 180);
            this.dataGridView1.TabIndex = 19;
            // 
            // sendButton
            // 
            this.sendButton.Font = new System.Drawing.Font("TH SarabunPSK", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.sendButton.Location = new System.Drawing.Point(336, 391);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(75, 41);
            this.sendButton.TabIndex = 20;
            this.sendButton.Text = "ส่ง";
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += new System.EventHandler(this.sendButton_Click);
            // 
            // cancleButton
            // 
            this.cancleButton.Font = new System.Drawing.Font("TH SarabunPSK", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.cancleButton.Location = new System.Drawing.Point(427, 391);
            this.cancleButton.Name = "cancleButton";
            this.cancleButton.Size = new System.Drawing.Size(75, 41);
            this.cancleButton.TabIndex = 21;
            this.cancleButton.Text = "ยกเลิก";
            this.cancleButton.UseVisualStyleBackColor = true;
            this.cancleButton.Click += new System.EventHandler(this.cancleButton_Click);
            // 
            // totalwalletlabel
            // 
            this.totalwalletlabel.AutoSize = true;
            this.totalwalletlabel.BackColor = System.Drawing.SystemColors.Control;
            this.totalwalletlabel.Font = new System.Drawing.Font("TH SarabunPSK", 16F);
            this.totalwalletlabel.ForeColor = System.Drawing.SystemColors.MenuText;
            this.totalwalletlabel.Location = new System.Drawing.Point(248, 65);
            this.totalwalletlabel.Name = "totalwalletlabel";
            this.totalwalletlabel.Size = new System.Drawing.Size(21, 30);
            this.totalwalletlabel.TabIndex = 35;
            this.totalwalletlabel.Text = "0";
            this.totalwalletlabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("TH SarabunPSK", 16F);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label2.Location = new System.Drawing.Point(103, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 30);
            this.label2.TabIndex = 34;
            this.label2.Text = "จำนวนกระเป๋าเงิน:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // totalDigibyteLabel
            // 
            this.totalDigibyteLabel.AutoSize = true;
            this.totalDigibyteLabel.BackColor = System.Drawing.SystemColors.Control;
            this.totalDigibyteLabel.Font = new System.Drawing.Font("TH SarabunPSK", 16F);
            this.totalDigibyteLabel.ForeColor = System.Drawing.SystemColors.MenuText;
            this.totalDigibyteLabel.Location = new System.Drawing.Point(248, 104);
            this.totalDigibyteLabel.Name = "totalDigibyteLabel";
            this.totalDigibyteLabel.Size = new System.Drawing.Size(21, 30);
            this.totalDigibyteLabel.TabIndex = 37;
            this.totalDigibyteLabel.Text = "0";
            this.totalDigibyteLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("TH SarabunPSK", 16F);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label4.Location = new System.Drawing.Point(81, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(141, 30);
            this.label4.TabIndex = 36;
            this.label4.Text = "จำนวนเงินที่ส่งทั้งหมด:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            
            // 
            // leftBalanceLabel
            // 
            this.leftBalanceLabel.AutoSize = true;
            this.leftBalanceLabel.BackColor = System.Drawing.SystemColors.Control;
            this.leftBalanceLabel.Font = new System.Drawing.Font("TH SarabunPSK", 16F);
            this.leftBalanceLabel.ForeColor = System.Drawing.SystemColors.MenuText;
            this.leftBalanceLabel.Location = new System.Drawing.Point(248, 143);
            this.leftBalanceLabel.Name = "leftBalanceLabel";
            this.leftBalanceLabel.Size = new System.Drawing.Size(21, 30);
            this.leftBalanceLabel.TabIndex = 39;
            this.leftBalanceLabel.Text = "0";
            this.leftBalanceLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("TH SarabunPSK", 16F);
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label6.Location = new System.Drawing.Point(160, 143);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 30);
            this.label6.TabIndex = 38;
            this.label6.Text = "คงเหลือ:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(374, 33);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(128, 23);
            this.progressBar1.TabIndex = 40;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("TH SarabunPSK", 16F);
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label5.Location = new System.Drawing.Point(463, 104);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 30);
            this.label5.TabIndex = 42;
            this.label5.Text = "DGB";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("TH SarabunPSK", 16F);
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label7.Location = new System.Drawing.Point(463, 143);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(39, 30);
            this.label7.TabIndex = 43;
            this.label7.Text = "DGB";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // progressLabel
            // 
            this.progressLabel.Font = new System.Drawing.Font("TH SarabunPSK", 12F);
            this.progressLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.progressLabel.Location = new System.Drawing.Point(354, 59);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(168, 30);
            this.progressLabel.TabIndex = 44;
            this.progressLabel.Text = "กำลังส่ง";
            this.progressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.progressLabel.Visible = false;
            // 
            // SendDGBForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(579, 460);
            this.Controls.Add(this.progressLabel);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.leftBalanceLabel);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.totalDigibyteLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.totalwalletlabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cancleButton);
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SendDGBForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "โปรแกรมสร้างสัญญาเงินอิเล็กทรอนิกส์บนบล็อกเชน";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SendDGBForm_FormClosing);
            this.Load += new System.EventHandler(this.SendDGBForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button sendButton;
        private System.Windows.Forms.Button cancleButton;
        private System.Windows.Forms.Label totalwalletlabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label totalDigibyteLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label leftBalanceLabel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label progressLabel;
    }
}