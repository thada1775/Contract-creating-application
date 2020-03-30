using NBitcoin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Thaismartcontract.API;
using Thaismartcontract.API.Extension;
using Thaismartcontract.API.Model;
using Thaismartcontract.CPMO.Extension;
using Thaismartcontract.CPMO.Model;

namespace Thaismartcontract.CPMO
{
    public class ClientHelper : Helper
    {
        /// <summary>
        /// A constructor of helper class for client part which handle consent approval.
        /// </summary>
        /// <param name="privateKey">User's private key</param>
        /// <param name="network">NBitcoin-compatible blockchain network</param>
        public ClientHelper(string privateKey, Network network = null) : base(privateKey, network)
        {
        }

        /// <summary>
        /// Create a transaction that approve the consent according to the consent ask.
        /// </summary>
        /// <param name="questionTxId">TxID object from BroadcastTransaction() of the consent ask</param>
        /// <returns>Signed NBitcoin-compatible transaction. Use ToHex() in case of manual broadcast.</returns>
        public Task<Transaction> CreateApprove(TxID questionTxId) => CreateApprove(questionTxId.txid);


        /// <summary>
        /// Create a transaction that approve the consent according to the consent ask.
        /// </summary>
        /// <param name="questionTxId">A transaction ID of the consent ask</param>
        /// <returns>Signed NBitcoin-compatible transaction. Use ToHex() in case of manual broadcast.</returns>
        public Task<Transaction> CreateApprove(string questionTxId)
        {
            return api.BuildTransactionAnswer(privateKey, questionTxId);
        }

        
    }
}
