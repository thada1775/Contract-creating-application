using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractCreate.Models
{
    public class AddressBlockchain
    {
        public string addrStr { get; set; }
        public double balance { get; set; }
        public int balanceSat { get; set; }
        public double totalReceived { get; set; }
        public long totalReceivedSat { get; set; }
        public double totalSent { get; set; }
        public long totalSentSat { get; set; }
        public int unconfirmedBalance { get; set; }
        public int unconfirmedBalanceSat { get; set; }
        public int unconfirmedTxApperances { get; set; }
        public int txApperances { get; set; }
        public List<string> transactions { get; set; }
    }
    
}
