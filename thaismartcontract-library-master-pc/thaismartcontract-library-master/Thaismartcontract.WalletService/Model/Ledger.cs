using LiteDB;
using NBitcoin;
using System;
using System.Linq;
using System.Text;
using Thaismartcontract.API.Extension;
using Thaismartcontract.WalletService.Extension;

namespace Thaismartcontract.WalletService.Model
{
    public class Ledger
    {
        [BsonId]
        public string TxId { get; set; }
        public int Blockheight { get; set; }
        public OperationCode Operation { get; set; }
        public byte[] TokenSenderHash { get; set; }
        [BsonIgnore]
        public string TokenSenderHashHex
        {
            get
            {
                return TokenSenderHash.ByteArrayToString();
            }
            set
            {
                TokenSenderHash = value.StringToByteArray();
            }
        }

        public byte[] TokenReceiverHash { get; set; }
        [BsonIgnore]
        public string TokenReceiverHashHex
        {
            get
            {
                return TokenReceiverHash.ByteArrayToString();
            }
            set
            {
                TokenReceiverHash = value.StringToByteArray();
            }
        }
        [BsonIgnore]
        public BitcoinPubKeyAddress TokenReceiver { get; set; }

        [BsonIgnore]
        public BitcoinPubKeyAddress UtxoReceiver { get; set; }

        public decimal Amount { get; set; }

        public string ReferenceCode { get; set; }

        public DateTime Time { get; set; }

        public ProcessStatus Status { get; set; }

        [BsonIgnore]
        public decimal Fee { get; set; }

        public byte[] TokenName { get; set; }
        [BsonIgnore]
        public string TokenHex
        {
            get
            {
                return TokenName.ByteArrayToString();
            }
            set
            {
                TokenName = value.StringToByteArray();
            }
        }
        [BsonIgnore]
        public string TokenString
        {
            get
            {
                return Encoding.Default.GetString(TokenName.SkipWhile(t => t == 0).ToArray());
            }
        }

        [BsonIgnore]
        private WalletContract _contract;

        [BsonIgnore]
        public bool IsSaved => (!string.IsNullOrEmpty(TxId)) && (Blockheight != 0);

        public Ledger()
        {

        }

        internal Ledger(ContractService contractService, WalletContract contract, OperationCode operation, BitcoinPubKeyAddress utxoReceiver, BitcoinPubKeyAddress tokenReceiver = null, decimal? amount = null, string referenceCode = null)
        {
            _contract = contract;
            Operation = operation;
            TokenReceiver = tokenReceiver;
            TokenReceiverHash = tokenReceiver.ExtractWitnessProgram();
            ReferenceCode = referenceCode;
            UtxoReceiver = utxoReceiver;
            if (operation == OperationCode.Issue && tokenReceiver == null)
            {
                TokenReceiverHash = new BitcoinPubKeyAddress(contract.OwnerPublicAddress, contractService.MainNetwork).ExtractWitnessProgram();
            }
            if (amount < 0)
            {
                throw new InvalidOperationException("The amount cannot be negative.");
            }
            if (amount.HasValue)
            {
                Amount = amount.Value;
            }
            else if (operation != OperationCode.Issue)
            {
                throw new ArgumentNullException("Amount cannot be null if the operation is not issuance of the owner address.");
            }
            else
            {
                Amount = contract.TotalSupply;
            }
        }

        public string GetOP_RETURN()
        {
            // 16 + 2 + 20 + 16 + 
            var a = _contract.NameHex;
            var b = BitConverter.ToString(BitConverter.GetBytes((ushort)Operation).Reverse().ToArray()).Replace("-", "");
            var c = TokenReceiverHashHex;
            var roundedAmount = Math.Round(Amount, _contract.NoOfDecimal);
            var d = BitConverterExtension.GetHexBytes(roundedAmount);

            string validatedReferenceCode = ReferenceCode;
            string e = "";
            if (!string.IsNullOrEmpty(ReferenceCode))
            {
                if (ReferenceCode.Length > 20)
                {
                    validatedReferenceCode = ReferenceCode.Substring(0, Math.Min(ReferenceCode.Length, 20));
                }

                e = BitConverter.ToString(Encoding.Default.GetBytes(validatedReferenceCode)).Replace("-", "").ToLower();
            }
            var output = $"{a}{b}{c}{d}{e}";
            return output.Substring(0, Math.Min(output.Length, 160));
        }


    }

    public enum ProcessStatus : ushort
    {
        NotProcessed = 0x00,
        Processed = 0x01,
        FailedIgnore = 0x02,
        FeatureNotAvailable = 0x03,
        NotDefined = 0x04,
        DustAmount = 0x05
    }

    public enum OperationCode : ushort
    {
        Transfer = 0x00,
        Fee = 0x01,
        Burn = 0x02,
        Interest = 0x03,
        Issue = 0x04
    }
}
