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
    public partial class ProgressbarForm : Form
    {
        private KeyService keyService;
        private CryptoKeyPair saveKey;
        
        private int MoneyToSend = 1000;
        private int Fee = 50000;
        private MainForm parentForm;
        HttpClient httpClient;
        private Transaction tx;
        private DigibyteAPI api;
        private bool triggerFinish = false;
        private List<WalletAccount> LowMoneyWallet;
        public ProgressbarForm(MainForm parentForm, List<WalletAccount> wallet)
        {
            InitializeComponent();
            this.parentForm = parentForm;
            LowMoneyWallet = wallet;
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(Program.InsightAPI);
            InitializeMoney();
        }

        private Thread t;
        private void InitializeMoney()
        {

            keyService = new KeyService("1234");
            saveKey = keyService.GetKey();      //Prepare KeyPair of Sender
            api = new DigibyteAPI(new APIOptions { BaseURL = Program.InsightAPI });
            
            t = new Thread(new ThreadStart(SendDGB));
            t.Start();
        }

        [Obsolete]
        private async void SendDGB()
        {
            InvokeHelp.SetControlPropertyThreadSafe(progressBar1, "Value", 1);
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
                InvokeHelp.SetControlPropertyThreadSafe(progressBar1, "Value", 2);

                var txid = await BroadcastTransaction(tx.ToHex());

                int issuedRepeat = 20;
            
            InvokeHelp.SetControlPropertyThreadSafe(progressBar1, "Value", 3);
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
                    InvokeHelp.SetControlPropertyThreadSafe(progressBar1, "Visible", false);
                    MessageBox.Show("การออกเงินให้เจ้าของเงินไม่สมบูรณ์ กรุณาตรวจสอบรหัสธุรกรรมการออกเงิน :" + txid.txid);
                    triggerFinish = true;
                    Action action1 = new Action(FinishCreating);
                    this.BeginInvoke(action1);
                    return;
                }
            }
            InvokeHelp.SetControlPropertyThreadSafe(progressBar1, "Value", 4);
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

        private void ProgressbarForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //this.parentForm.LoadData();
        }
    }
}
