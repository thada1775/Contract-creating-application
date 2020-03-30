using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractCreate.Models
{
    public class KeyAccount
    {
        public string contract_id { get; set; }
        public string publickey { get; set; }
        
    }
    public class WalletAccount
    {
        [BsonId]
        public KeyAccount account_id { get; set; }
        public decimal balance { get; set; }
    }
}
