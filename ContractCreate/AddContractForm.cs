using ContractCreate.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Thaismartcontract.API;
using Thaismartcontract.WalletService;
using Thaismartcontract.WalletService.Model;

namespace ContractCreate
{
    public partial class AddContractForm : Form
    {
        private string Contract_id { get; set; }
        private WalletContract MyContract;
        private ContractService contractService { get; set; }
        private List<WalletContract> AllMyContract { get; set; }
        private MainForm parentForm;
        public string InsightAPI = "https://explorer.thaismartcontract.com/";
        public AddContractForm(MainForm parentForm)
        {
            contractService = new ContractService(@"tsc-wallet.db", new DigibyteAPI(new APIOptions() { BaseURL = Program.InsightAPI }));
            this.parentForm = parentForm;
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            string path = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}CurrentContract.txt";
            using (StreamReader sr = File.OpenText(path))
            {
                string s;
                if ((s = sr.ReadLine()) != null)
                {
                    contracttextbox.Text = s;
                    MyContract = contractService.LoadContract(s);
                    contractname.Text = MyContract.NameString;
                    tokenname.Text = MyContract.TokenString;
                }
            }

            AllMyContract = contractService.FindLocalContract();            //view transaction in gridview
            var AllMyContract2 = AllMyContract.Select(x => new { x.ID, x.NameString, x.TokenString }).ToList();
            dataGridView1.DataSource = AllMyContract2;
            dataGridView1.Columns[0].HeaderText = "รหัสสัญญา";
            dataGridView1.Columns[1].HeaderText = "ชื่อสัญญา";
            dataGridView1.Columns[2].HeaderText = "หน่วยนับ";

            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

        }
        private void Edit(object sender, DataGridViewCellEventArgs e)
        {
            
            if (dataGridView1.SelectedRows.Count != 0)
            {
                DataGridViewRow row = this.dataGridView1.SelectedRows[0];
                contracttextbox.Text=  row.Cells["ID"].Value.ToString();
            }
        }

        private void AddContractForm_Load(object sender, EventArgs e)
        {

        }

        

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private async void addbutton_Click(object sender, EventArgs e)
        {
            Contract_id = contracttextbox.Text;

            var result = await contractService.FindContract(contracttextbox.Text);
            if (result == null)
            {
                MessageBox.Show("ไม่ค้นพบรหัสสัญญาใน Block explorer");
            }
            else
            {
                MyContract = contractService.LoadContract(Contract_id);

                

                string connectionString = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}tsc-wallet.db";
                using (var db = new LiteDatabase(connectionString))             //Insert Wallet
                {
                    var collection = db.GetCollection<MonitorContract>("Contracts");

                    MonitorContract existing = null;
                    existing = collection.Find(c => c.contract_id == Contract_id).FirstOrDefault();

                    if (existing == null)
                    {
                        var contract = new MonitorContract
                        {
                            contract_id = Contract_id,
                            owner_pubkey = MyContract.OwnerPublicAddress,
                            blockheight = 0
                        };
                        collection.Insert(contract);
                    }                     
                }
                string path = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}CurrentContract.txt";
                if (File.Exists(path))
                {
                    File.WriteAllText(path, string.Empty);
                    
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        
                        sw.WriteLine(MyContract.ID);
                    }
                    
                }
                MessageBox.Show("เลือกรหัสสัญญาสำเร็จ");
                LoadData();
            }
        }

        private void deletebutton_Click(object sender, EventArgs e)
        {
            string path = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}CurrentContract.txt";
            if (File.Exists(path))
            {
                File.WriteAllText(path, string.Empty);
            }
            MessageBox.Show("ลบรหัสสัญญาที่เลือกไว้สำเร็จ");
            contracttextbox.Text = "";
            contractname.Text = "";
            tokenname.Text = "";
            MyContract = null;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddContractForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.parentForm.LoadData();
        }
    }
}
