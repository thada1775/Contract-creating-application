using Thaismartcontract.WalletService.Model;

namespace Thaismartcontract.WalletService.Extension
{
    public static class LedgerExtension
    {
        public static ViewableLedger AsVieweable(this Ledger ledger, ContactService service)
        {
            var viewable = new ViewableLedger()
            {
                TxId = ledger.TxId,
                Blockheight = ledger.Blockheight,
                Operation = ledger.Operation.ToString(),
                Sender = service.GetContact(ledger.TokenSenderHashHex),
                Receiver = service.GetContact(ledger.TokenReceiverHashHex),
                Amount = ledger.Amount,
                ReferenceCode = ledger.ReferenceCode,
                Status = ledger.Status.ToString(),
                Time = ledger.Time.AddHours(7)
            };
            return viewable;
        }
    }
}
