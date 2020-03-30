using ContractCreate.Models;
using LiteDB;
using NBitcoin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Thaismartcontract.API;
using Thaismartcontract.API.Model;
using Thaismartcontract.WalletService;

namespace ContractCreate
{
    public partial class SendDGBForm : Form
    {
        private KeyService keyService;
        private CryptoKeyPair saveKey;
        private List<WalletAccount> lowBalanceWallet;
        private int MoneyToSend = 1000;
        private int Fee = 50000;
        private MainForm parentForm;
        HttpClient httpClient;     
        private Transaction tx;
        private DigibyteAPI api;
        private bool triggerFinish = false;

        public SendDGBForm(MainForm parentForm,List<WalletAccount> wallet)
        {          
            InitializeComponent();
            this.parentForm = parentForm;
            lowBalanceWallet = wallet.Where(x => x.balance < Properties.Settings.Default.minBalance).ToList();  //Find Wallet is low money
            progressBar1.Visible = false;
            progressBar1.Maximum = 3;
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(Program.InsightAPI);
            InitializeMoney();
        }
        private void InitializeMoney()
        {
            keyService = new KeyService("1234");
            saveKey = keyService.GetKey();      //Prepare KeyPair of Sender
            api = new DigibyteAPI(new APIOptions { BaseURL = Program.InsightAPI });
            LoadData();  
        }

        decimal totalSend;
        public void LoadData()
        {
            

            var amountToSend = (lowBalanceWallet.Count * (MoneyToSend / 100000000m));   //100000000 is Satoshi
            totalSend = amountToSend + (Fee / 100000000m);
            decimal leftBanlance = parentForm.DgbBalance - totalSend;
            totalwalletlabel.Text = lowBalanceWallet.Count().ToString();
            totalDigibyteLabel.Text = totalSend.ToString();
            
            leftBalanceLabel.Text = leftBanlance.ToString();
            
            if(leftBanlance <= 0.0m)
            {
                MessageBox.Show("ยอดเงินสำหรับการส่งไม่เพียงพอ");
                sendButton.Enabled = false;
            }
            dataGridView1.DataSource = lowBalanceWallet.Select(x => new { x.account_id.publickey, x.balance }).ToList();

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.HeaderCell.Style.Font = new Font("TH SarabunPSK", 20F, FontStyle.Bold, GraphicsUnit.Pixel);

            }
            dataGridView1.Columns[0].HeaderText = "กุญแจสาธารณะ";
            dataGridView1.Columns[1].HeaderText = "Digibyte คงเหลือ";
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            PrepareWallet();
        }

        [Obsolete]
        public async void PrepareWallet()
        {
            Network network = NBitcoin.Altcoins.Digibyte.Instance.Mainnet;
            BitcoinSecret sender = saveKey.SecretKey;
            
            var myTransaction = Transaction.Create(network);

            var fee = new Money(Fee);
            var allUtxo = await GetReceivedCoinFromDigibyte(sender.GetAddress().ToString());
            var utxo = allUtxo.OrderByDescending(u => u.amount).First().AsCoin();
            //var utxo = allUtxo[0].ToCoin();

            var txBuilder = network.CreateTransactionBuilder();
            for (int i = 0; i < allUtxo.Count(); i++)
            {
                txBuilder = txBuilder.AddCoins(allUtxo[i].AsCoin());
            }

            
            txBuilder.AddKeys(sender);

            foreach (var a in lowBalanceWallet)
            {  
                try
                {
                    BitcoinAddress receiver;
                    if (a.account_id.publickey.StartsWith("dgb1"))
                    {
                        receiver = new BitcoinWitPubKeyAddress(a.account_id.publickey, network);
                    }
                    else
                    {
                        receiver = new BitcoinPubKeyAddress(a.account_id.publickey, network);
                    }

                    txBuilder
                     .Send(receiver, new Money(MoneyToSend));
                    
                }
                catch
                {
                    continue;
                }

            }
            tx = txBuilder
                .SetChange(sender.GetAddress())
                .SendFees(fee)
                .BuildTransaction(true);

            var verified = txBuilder.Verify(tx);
        }

        public async Task<List<UTXO>> GetReceivedCoinFromDigibyte(string address)
        {
            var result = await httpClient.GetStringAsync($"/api/addr/{address}/utxo");
            return JsonConvert.DeserializeObject<List<UTXO>>(result);
        }

        public async Task<TxID> BroadcastTransaction(string hex)
        {
            var data = new RawTX()
            {
                rawtx = hex
            };
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = await httpClient.PostAsync($"/api/tx/send", content);
            result.EnsureSuccessStatusCode();
            var output = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TxID>(output);
        }

        private void cancleButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private Thread t;
        private void sendButton_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("จำนวนเงินที่ส่ง "+ totalSend + " DGB", "คำขอยืนยัน", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                t = new Thread(new ThreadStart(SendDGB));
                t.Start();
            }
        }
        private async void SendDGB()
        {
            InvokeHelp.SetControlPropertyThreadSafe(progressBar1, "Visible", true);
            InvokeHelp.SetControlPropertyThreadSafe(progressLabel, "Visible", true);
            InvokeHelp.SetControlPropertyThreadSafe(progressBar1, "Value", 1);
            var txid = await BroadcastTransaction(tx.ToHex());
            int issuedRepeat = 30;
            InvokeHelp.SetControlPropertyThreadSafe(progressBar1, "Value", 2);
        start:
            string verifyTransaction = "ตรวจสอบธุรกรรมการส่งเงินครั้งที่ " + (31-issuedRepeat);
            InvokeHelp.SetControlPropertyThreadSafe(progressLabel, "Text", verifyTransaction);
            var txContract = await api.GetTransactionInfo(txid.txid);
            if (txContract.time == 0 && txContract.blocktime == 0)         
            {
                if (issuedRepeat > 0)
                {
                    issuedRepeat--;
                    Thread.Sleep(5000);
                    goto start;
                }
                else
                {
                    InvokeHelp.SetControlPropertyThreadSafe(progressBar1, "Visible", false);
                    MessageBox.Show("การออกเงินให้เจ้าของเงินไม่สมบูรณ์ กรุณาตรวจสอบรหัสธุรกรรมการออกเงิน :" + txid.txid);
                    triggerFinish = true;
                    Action action1 = new Action(FinishCreating);
                    this.BeginInvoke(action1);
                    return;
                }
            }
            InvokeHelp.SetControlPropertyThreadSafe(progressBar1, "Value", 3);
            MessageBox.Show("ส่งเงินสำเร็จ");

            triggerFinish = true;

            Action action = new Action(FinishCreating);
            this.BeginInvoke(action);
    }

    private void FinishCreating()
    {
        if (triggerFinish)
        {
             triggerFinish = false;
             this.Close();
        }
    }

        private void SendDGBForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.parentForm.LoadData();
        }

        private void SendDGBForm_Load(object sender, EventArgs e)
        {

        }
    }
}
