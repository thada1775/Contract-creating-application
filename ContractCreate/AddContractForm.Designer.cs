namespace ContractCreate
{
    partial class AddContractForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.contracttextbox = new System.Windows.Forms.TextBox();
            this.addbutton = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.deletebutton = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.contractname = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tokenname = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("TH SarabunPSK", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label1.Location = new System.Drawing.Point(42, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(272, 43);
            this.label1.TabIndex = 17;
            this.label1.Text = "รหัสสัญญาเงินอิเล็กทรอนิกส์";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("TH SarabunPSK", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label2.ForeColor = System.Drawing.SystemColors.MenuText;
            this.label2.Location = new System.Drawing.Point(44, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 33);
            this.label2.TabIndex = 19;
            this.label2.Text = "รหัสสัญญา";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // contracttextbox
            // 
            this.contracttextbox.BackColor = System.Drawing.SystemColors.Menu;
            this.contracttextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.contracttextbox.Location = new System.Drawing.Point(136, 82);
            this.contracttextbox.Name = "contracttextbox";
            this.contracttextbox.Size = new System.Drawing.Size(443, 35);
            this.contracttextbox.TabIndex = 18;
            // 
            // addbutton
            // 
            this.addbutton.Font = new System.Drawing.Font("TH SarabunPSK", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.addbutton.Location = new System.Drawing.Point(596, 81);
            this.addbutton.Name = "addbutton";
            this.addbutton.Size = new System.Drawing.Size(75, 38);
            this.addbutton.TabIndex = 20;
            this.addbutton.Text = "เพิ่ม";
            this.addbutton.UseVisualStyleBackColor = true;
            this.addbutton.Click += new System.EventHandler(this.addbutton_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(50, 173);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(714, 278);
            this.dataGridView1.TabIndex = 21;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Edit);
            // 
            // deletebutton
            // 
            this.deletebutton.Font = new System.Drawing.Font("TH SarabunPSK", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.deletebutton.Location = new System.Drawing.Point(689, 81);
            this.deletebutton.Name = "deletebutton";
            this.deletebutton.Size = new System.Drawing.Size(75, 38);
            this.deletebutton.TabIndex = 22;
            this.deletebutton.Text = "ลบ";
            this.deletebutton.UseVisualStyleBackColor = true;
            this.deletebutton.Click += new System.EventHandler(this.deletebutton_Click);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("TH SarabunPSK", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.button3.Location = new System.Drawing.Point(689, 473);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 38);
            this.button3.TabIndex = 24;
            this.button3.Text = "ปิด";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // contractname
            // 
            this.contractname.AutoSize = true;
            this.contractname.BackColor = System.Drawing.SystemColors.Control;
            this.contractname.Font = new System.Drawing.Font("TH SarabunPSK", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.contractname.ForeColor = System.Drawing.SystemColors.MenuText;
            this.contractname.Location = new System.Drawing.Point(215, 137);
            this.contractname.Name = "contractname";
            this.contractname.Size = new System.Drawing.Size(24, 33);
            this.contractname.TabIndex = 35;
            this.contractname.Text = "0";
            this.contractname.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("TH SarabunPSK", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label3.Location = new System.Drawing.Point(130, 137);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 33);
            this.label3.TabIndex = 34;
            this.label3.Text = "ชื่อสัญญา:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tokenname
            // 
            this.tokenname.AutoSize = true;
            this.tokenname.BackColor = System.Drawing.SystemColors.Control;
            this.tokenname.Font = new System.Drawing.Font("TH SarabunPSK", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.tokenname.ForeColor = System.Drawing.SystemColors.MenuText;
            this.tokenname.Location = new System.Drawing.Point(527, 137);
            this.tokenname.Name = "tokenname";
            this.tokenname.Size = new System.Drawing.Size(24, 33);
            this.tokenname.TabIndex = 37;
            this.tokenname.Text = "0";
            this.tokenname.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("TH SarabunPSK", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label5.Location = new System.Drawing.Point(444, 137);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 33);
            this.label5.TabIndex = 36;
            this.label5.Text = "หน่วยนับ:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // AddContractForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(857, 546);
            this.Controls.Add(this.tokenname);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.contractname);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.deletebutton);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.addbutton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.contracttextbox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddContractForm";
            this.Text = "โปรแกรมสร้างสัญญาเงินอิเล็กทรอนิกส์บนบล็อกเชน";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AddContractForm_FormClosing);
            this.Load += new System.EventHandler(this.AddContractForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox contracttextbox;
        private System.Windows.Forms.Button addbutton;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button deletebutton;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label contractname;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label tokenname;
        private System.Windows.Forms.Label label5;
    }
}