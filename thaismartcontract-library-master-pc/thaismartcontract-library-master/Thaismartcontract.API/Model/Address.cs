using System.Collections.Generic;

namespace Thaismartcontract.API.Model
{
    public class Address
    {
        public string addrStr { get; set; }
        public decimal balance { get; set; }
        public long balanceSat { get; set; }
        public decimal totalReceived { get; set; }
        public long totalReceivedSat { get; set; }
        public decimal totalSent { get; set; }
        public long totalSentSat { get; set; }
        public long unconfirmedBalance { get; set; }
        public long unconfirmedBalanceSat { get; set; }
        public long unconfirmedTxApperances { get; set; }
        public long txApperances { get; set; }
        public List<string> transactions { get; set; }
    }
}
