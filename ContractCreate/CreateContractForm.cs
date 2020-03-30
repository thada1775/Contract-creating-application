using NBitcoin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Thaismartcontract.API;
using Thaismartcontract.WalletService;
using Thaismartcontract.WalletService.Model;

namespace ContractCreate
{
    public partial class CreateContractForm : Form
    {
        private KeyService keyService;
        private CryptoKeyPair currentKeyPair;
        private BitcoinSecret privateKey;
        private ContractService contractService;
        private WalletContract MyContract;
        public string publicKey;
        private string currentContract;
        private bool CoppyButton = false;
        
        public CreateContractForm(string currentContract)
        {
            InitializeComponent();
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            this.button1.Image = (Image)(new Bitmap(ContractCreate.Properties.Resources.document, new Size(32, 32)));
            this.button3.Image = (Image)(new Bitmap(ContractCreate.Properties.Resources.save, new Size(20, 20)));


            this.currentContract = currentContract;
            InitializeContract();
        }
        public async void InitializeContract()
        {
            var api = new DigibyteAPI(new APIOptions { BaseURL = Program.InsightAPI });
            contractService = new ContractService(@"tsc-wallet.db", api);
            MyContract = contractService.LoadContract(currentContract);
            
            if (MyContract != null)
            {
                StatusLabel.Text = "สร้างสำเร็จ";
                ContractIDtextBox.Text = MyContract.ID;
                NameLabel.Text = MyContract.NameString;
                TokenLabel.Text = MyContract.TokenString;
                TotalSupplyLabel.Text = MyContract.TotalSupply.ToString();

                NoOfDecimalLabel.Text = MyContract.NoOfDecimal.ToString();
                OwnerPublicLabel.Text = MyContract.OwnerPublicAddress;
                GenerateQrCode();
                CoppyButton = true;
            }
            else
            {
                StatusLabel.Text = "สร้างไม่สำเร็จ";
                StatusLabel.ForeColor = Color.Red;
                ContractIDtextBox.Text = currentContract;
                NameLabel.Text = "None";
                TokenLabel.Text = "None";
                TotalSupplyLabel.Text = "None";
                NoOfDecimalLabel.Text = "None";
                OwnerPublicLabel.Text = "None";
                CoppyButton = false;
            }
        }
        Bitmap newImage;
        private void GenerateQrCode()
        {

            Zen.Barcode.CodeQrBarcodeDraw qrcode = Zen.Barcode.BarcodeDrawFactory.CodeQr;
            Image image = qrcode.Draw(MyContract.ID, 32);
            pictureBox1.Image = image;

            newImage = new Bitmap(250, 250);
            using (Graphics graphics = Graphics.FromImage(newImage))
            {
                graphics.Clear(Color.White);
                int x = (newImage.Width - image.Width) / 2;
                int y = (newImage.Height - image.Height) / 2;
                graphics.DrawImage(image, x, y);
                newImage.Save("completed1.jpg");
            }

        }

        private void button1_Click(object sender, EventArgs e)      //Coppy Contract id to clipbroad
        {
            if (CoppyButton)
            {
                System.Windows.Forms.Clipboard.SetText(MyContract.ID);
                MessageBox.Show("คัดลอกรหัสสัญญาเงินอิเล็กทรอนิกส์สำเร็จ");
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = @"C:\";
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.FileName = MyContract.NameString;
            saveFileDialog1.CheckPathExists = true;

            saveFileDialog1.Filter = "Images|*.png;*.bmp;*.jpg";
            ImageFormat format = ImageFormat.Png;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string ext = System.IO.Path.GetExtension(saveFileDialog1.FileName);
                switch (ext)
                {
                    case ".jpg":
                        format = ImageFormat.Jpeg;
                        break;
                    case ".png":
                        format = ImageFormat.Png;
                        break;
                }
                newImage.Save(saveFileDialog1.FileName, format);
            }
            //MessageBox.Show("คัดลอกสำเร็จ");
        }
    }
}
