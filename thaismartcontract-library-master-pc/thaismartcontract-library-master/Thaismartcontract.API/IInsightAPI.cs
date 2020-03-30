using NBitcoin;
using System.Collections.Generic;
using System.Threading.Tasks;
using Thaismartcontract.API.Model;

namespace Thaismartcontract.API
{
    public interface IInsightAPI
    {
        Task<List<UTXO>> ListUnspentByAddress(string address);
        Task<Transaction> BuildTransaction(string senderPrivateKey, string receiverPublicKey, string hexData);
        Task<Transaction> BuildTransaction(string senderPrivateKey, string receiverPublicKey, string futureReceiverPublicKey, string hexData);
        Task<Transaction> BuildTransaction(BitcoinSecret senderPrivateKey, BitcoinPubKeyAddress receiverPublicKey, BitcoinPubKeyAddress futureSenderPublicKey, string hexData);
        Task<Transaction> BuildTransaction(BitcoinSecret senderPrivateKey, BitcoinPubKeyAddress receiverPublicKey, decimal provisionAmount, string hexData, string inputTransaction = null);
        Task<Transaction> BuildTransactionQuestion(BitcoinSecret providerPrivateKey, BitcoinPubKeyAddress userPublicKey, string data);
        Task<Transaction> BuildTransactionAnswer(BitcoinSecret userPrivateKey, string QuestionTxID, string data = null);
        Task<ApiTransaction> GetTransactionInfo(string txid);
        Task<TxID> BroadcastTransaction(Transaction tx);
        Task<ApiTransactionCollections> GetAllTransaction(string address, int limitHeight = 0);
        Task<ApiTransactionCollections> GetAllTransactionByPage(string address, int page = 0);
        Task<Address> GetAddress(string address);
        Task<Sync> GetSync();
        Task<bool> IsConfirmedAsync(string txID);
        Task<int> GetBlockHeight(string blockhash);
    }
}
