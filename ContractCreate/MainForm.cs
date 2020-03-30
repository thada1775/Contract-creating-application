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
using System.Timers;
using System.Windows.Forms;
using Thaismartcontract.API;
using Thaismartcontract.API.Model;
using Thaismartcontract.WalletService;

namespace ContractCreate
{
    public partial class MainForm : Form
    {
        KeyService keyService;
        private CryptoKeyPair currentKeyPair;
        private CryptoKeyPair saveKey;
        private BitcoinSecret privateKey;
        public string publicKey;
        private HttpClient client;
        public AddressBlockchain DGBbalance;
        public APITransByAddr transactions;
        public decimal DgbBalance;
        private DigibyteAPI api;
        private List<WalletAccount> WalletInDb;
        private string connectionString;
        private MonitorContract CurrentContract;
        private int LastBlockHeight;
        private bool IsDone = false;
        private bool triggerAutoSend = false;
        private List<WalletAccount> LowMoneyWallet = new List<WalletAccount>();
        private int Fee = 50000;
        private int MoneyToSend = 1000;
        private Transaction tx;
        System.Timers.Timer timer;
        public MainForm()
        {
            InitializeComponent();
            progressBar1.Maximum = 7;
            progressBar1.Visible = false;
            this.refreshbutton.Image = (Image)(new Bitmap(ContractCreate.Properties.Resources.reload1, new Size(32, 32)));
            client = new HttpClient();
            client.BaseAddress = new System.Uri(Program.InsightAPI);

            api = new DigibyteAPI(new APIOptions { BaseURL = Program.InsightAPI });
            string path = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}CurrentContract.txt";
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
             
                }
            }
            connectionString = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}tsc-wallet.db";
            InitializeEMoney();
        }
        public async void InitializeEMoney()
        {
            keyService = new KeyService("1234");
            try
            {
                if (!keyService.HasSetupKey())
                {
                    var savekeyForm = new SaveKeyForm();
                    savekeyForm.ShowDialog();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("ไม่ค้นพบกุญแจ");
            }

            saveKey = keyService.GetKey();
            if(saveKey == null)
            {
                Environment.Exit(1);
            }
            privateKey = saveKey.SecretKey;
            publicKey = saveKey.PublicKeyWif;

            publickeylabel.Text = this.publicKey;

            timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Interval = 60000;
            timer.Enabled = true;

            LoadData();
        }
        
        Thread m;
        public void LoadData()
        {
            m = new Thread(new ThreadStart(ScanWallet));
            m.Start();
        }
        private async void ScanWallet()
        {

            timer.Stop();

            InvokeHelp.SetControlPropertyThreadSafe(refreshbutton, "Enabled", false);
            InvokeHelp.SetControlPropertyThreadSafe(rescanbutton, "Enabled", false);
            InvokeHelp.SetControlPropertyThreadSafe(sendDgbButton, "Enabled", false);
            InvokeHelp.SetControlPropertyThreadSafe(addcontractbutton, "Enabled", false);


            InvokeHelp.SetControlPropertyThreadSafe(progressBar1, "Visible", true);
            InvokeHelp.SetControlPropertyThreadSafe(progressBar1, "Value", 1);
            DgbBalance = await api.GetBalance(publicKey);
            ShowDataToGrid(new List<WalletAccount>());
            if (InvokeRequired) //Invoke DGBBalancelabel
            {
                this.Invoke(new MethodInvoker(delegate { DGBBalancelabel.Text = DgbBalance.ToString(); }));

            }
            else
            {
                DGBBalancelabel.Text = DgbBalance.ToString();
            }
            string path = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}CurrentContract.txt";
            using (StreamReader sr = File.OpenText(path))
            {
                string s;
                if ((s = sr.ReadLine()) != null)
                {
                    using (var db = new LiteDatabase(connectionString))
                    {
                        var collection = db.GetCollection<MonitorContract>("Contracts");
                        CurrentContract = collection.FindOne(x => x.contract_id == s);
                    }
                    InvokeHelp.SetControlPropertyThreadSafe(addcontractbutton, "Text", "แก้ไข"); 
                }
                else
                {
                    InvokeHelp.SetControlPropertyThreadSafe(addcontractbutton, "Text", "เพิ่ม");

                    ShowDataToGrid(new List<WalletAccount>());
                    InvokeHelp.SetControlPropertyThreadSafe(progressBar1, "Visible", false);
                    InvokeHelp.SetControlPropertyThreadSafe(addcontractbutton, "Enabled", true);    //try
                    return;

                }
            }
            
            if (CurrentContract == null)
            {
                InvokeHelp.SetControlPropertyThreadSafe(addcontractbutton, "Enabled", true);        //try
                return;
            }
            
            List<String> pbkey = new List<string>();
            try
            {
                int i = 0;
                int totalPage = 1;

                bool stopLimit = false;
                LastBlockHeight = 0;
                string ownerpubkey = CurrentContract.owner_pubkey;
                while (i < totalPage && !stopLimit)
                {
                    var response = await client.GetAsync($"/api/txs/?address={ownerpubkey}&pageNum={i}");
                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadAsStringAsync();
                    transactions = JsonConvert.DeserializeObject<APITransByAddr>(result);
                    totalPage = transactions.pagesTotal;
                    List<Tx> Address = transactions.txs;

                    foreach (var datatx in Address)
                    {
                        var height = await api.GetBlockHeight(datatx.blockhash);
                        if (height > CurrentContract.blockheight)       //only BlockHeight more than BlockHeight in DB Contract
                        {
                            if (LastBlockHeight == 0)
                            {
                                LastBlockHeight = height;
                            }

                            foreach (var vins in datatx.vin)
                            {
                                pbkey.Add(vins.addr);
                            }
                            foreach (var vout in datatx.vout)
                            {
                                if (vout.scriptPubKey.addresses != null)
                                {

                                    foreach (var addr in vout.scriptPubKey.addresses)
                                    {
                                        pbkey.Add(addr.ToString());

                                    }
                                }
                            }
                        }
                        else
                        {
                            stopLimit = true;
                            break;
                        }
                    }
                    i++;
                }
                pbkey = pbkey.Distinct().ToList();              //Distince wallet
                InvokeHelp.SetControlPropertyThreadSafe(progressBar1, "Value", 2);

                List<string> walletInDb = new List<string>();
                List<string> contractInDb = new List<string>();
                using (var db = new LiteDatabase(connectionString))                     //Compare present wallet in api  with wallet in db. Insert difference wallet(new wallet) to db
                {
                    var collection = db.GetCollection<WalletAccount>("Accounts");
                    var walletInDb1 = collection.Find(x => x.account_id.contract_id == CurrentContract.contract_id).ToList();

                    if (walletInDb1.Count() != 0)
                    {
                        walletInDb = walletInDb1.Select(x => x.account_id.publickey).ToList();
                        pbkey = pbkey.Except(walletInDb).ToList();
                    }
                }
                InvokeHelp.SetControlPropertyThreadSafe(progressBar1, "Value", 3);
                var a1 = pbkey.IndexOf(CurrentContract.owner_pubkey);          //remove owner public key
                if (a1 >= 0)
                {
                    pbkey.RemoveAt(a1);
                }
                InvokeHelp.SetControlPropertyThreadSafe(progressBar1, "Value", 4);

                if (pbkey.Count() != 0)                             //if it have got new wallet. Insert it.
                {
                    using (var db = new LiteDatabase(connectionString))             //Insert Wallet
                    {
                        var collection = db.GetCollection<WalletAccount>("Accounts");
                        foreach (var tx in pbkey)
                        {
                            var account = new WalletAccount
                            {

                                account_id = new KeyAccount
                                {
                                    contract_id = CurrentContract.contract_id,
                                    publickey = tx.ToString()
                                }
                            };
                            collection.Insert(account);
                        }
                    }
                    using (var db = new LiteDatabase(connectionString))                     //Compare present wallet in api  with wallet in db. Insert difference wallet(new wallet) to db
                    {
                        var collection = db.GetCollection<MonitorContract>("Contracts");
                        CurrentContract.blockheight = LastBlockHeight;
                        collection.Update(CurrentContract);
                    }

                }
                InvokeHelp.SetControlPropertyThreadSafe(progressBar1, "Value", 5);
                using (var db = new LiteDatabase(connectionString))
                {
                    var collection = db.GetCollection<WalletAccount>("Accounts");
                    var walletInDb1 = collection.Find(x => x.account_id.contract_id == CurrentContract.contract_id);
                    if (walletInDb1.Count() != 0)
                    {
                        walletInDb = walletInDb1.Select(x => x.account_id.publickey).ToList();

                        foreach (var wallet in walletInDb)
                        {
                            var balance = await api.GetBalance(wallet);
                            var lessMoney = collection.FindOne(x => x.account_id.contract_id == CurrentContract.contract_id && x.account_id.publickey == wallet);
                            lessMoney.balance = balance;
                            collection.Update(lessMoney);
                        }
                    }
                    WalletInDb = collection.Find(x => x.account_id.contract_id == CurrentContract.contract_id).ToList();
                }
                InvokeHelp.SetControlPropertyThreadSafe(progressBar1, "Value", 6);
            }
            
            catch (Exception e)
            {
                MessageBox.Show("การเชื่อมต่อตัวสำรวจบล็อกมีปัญหา โปรดลองใหม่ภายหลัง");
                //MessageBox.Show(e.Message);
                using (var db = new LiteDatabase(connectionString))     //if cannot use transaction in blockchian. Use last data in databasse
                {
                    var collection = db.GetCollection<WalletAccount>("Accounts");
                    WalletInDb = collection.Find(x => x.account_id.contract_id == CurrentContract.contract_id).ToList();
                }
            }
            if (WalletInDb != null)
            {
                ShowDataToGrid(WalletInDb);
                InvokeHelp.SetControlPropertyThreadSafe(progressBar1, "Value", 7);
                InvokeHelp.SetControlPropertyThreadSafe(progressBar1, "Visible", false);
            }
            InvokeHelp.SetControlPropertyThreadSafe(progressBar1, "Visible", false);

            InvokeHelp.SetControlPropertyThreadSafe(refreshbutton, "Enabled", true);
            InvokeHelp.SetControlPropertyThreadSafe(rescanbutton, "Enabled", true);
            InvokeHelp.SetControlPropertyThreadSafe(sendDgbButton, "Enabled", true);
            InvokeHelp.SetControlPropertyThreadSafe(addcontractbutton, "Enabled", true);

            LowMoneyWallet = WalletInDb.FindAll(x => x.balance < Properties.Settings.Default.minBalance);  //Find Wallet is low money
            IsDone = true;
            timer.Start();
            AutoSendDGB();
        }

        Thread t;
        private void AutoSendDGB()
        {
            if(triggerAutoSend)
            {
                timer.Stop();
                
                
                if (LowMoneyWallet.Count() != 0)
                {
                    //ProgressbarForm Autosent = new ProgressbarForm(this, LowMoneyWallet);
                    ////Autosent.Invoke(new Action(() => new ProgressbarForm(this, LowMoneyWallet)));
                    //this.Invoke(new MethodInvoker(delegate { Autosent.ShowDialog(); }));
                    t = new Thread(new ThreadStart(SendDigibyte));
                    t.Start();
                    
                }
                else
                {
                    m.Abort();
                    timer.Start();
                }
                triggerAutoSend = false;
                
            }
            //InvokeHelp.SetControlPropertyThreadSafe(progressBar1, "Visible", false);
            
        }

        private async void SendDigibyte()
        {
            InvokeHelp.SetControlPropertyThreadSafe(Sendinglabel, "Text", "กำลังส่งเงินอัตโนมัติ");
            InvokeHelp.SetControlPropertyThreadSafe(progressBar2, "Visible", true);
            InvokeHelp.SetControlPropertyThreadSafe(refreshbutton, "Enabled", false);
            InvokeHelp.SetControlPropertyThreadSafe(progressBar2, "Value", 1);
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

            foreach (var a in LowMoneyWallet)
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
            InvokeHelp.SetControlPropertyThreadSafe(progressBar2, "Value", 2);

            var txid = await BroadcastTransaction(tx.ToHex());

            int issuedRepeat = 30;

            InvokeHelp.SetControlPropertyThreadSafe(progressBar2, "Value", 3);
        start:
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
                    InvokeHelp.SetControlPropertyThreadSafe(progressBar2, "Visible", false);
                    InvokeHelp.SetControlPropertyThreadSafe(Sendinglabel, "Text", "ส่งเงินไม่สำเร็จ");
                    timer.Start();
                    return;
                }
            }


            InvokeHelp.SetControlPropertyThreadSafe(progressBar2, "Value", 4);
            InvokeHelp.SetControlPropertyThreadSafe(progressBar2, "Visible", false);

            InvokeHelp.SetControlPropertyThreadSafe(Sendinglabel, "ForeColor", Color.Green);
            InvokeHelp.SetControlPropertyThreadSafe(Sendinglabel, "Text", "ส่งเงินอัตโนมัติสำเร็จ");
            
            InvokeHelp.SetControlPropertyThreadSafe(timeSendinglabel, "Text", DateTime.Now.ToString("HH:mm dd/MM/yyyy"));
            InvokeHelp.SetControlPropertyThreadSafe(NumOfWalletSendlabel, "Text", LowMoneyWallet.Count().ToString());

            InvokeHelp.SetControlPropertyThreadSafe(totalSendlabel, "ForeColor", Color.BlueViolet);

            decimal totaltSent = LowMoneyWallet.Count() * (MoneyToSend / 100000000m);
            totaltSent = totaltSent + (Fee / 100000000m);
            InvokeHelp.SetControlPropertyThreadSafe(totalSendlabel, "Text", totaltSent.ToString());

            InvokeHelp.SetControlPropertyThreadSafe(refreshbutton, "Enabled", true);

            LoadData();
            t.Abort();

            
        }
        private void ShowDataToGrid(List<WalletAccount> wallet)
        {
            InvokeHelp.SetControlPropertyThreadSafe(totalwalletlabel, "Text", wallet.Count().ToString());
            InvokeHelp.SetControlPropertyThreadSafe(dataGridView1, "DataSource", wallet.Select(x => new { x.account_id.publickey, x.balance }).ToList());
            if (InvokeRequired) //Invoke DGBBalancelabel
            {
                foreach (DataGridViewColumn col in dataGridView1.Columns)
                {
                    this.Invoke(new MethodInvoker(delegate { col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter; }));
                    this.Invoke(new MethodInvoker(delegate { col.HeaderCell.Style.Font = new Font("TH SarabunPSK", 20F, FontStyle.Bold, GraphicsUnit.Pixel); }));
                      
                }

                //this.Invoke(new MethodInvoker(delegate { dataGridView1.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter; }));
                this.Invoke(new MethodInvoker(delegate { dataGridView1.Columns[0].HeaderText = "กุญแจสาธารณะ"; }));
                this.Invoke(new MethodInvoker(delegate { dataGridView1.Columns[1].HeaderText = "Digibyte คงเหลือ"; }));
                this.Invoke(new MethodInvoker(delegate { dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; }));
                this.Invoke(new MethodInvoker(delegate { dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; }));

            }
            else
            {
                dataGridView1.Columns[0].HeaderText = "กุญแจสาธารณะ";
                dataGridView1.Columns[1].HeaderText = "Digibyte คงเหลือ";
                dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }
        private  void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (IsDone)
            {
                triggerAutoSend = true;
                LoadData(); 
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        Form CreateContractForm;
        private void button3_Click(object sender, EventArgs e)
        {
            timer.Stop();
            CreateContractForm = new Form1(this);
            CreateContractForm.ShowDialog();
            timer.Start();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

       

        private void button4_Click(object sender, EventArgs e)
        {
            timer.Stop();
            var AddContractForm = new AddContractForm(this);
            AddContractForm.ShowDialog();
            //timer.Start();
            //LoadData();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void publickeylabel_Click(object sender, EventArgs e)
        {
            
        }

        private void refreshbutton_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void rescanbutton_Click(object sender, EventArgs e)
        {
            timer.Stop();
            var result = MessageBox.Show("ข้อมูลต่าง ๆในฐานข้อมูลจะถูกลบ จากนั้นโปรแกรมจะทำการค้นหาและเพิ่มเข้าไปใหม่", "ยืนยัน", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                using (var db = new LiteDatabase(connectionString))
                {
                    var collection = db.GetCollection<MonitorContract>("Contracts");

                    var allContract = collection.FindAll();
                    foreach(var wallet in allContract)
                    {
                        wallet.blockheight = 0;
                        collection.Update(wallet);
                    }
                    db.DropCollection("Accounts");
                    LoadData();
  
                }
                
            }
            timer.Start();
        }

        private void sendDgbButton_Click(object sender, EventArgs e)
        {
            timer.Stop();
            if (LowMoneyWallet.Count() != 0)
            {
                var sendDGBForm = new SendDGBForm(this, WalletInDb);
                sendDGBForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("ไม่มีกระเป๋าเงินที่ยอดเงินหมด", "การส่งเงิน");
                timer.Start();
            }
            
            
        }

        
        

        private void MainForm_Load(object sender, EventArgs e)
        {
            
  
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }
        public async Task<List<UTXO>> GetReceivedCoinFromDigibyte(string address)
        {
            var result = await client.GetStringAsync($"/api/addr/{address}/utxo");
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
            var result = await client.PostAsync($"/api/tx/send", content);
            result.EnsureSuccessStatusCode();
            var output = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TxID>(output);
        }
    }
}
