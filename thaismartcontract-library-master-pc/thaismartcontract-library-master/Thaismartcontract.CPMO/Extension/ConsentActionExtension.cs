using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thaismartcontract.API.Extension;
using Thaismartcontract.API.Model;
using Thaismartcontract.CPMO.Model;

namespace Thaismartcontract.CPMO.Extension
{
    public static class ConsentActionExtension
    {
        /// <summary>
        /// Extract ConsentAction information from OP_RETURN part
        /// </summary>
        /// <param name="transaction">The transaction to be extracted information from</param>
        /// <returns>If information is present, ConsentAction object is returned. Otherwise, null is returned.</returns>
        public static ConsentAction AsConsentAction(this ApiTransaction transaction)
        {
            try
            {
                var ca = new ConsentAction();
                var op_return = transaction.GetOP_RETURN();
                if (string.IsNullOrEmpty(op_return) || !op_return.StartsWith("43504d4f2d5448"))
                    return null;

                var firstIndex = op_return.IndexOf("3a");
                ca.Domain = Encoding.ASCII.GetString(FromHex(op_return.Substring(0, firstIndex)));
                var secondIndex = op_return.IndexOf("3a", firstIndex + 2);
                var action = Encoding.ASCII.GetString(FromHex(op_return.Substring(firstIndex + 2, secondIndex - firstIndex - 2)));
                ca.DocumentHash = op_return.Substring(secondIndex + 2);

                if (ca.DocumentHash.Length > 64)
                {
                    return null;
                }

                if (action == "ASK")
                {
                    ca.AskFrom = transaction.vin[0].addr;
                    ca.AskTxId = transaction.txid;
                    ca.AskTick = transaction.blocktime;
                    ca.ApproveFrom = transaction.vout.First(v => v.scriptPubKey.addresses != null && v.scriptPubKey.addresses[0] != ca.AskFrom).scriptPubKey.addresses[0];
                    return ca;
                }
                else if (action == "APPROVE")
                {
                    ca.ApproveFrom = transaction.vin[0].addr;
                    ca.AskFrom = transaction.vout.First(v => v.scriptPubKey.addresses != null && v.scriptPubKey.addresses[0] != ca.AskFrom).scriptPubKey.addresses[0];
                    ca.ApproveTxId = transaction.txid;
                    ca.ApprovedTick = transaction.blocktime;
                    return ca;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }

        }

        public static List<ConsentAction> OnlyUnApproved(this List<ConsentAction> consents)
        {
            return consents.Where(c => !c.IsApproved).ToList();
        }

        public static List<ConsentAction> OnlyApproved(this List<ConsentAction> consents)
        {
            return consents.Where(c => c.IsApproved).ToList();
        }

        public static byte[] FromHex(string hex)
        {
            hex = hex.Replace("-", "");
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return raw;
        }
    }
}
