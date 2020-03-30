using System;
using System.Collections.Generic;
using System.Text;

namespace ContractCreate.Models
{
    public class ScriptSig
    {
        public string asm { get; set; }
        public string hex { get; set; }
    }


    public class Tx
    {
        public string txid { get; set; }
        public string hash { get; set; }
        public int version { get; set; }
        public int size { get; set; }
        public int vsize { get; set; }
        public int weight { get; set; }
        public int locktime { get; set; }
        public List<Vin> vin { get; set; }
        public List<Vout> vout { get; set; }
        public string blockhash { get; set; }
        public int confirmations { get; set; }
        public int time { get; set; }
        public int blocktime { get; set; }
        public double valueOut { get; set; }
        public double valueIn { get; set; }
        public double fees { get; set; }
    }

    public class APITransByAddr
    {
        public int pagesTotal { get; set; }
        public List<Tx> txs { get; set; }
    }
    
}
