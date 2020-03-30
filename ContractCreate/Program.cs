using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ContractCreate
{
    static class Program
    {
        //public static string InsightAPI = "https://explorer.thaismartcontract.com/";
        //public static string InsightAPI = "https://digiexplorer.info/";
        public static string InsightAPI = "https://insight.thaismartcontract.com/";
        public static decimal minBalance = 0.00040m;
        public static string TEst = Properties.Settings.Default.API1;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
