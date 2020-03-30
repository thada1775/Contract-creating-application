using NBitcoin;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Thaismartcontract.API.Extension;
using Thaismartcontract.WalletService.Model;
using Thaismartcontract.API.Model;

namespace Thaismartcontract.WalletService.Extension
{
    public static class ApiTransactionExtension
    {
        public static Ledger ToLedger(this ApiTransaction transaction, WalletContract contract, ContractService service)
        {
            var ledger = new Ledger()
            {
                TxId = transaction.txid,
                Blockheight = transaction.blockheight,
                Time = new DateTime(1970, 1, 1).AddSeconds(transaction.blocktime)
            };
            var domain = BitConverter.ToString(Encoding.Default.GetBytes("RMUTSB.AC.TH")).Replace("-", "").ToLower();
            //var domain = "54534357414c4c4554";
            var data = transaction.GetOP_RETURN();
            try
            {
                if (data.StartsWith(domain))
                {
                    return null;
                }
                var contractNameHex = data.Substring(0, 16);
                if (contract.NameHex == contractNameHex)
                {
                    ledger.Operation = (OperationCode)(ushort.Parse(data.Substring(16, 4), System.Globalization.NumberStyles.HexNumber));
                    var publicAddress = transaction.vin.First(v => !string.IsNullOrEmpty(v.addr)).addr;
                    var publicKey = new BitcoinPubKeyAddress(publicAddress, service.MainNetwork);
                    ledger.TokenSenderHash = publicKey.ExtractWitnessProgram();
                    ledger.TokenReceiverHashHex = data.Substring(20, 40);
                    ledger.Amount = BitConverterExtension.ToDecimal(data.Substring(60, 32).StringToByteArray());
                    if (data.Length > 92)
                    {
                        ledger.ReferenceCode = Encoding.Default.GetString(data.Substring(92).StringToByteArray());
                    }
                    return ledger;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

    }
}
