using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractCreate.Models
{
    

    public class Vin
    {
        public string txid { get; set; }
        public int vout { get; set; }
        public ScriptSig scriptSig { get; set; }
        public long sequence { get; set; }
        public int n { get; set; }
        public string addr { get; set; }
        public int valueSat { get; set; }
        public double value { get; set; }
        public object doubleSpentTxID { get; set; }
    }

    public class ScriptPubKey
    {
        public string asm { get; set; }
        public string hex { get; set; }
        public int reqSigs { get; set; }
        public string type { get; set; }
        public List<string> addresses { get; set; }
    }

    public class Vout
    {
        public string value { get; set; }
        public int n { get; set; }
        public ScriptPubKey scriptPubKey { get; set; }
        public string spentTxId { get; set; }
        public int spentIndex { get; set; }
        public int spentTs { get; set; }
    }

    public class TransBlockchain
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
    
}
