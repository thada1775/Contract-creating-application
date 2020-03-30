using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thaismartcontract.WalletService.Model
{
    public class Contact
    {       
        public string Name { get; set; }
        [BsonId]
        public string Address { get; set; }
        public byte[] AddressHash { get; set; }
        public string AddressHashString
        {
            get
            {
                return BitConverter.ToString(AddressHash).Replace("-", "").ToLower();
            }
        }

        [BsonIgnore]
        public string DisplayName => $"{Name} ({Address})";
    }
}
