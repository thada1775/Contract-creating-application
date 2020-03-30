using System.Linq;
using Thaismartcontract.API.Model;

namespace Thaismartcontract.API.Extension
{
    public static class ApiTransactionExtension
    {
        public static string GetOwnerAddress(this ApiTransaction transaction)
        {
            var inAddr = transaction.vin.FirstOrDefault(v => v.addr != null).addr;
            var outAddr = transaction.vout.FirstOrDefault(v => v.scriptPubKey.addresses != null).scriptPubKey.addresses.FirstOrDefault();

            if (inAddr == outAddr)
                return inAddr;
            else
                return null;

        }

        public static string GetOP_RETURN(this ApiTransaction transaction)
        {
            var tx = transaction.vout.FirstOrDefault(v => v.scriptPubKey.hex.StartsWith("6a"));
            if (tx != null)
            {
                if (tx.scriptPubKey.hex.StartsWith("6a4c50"))
                {
                    return tx.scriptPubKey.hex.Substring(6).ToLower();
                }
                else
                {
                    return tx.scriptPubKey.hex.Substring(4).ToLower();
                }
            }
            else
            {
                return null;
            }
        }

        public static bool CheckSenderExists(this ApiTransaction transaction, string publicKey)
        {
            return transaction.vin.Exists(vin => vin.addr == publicKey);
        }

        public static string GetFirstSender(this ApiTransaction transaction)
        {
            return transaction.vin.First(vin => !string.IsNullOrEmpty(vin.addr)).addr;
        }
        public static bool CheckReceiverExists(this ApiTransaction transaction, string publicKey)
        {
            return transaction.vout.Exists(vout => vout.scriptPubKey.addresses != null && vout.scriptPubKey.addresses.Contains(publicKey));
        }
        public static bool CheckReceiverAmount(this ApiTransaction transaction, string publicKey, decimal matchValue)
        {
            return transaction.vout.Exists(vout => vout.scriptPubKey.addresses != null && vout.scriptPubKey.addresses.Contains(publicKey)
                                                    && decimal.Parse(vout.value) == matchValue);
        }
    }
}
