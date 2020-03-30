using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Thaismartcontract.API;
using Thaismartcontract.WalletService;

namespace ContractCreate
{
    public partial class SaveKeyForm : Form
    {
        private KeyService keyService;
        private CryptoKeyPair currentKeyPair;
        public SaveKeyForm()
        {
            InitializeComponent();
            InitializeKey();
        }
        public void InitializeKey()
        {
            keyService = new KeyService("1234");
            button1.Enabled = false;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("บันทึกกุญแจหรือไม่", "ยืนยัน", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if(result == DialogResult.OK)
            {
                keyService.SaveKey(currentKeyPair);
                this.Close();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var result = keyService.ParsePrivateKey(textBox1.Text);
            if(result != null)
            {
                currentKeyPair = result;
                textBox2.Text = result.PublicKeyWif;
                button1.Enabled = true;
            }
            else
            {
                textBox2.Text = null;
                button1.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (keyService.GetKey() == null)
            {
                Environment.Exit(0);
            }
            else
            {
                this.Close();
            }
        }
    }
}
