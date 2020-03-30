using System;

namespace Thaismartcontract.CPMO.Model
{
    public class ConsentAction
    {
        public string Domain { get; set; }
        public string DocumentHash { get; set; }

        public string AskFrom { get; set; }
        public string AskTxId { get; set; }
        public long AskTick { get; set; }

        public string ApproveFrom { get; set; }
        public string ApproveTxId { get; set; }
        public long ApprovedTick { get; set; }

        public DateTime AskDate => new DateTime(1970, 1, 1).AddHours(7).AddSeconds(AskTick);
        public DateTime ApproveDate => new DateTime(1970, 1, 1).AddHours(7).AddSeconds(ApprovedTick);
        public bool IsApproved => !string.IsNullOrEmpty(ApproveTxId);
        public bool IsAsked => !string.IsNullOrEmpty(AskTxId);

    }
}
