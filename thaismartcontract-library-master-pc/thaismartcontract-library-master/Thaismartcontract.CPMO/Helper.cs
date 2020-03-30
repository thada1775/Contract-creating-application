using NBitcoin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thaismartcontract.API;
using Thaismartcontract.API.Model;
using Thaismartcontract.CPMO.Extension;
using Thaismartcontract.CPMO.Model;

namespace Thaismartcontract.CPMO
{
    public abstract class Helper
    {
        protected IInsightAPI api;
        protected BitcoinSecret privateKey;
        public Network MainNetwork { get; protected set; }
        protected BitcoinPubKeyAddress publicKey;

        /// <summary>
        /// Helper Constructor settings the default variables. (unable to instantiate)
        /// </summary>
        /// <param name="privateKey">User's private key</param>
        /// <param name="network">NBitcoin-compatible blockchain network</param>
        /// <param name="api">Insight-based block explorer (>0.4)</param>
        /// <returns></returns>
        public Helper(string privateKey, Network network = null, IInsightAPI api = null)
        {
            MainNetwork = network ?? NBitcoin.Altcoins.Digibyte.Instance.Mainnet;
            this.privateKey = new BitcoinSecret(privateKey, MainNetwork);
            this.api = api ?? new DigibyteAPI();
            publicKey = this.privateKey.GetAddress();
        }

        /// <summary>
        /// List all consents associated with the public key.
        /// </summary>
        /// <param name="publicKey">User's public key (If not supplied, the public key associated with the private key is used)</param>
        /// <returns>List of merged ConsentAction object.</returns>
        public async Task<List<ConsentAction>> ListAllConsent(string publicKey = null)
        {
            var myAddress = publicKey ?? this.publicKey.ToString() ;
            var allTransaction = await api.GetAllTransaction(myAddress);
            var allConsents = new List<ConsentAction>();
            foreach (var tx in allTransaction.txs)
            {
                var ca = tx.AsConsentAction();
                if (ca != null)
                {
                    allConsents.Add(ca);
                }
            }
            return MergeConsent(allConsents);
        }

        /// <summary>
        /// Merge paired ConsentAction from two transactions to one object.
        /// </summary>
        /// <param name="consents">List of ConsentAction to be merged</param>
        /// <returns>List of merged ConsentAction object.</returns>
        private List<ConsentAction> MergeConsent(List<ConsentAction> consents)
        {
            var result = new List<ConsentAction>();
            consents.Reverse();
            foreach (var ca in consents)
            {
                if (ca.IsAsked)
                {
                    result.Add(ca);
                }
                if (ca.IsApproved)
                {
                    var matchedAsk = result.FirstOrDefault(c => c.Domain == ca.Domain && 
                                                                c.AskFrom == ca.AskFrom && 
                                                                c.ApproveFrom == ca.ApproveFrom && 
                                                                c.DocumentHash == ca.DocumentHash);
                    if (matchedAsk != null)
                    {
                        matchedAsk.ApproveTxId = ca.ApproveTxId;
                        matchedAsk.ApprovedTick = ca.ApprovedTick;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Obtain the cryptographic key generator.
        /// </summary>
        /// <returns>Call GenerateKeyPair() or GenerateKeyPairFromSeed() to create a new key pair.</returns>
        public IKeyGenerator KeyGenerator() => new DigibyteKeyGenerator();


        /// <summary>
        /// Broadcast transaction to blockchain.
        /// </summary>
        /// <param name="transaction">Signed NBitcoin-compatible transaction</param>
        /// <returns>TransactionID of the published transaction.</returns>
        public Task<TxID> BroadCastTransaction(Transaction transaction) => api.BroadcastTransaction(transaction);

    }
}
