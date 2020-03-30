using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractCreate.Models
{
    class MonitorContract       //Contract
    {
        [BsonId]
        public string contract_id { get; set; }
        public string owner_pubkey { get; set; }
        public int blockheight { get; set; }
        public List<WalletAccount> Accounts { get; set; }
    }
}
