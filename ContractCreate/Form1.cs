using ContractCreate.Models;
using NBitcoin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Thaismartcontract.API;
using Thaismartcontract.WalletService;
using Thaismartcontract.WalletService.Model;

namespace ContractCreate
{
    public partial class Form1 : Form
    {
        
        private BitcoinSecret privateKey;
        public string publicKey;
        private KeyService keyService;
        private ContractService contractService;
        WalletContract resultContract;

        public string contractID;
        public string issuedID;

        private TransBlockchain transactions;
        private readonly HttpClient client;     //Get Api
        private bool triggerFinish = false;
        private MainForm parentForm;
        
        public Form1(MainForm parentForm)
        {
            InitializeComponent();
            client = new HttpClient();          //Get Api
            client.BaseAddress = new System.Uri(Program.InsightAPI);
            this.parentForm = parentForm;
            button1.Enabled = false;
            progressBar1.Visible = false;
            progressBar1.Minimum = 1;
            progressBar1.Maximum = 7;
            InitializeEMoney();
        }

        public async void InitializeEMoney()
        {
            keyService = new KeyService("1234");
            var saveKey = keyService.GetKey();
            if (saveKey == null)
            {
                Environment.Exit(1);
            }
            privateKey = saveKey.SecretKey;

            publicKey = saveKey.PublicKeyWif;

            publickeylabel.Text = publicKey;

            decimal balance = parentForm.DgbBalance;
            DgbBalancelabel.Text = balance.ToString();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            CheckForm();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
        private void CheckForm()
        {
            if (String.IsNullOrEmpty(contracttextbox.Text) || (String.IsNullOrEmpty(tokentextbox.Text)))
            {
                button1.Enabled = false;
                return;
            }
            if (String.IsNullOrEmpty(AmountTextBox.Text) || String.IsNullOrEmpty(NoOfDecimalTextBox.Text))
            {
                button1.Enabled = false;
                return;
            }

            button1.Enabled = true;
            
        }
        private async void CreateContract()
        {
            InvokeHelp.SetControlPropertyThreadSafe(progressLabel, "Visible", true);
            InvokeHelp.SetControlPropertyThreadSafe(progressLabel, "Text", "กำลังสร้างสัญญาเงินอิเล็กทรอนิกส์");
            var contract = Encoding.UTF8.GetBytes(contracttextbox.Text); ;
            var token = Encoding.UTF8.GetBytes(tokentextbox.Text); ;
            decimal amount = Decimal.Parse(AmountTextBox.Text);
            ushort noOfdecimal = Convert.ToUInt16(NoOfDecimalTextBox.Text);

            WalletContract mycontract = null;
            var api = new DigibyteAPI(new APIOptions { BaseURL = Program.InsightAPI });
            contractService = new ContractService(@"tsc-wallet.db", api);
            mycontract = await contractService.CreateContract(privateKey, contract, token, amount, noOfdecimal);
            SetControlPropertyThreadSafe(progressBar1, "Value", 2);
            contractID = mycontract.ID;
            MessageBox.Show("รหัสสัญญา :" + contractID);        //test Show Contract ID

            int CountConnect = 0;
            bool triggerContract = false;
            transactions = new TransBlockchain();
            while (CountConnect <= 15)        //find contractID in blockchain
            {
                InvokeHelp.SetControlPropertyThreadSafe(progressLabel, "Text", "กำลังตรวจสอบธุรกรรมการสร้างสัญญา");
                try
                {
                    var response = await client.GetAsync("/api/tx/" + contractID);
                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadAsStringAsync();
                    transactions = JsonConvert.DeserializeObject<TransBlockchain>(result);


                    if (transactions.time != 0 && transactions.blocktime != 0)
                    {
                        triggerContract = true;
                        break;
                    }

                    else
                    {
                        CountConnect++;
                        Thread.Sleep(5000);
                    }
                }
                catch
                {
                    CountConnect++;
                    Thread.Sleep(5000);
                }
            }
            SetControlPropertyThreadSafe(progressBar1, "Value", 3);
            if (!triggerContract)
            {
                InvokeHelp.SetControlPropertyThreadSafe(progressLabel, "Visible", false);
                SetControlPropertyThreadSafe(progressBar1, "Visible", false);
                MessageBox.Show("ไม่ค้นพบสัญญาเงินอิเล็กทรอนิกส์ในบล็อกเชน :" + contractID);
                return;
            }

            resultContract = await contractService.FindContract(contractID);
            SetControlPropertyThreadSafe(progressBar1, "Value", 4);

            if (resultContract == null)      //if dont seen this transaction. exite method
            {
                InvokeHelp.SetControlPropertyThreadSafe(progressLabel, "Visible", false);
                SetControlPropertyThreadSafe(progressBar1, "Visible", false);
                MessageBox.Show("การสร้างรหัสสัญญาไม่สำเร็จ");
                return;
            }


            //Check Block of Contract and LastBlock. because if block of contract is lastblock, calculating of cryptocurrency balance will be issue
            bool triggerAllow = false;

            CountConnect = 0;
            while (CountConnect <= 10)
            {
                try
                {
                    int ContractBlock = await api.GetBlockHeight(transactions.blockhash);
                    var LastSync = await api.GetSync();
                    int Lastblock = LastSync.blockChainHeight;
                    if (ContractBlock != Lastblock)
                    {
                        triggerAllow = true;
                        break;
                    }
                    else
                    {
                        CountConnect++;
                        Thread.Sleep(3000);
                    }
                }
                catch
                {
                    CountConnect++;
                    Thread.Sleep(3000);
                }
            }
            SetControlPropertyThreadSafe(progressBar1, "Value", 5);
            if (!triggerAllow)
            {
                InvokeHelp.SetControlPropertyThreadSafe(progressLabel, "Visible", false);
                InvokeHelp.SetControlPropertyThreadSafe(progressBar1, "Visible", false);
                MessageBox.Show("ขณะนี้ยังไม่สามารถออกเงินได้ ลองอีกครั้ง");
                return;
            }

            Thread.Sleep(5000);                     //wait mined
            int issuedRepeat = 2;
          start:
            InvokeHelp.SetControlPropertyThreadSafe(progressLabel, "Text", "กำลังออกเงิน");
            var walletService = contractService.CreateWalletService(resultContract, privateKey);    //Create issued Ledger
            var ledger1 = walletService.CreateLedger(OperationCode.Issue);      

            await walletService.BroadcastLedger(ledger1);
            SetControlPropertyThreadSafe(progressBar1, "Value", 6);
            MessageBox.Show("รหัสการออกเงิน :" + ledger1.TxId);        //test Show issued ID

            transactions = new TransBlockchain();
            int Count = 0;
            bool IssueTrigger = false;
            while (Count <= 15)
            {
                InvokeHelp.SetControlPropertyThreadSafe(progressLabel, "Text", "กำลังตรวจสอบธุรกรรมการออกเงิน");
                try
                {
                    var response2 = await client.GetAsync("/api/tx/" + ledger1.TxId);
                    response2.EnsureSuccessStatusCode();
                    var result = await response2.Content.ReadAsStringAsync();
                    transactions = JsonConvert.DeserializeObject<TransBlockchain>(result);


                    if (transactions.time != 0 && transactions.blocktime != 0)
                    {
                        IssueTrigger = true;
                        break;

                    }

                    else
                    {
                        Count++;
                        Thread.Sleep(5000);
                    }
                }
                catch
                {
                    Count++;
                    Thread.Sleep(5000);
                }
            }
            SetControlPropertyThreadSafe(progressBar1, "Value", 7);
            if (!IssueTrigger)     //if
            {
                InvokeHelp.SetControlPropertyThreadSafe(progressLabel, "Visible", false);
                SetControlPropertyThreadSafe(progressBar1, "Visible", false);
                MessageBox.Show("หมดเวลาการรอ กรุณาตรวจสอบรหัสธุรกรรมการออกเงินด้วยตัวเองที่ตัวสำรวจบล็อก :" + transactions.txid);
                return;         //exite ,method
            }

            var txContract = await api.GetTransactionInfo(contractID);
            var txIssued = await api.GetTransactionInfo(ledger1.TxId);
            if(txContract.blocktime > txIssued.blocktime)         //if Contract blocktime more than Issued blocktime. New Issued again.
            {
                if(issuedRepeat > 0)
                {
                    issuedRepeat--;
                    goto start;
                }
                else
                {
                    InvokeHelp.SetControlPropertyThreadSafe(progressLabel, "Visible", false);
                    SetControlPropertyThreadSafe(progressBar1, "Visible", false);
                    MessageBox.Show("หมดเวลาการรอ กรุณาตรวจสอบรหัสธุรกรรมการออกเงินด้วยตัวเองที่ตัวสำรวจบล็อก :" + txIssued.txid);
                    return;
                }
                
            }
            InvokeHelp.SetControlPropertyThreadSafe(progressLabel, "Text", "สร้างสัญญาเงินอิเล็กทรอนิกส์สำเร็จ");
            triggerFinish = true;
            Action action = new Action(FinishCreating);
            this.BeginInvoke(action);
        } 
        private void FinishCreating()
        {
            if (triggerFinish)
            {
                var formPopup = new CreateContractForm(resultContract.ID);
                formPopup.ShowDialog();
                this.Close();
            }
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            progressBar1.Visible = true;
            Thread t = new Thread(new ThreadStart(CreateContract));
            t.Start();

            //this.Hide();
            //formPopup.Closed += (s, args) => this.Close();
            //formPopup.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void tokentextbox_TextChanged(object sender, EventArgs e)
        {
            CheckForm();
        }

        private bool triggerAmountTextbox = true;
        private void AmountTextBox_TextChanged(object sender, EventArgs e)
        {

            StringBuilder sb = new StringBuilder();
            Regex regex = new Regex(@"[0-9\.]");
            regex.Matches(AmountTextBox.Text)
                 .OfType<Match>()
                 .Select(m => m.Value)
                 .ToList()
                 .ForEach(i => sb.Append(i));
            AmountTextBox.Text = sb.ToString();
            CheckForm();

        }
        private void AmountTextBox_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void NoOfDecimalTextBox_TextChanged(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            Regex regex = new Regex(@"[0-9\.]");
            regex.Matches(NoOfDecimalTextBox.Text)
                 .OfType<Match>()
                 .Select(m => m.Value)
                 .ToList()
                 .ForEach(i => sb.Append(i));
            NoOfDecimalTextBox.Text = sb.ToString();

            CheckForm();
        }

        private void AmountTextBox_Leave(object sender, EventArgs e)
        {
            triggerAmountTextbox = false;

            CheckForm();
            triggerAmountTextbox = true;
        }

        private delegate void SetControlPropertyThreadSafeDelegate(
    Control control,
    string propertyName,
    object propertyValue);

        public static void SetControlPropertyThreadSafe(
            Control control,
            string propertyName,
            object propertyValue)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new SetControlPropertyThreadSafeDelegate
                (SetControlPropertyThreadSafe),
                new object[] { control, propertyName, propertyValue });
            }
            else
            {
                control.GetType().InvokeMember(
                    propertyName,
                    BindingFlags.SetProperty,
                    null,
                    control,
                    new object[] { propertyValue });
            }
        }


    }
}
