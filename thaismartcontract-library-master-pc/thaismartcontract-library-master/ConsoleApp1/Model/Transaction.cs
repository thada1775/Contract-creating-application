using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.Model
{
    public class ScriptSig
    {
        public string hex { get; set; }
        public string asm { get; set; }
    }

    public class Vin
    {
        public string txid { get; set; }
        public int vout { get; set; }
        public long sequence { get; set; }
        public int n { get; set; }
        public ScriptSig scriptSig { get; set; }
        public string addr { get; set; }
        public int valueSat { get; set; }
        public double value { get; set; }
        public object doubleSpentTxID { get; set; }
    }

    public class ScriptPubKey
    {
        public string hex { get; set; }
        public string asm { get; set; }
        public List<string> addresses { get; set; }
        public string type { get; set; }
    }

    public class Vout
    {
        public string value { get; set; }
        public int n { get; set; }
        public ScriptPubKey scriptPubKey { get; set; }
        public string spentTxId { get; set; }
        public int? spentIndex { get; set; }
        public int? spentHeight { get; set; }
    }

    public class Transaction
    {
        public string txid { get; set; }
        public int version { get; set; }
        public int locktime { get; set; }
        public List<Vin> vin { get; set; }
        public List<Vout> vout { get; set; }
        public string blockhash { get; set; }
        public int blockheight { get; set; }
        public int confirmations { get; set; }
        public int time { get; set; }
        public int blocktime { get; set; }
        public double valueOut { get; set; }
        public int size { get; set; }
        public double valueIn { get; set; }
        public double fees { get; set; }
    }

    public class TxID
    {
        public string txid { get; set; }
    }

    public class RawTX
    {
        public string rawtx { get; set; }
    }
}
