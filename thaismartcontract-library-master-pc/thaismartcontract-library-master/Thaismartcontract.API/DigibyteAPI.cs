using NBitcoin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Thaismartcontract.API.Extension;
using Thaismartcontract.API.Model;
using Block = Thaismartcontract.API.Model.Block;

namespace Thaismartcontract.API
{
    public class DigibyteAPI : IInsightAPI
    {
        protected readonly HttpClient client;
        protected readonly Network network;
        protected readonly decimal fee;
        protected int ThreadholdRate = 6;
        public JsonSerializerSettings JsonSettings { get; private set; }
        //private readonly BitcoinPubKeyAddress provisionAddress;
        //private readonly Money license = new Money(0.099m, MoneyUnit.BTC);


        /// <summary>
        /// A default constructor of Digibyte blockchain API.
        /// </summary>
        /// <returns>A Digibyte blockchain API provides an interface to indexed blockchain database.</returns>
        public DigibyteAPI()
        {
            JsonSettings = new JsonSerializerSettings();
            network = NBitcoin.Altcoins.Digibyte.Instance.Mainnet;
            NBitcoin.JsonConverters.Serializer.RegisterFrontConverters(JsonSettings, network);
            fee = 0.00001m;
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //client.BaseAddress = new Uri("https://explorer.Thaismartcontract.com");
            //client.BaseAddress = new Uri("https://digibyteblockexplorer.com");
            client.BaseAddress = new Uri("https://digiexplorer.info");
            //provisionAddress = new BitcoinPubKeyAddress("D7fyCDHUo3mc9E7HykTppjuv9D6BbL8u5a", network);
        }

        /// <summary>
        /// A constructor of Digibyte blockchain API.
        /// </summary>
        /// <param name="options">Customized Insight-compatible API option</param>
        /// <returns>A Digibyte blockchain API provides an interface to indexed blockchain database.</returns>
        public DigibyteAPI(APIOptions options)
        {
            network = NBitcoin.Altcoins.Digibyte.Instance.Mainnet;
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri($"{options.BaseURL}");
            if (options.ThreadholdRate != 0)
                ThreadholdRate = options.ThreadholdRate;
            fee = 0.00001m;
            //provisionAddress = new BitcoinPubKeyAddress("D7fyCDHUo3mc9E7HykTppjuv9D6BbL8u5a", network);
        }

        public async Task<List<UTXO>> ListUnspentByAddress(string address)
        {
            var result = await client.GetStringAsync($"/api/addr/{address}/utxo");
            return JsonConvert.DeserializeObject<List<UTXO>>(result);
        }

        public Task<Transaction> BuildTransaction(
                                                    BitcoinSecret senderPrivateKey,
                                                    BitcoinPubKeyAddress receiverPublicKey,
                                                    string hexData)
        {
            return BuildTransaction(senderPrivateKey, receiverPublicKey, receiverPublicKey, hexData);
        }

        public Task<Transaction> BuildTransaction(
                                            string senderPrivateKey,
                                            string receiverPublicKey,
                                            string hexData)
        {
            return BuildTransaction(senderPrivateKey, receiverPublicKey, senderPrivateKey, hexData);
        }

        public async Task<Transaction> BuildTransaction(
                                            string senderPrivateKey,
                                            string receiverPublicKey,
                                            string futureReceiverPublicKey,
                                            string hexData)
        {
            BitcoinSecret senderSecret = new BitcoinSecret(senderPrivateKey, network);
            BitcoinPubKeyAddress receiverPub = new BitcoinPubKeyAddress(receiverPublicKey, network);
            BitcoinPubKeyAddress futureReceiverPub = new BitcoinPubKeyAddress(futureReceiverPublicKey, network);
            return await BuildTransaction(senderSecret, receiverPub, futureReceiverPub, hexData);
        }

        public async Task<Transaction> BuildTransaction(
                                            BitcoinSecret senderPrivateKey,
                                            BitcoinPubKeyAddress receiverPublicKey,
                                            BitcoinPubKeyAddress futureSenderPublicKey,
                                            string hexData)
        {
            decimal savedRequiredAmount = 0;
            var futureSender = await GetAddress(futureSenderPublicKey.ToString());
            if (futureSender.balance < fee * 2)
            {
                savedRequiredAmount = fee * ThreadholdRate;
            }
            else
            {
                savedRequiredAmount = fee;
            }
            var requiredAmount = savedRequiredAmount + fee;
            //var requiredAmount = savedRequiredAmount + fee + license.ToDecimal(MoneyUnit.BTC);
            var txBuilder = network.CreateTransactionBuilder().AddKeys(senderPrivateKey);
            var allUtxo = await ListUnspentByAddress(senderPrivateKey.GetAddress().ToString());
            allUtxo = allUtxo.OrderBy(u => u.amount).ToList();
            var index = 0;
            var sumSentAmount = 0m;
            try
            {
                while (requiredAmount > 0)
                {
                    txBuilder = txBuilder.AddCoins(allUtxo[index].AsCoin());
                    requiredAmount -= allUtxo[index].amount;
                    sumSentAmount += allUtxo[index].amount;
                    index++;
                }
            }
            catch (Exception)
            {
                throw new Exception("No enough money for fee transactions.");
            }

            var tx = txBuilder
                 .Send(futureSenderPublicKey, new Money(savedRequiredAmount, MoneyUnit.BTC))
                 .Send(new Script("OP_RETURN " + hexData), Money.Zero)
                 //.Send(provisionAddress, license)
                 .SendFees(new Money(fee, MoneyUnit.BTC))
                 .SetChange(senderPrivateKey.GetAddress())
                 .BuildTransaction(true);

            if (txBuilder.Verify(tx))
            {
                return tx;
            }
            else
            {
                throw new InvalidOperationException("Cannot build transaction according to the given private key.");
            }
        }

        public async Task<Transaction> BuildTransaction(
                                            BitcoinSecret senderPrivateKey,
                                            BitcoinPubKeyAddress receiverPublicKey,
                                            decimal provisionAmount,
                                            string hexData,
                                            string inputTransaction = null)
        {
            var requiredAmount = provisionAmount + fee;
            var txBuilder = network.CreateTransactionBuilder().AddKeys(senderPrivateKey);
            var allUtxo = await ListUnspentByAddress(senderPrivateKey.GetAddress().ToString());
            if (string.IsNullOrEmpty(inputTransaction))
            {
                allUtxo = allUtxo.OrderBy(u => u.amount).ToList();
            }
            else
            {
                allUtxo = allUtxo.Where(u => u.txid == inputTransaction).ToList();
                if (allUtxo.Count == 0)
                {
                    throw new Exception("The specified transaction id is not found.");
                }
            }

            var index = 0;
            var sumSentAmount = 0m;
            try
            {
                while (requiredAmount > 0)
                {
                    txBuilder = txBuilder.AddCoins(allUtxo[index].AsCoin());
                    requiredAmount -= allUtxo[index].amount;
                    sumSentAmount += allUtxo[index].amount;
                    index++;
                }
            }
            catch (Exception)
            {
                throw new Exception("No enough money for fee transactions.");
            }

            var tx = txBuilder
                 .Send(receiverPublicKey, new Money(provisionAmount, MoneyUnit.BTC))
                 .Send(new Script("OP_RETURN " + hexData), Money.Zero)
                 .SendFees(new Money(fee, MoneyUnit.BTC))
                 .SetChange(senderPrivateKey.GetAddress())
                 .BuildTransaction(true);

            if (txBuilder.Verify(tx))
            {
                return tx;
            }
            else
            {
                throw new InvalidOperationException("Cannot build transaction according to the given private key.");
            }
        }

        public async Task<Transaction> BuildTransactionQuestion(
                                            BitcoinSecret providerPrivateKey,
                                            BitcoinPubKeyAddress userPublicKey,
                                            string data)
        {
            var provisionAmount = 2 * fee;
            return await BuildTransaction(providerPrivateKey, userPublicKey, provisionAmount, data);
        }

        public async Task<Transaction> BuildTransactionAnswer(
                                            BitcoinSecret userPrivateKey,
                                            string QuestionTxID,
                                            string data = null)
        {

            var apiTransaction = await GetTransactionInfo(QuestionTxID);
            string question = apiTransaction.GetOP_RETURN();
            var providerPublicKey = new BitcoinPubKeyAddress(apiTransaction.vin[0].addr, network);
            string answer;
            if (data is null)
            {
                // REPLACE ASK TO APPROVE OP_RETURN 48 bytes;
                answer = question.Replace("41534b", "415050524f5645");
            }
            else
            {
                var ba = Encoding.ASCII.GetBytes(data);
                answer = BitConverter.ToString(ba).Replace("-", "");
            }
            decimal provisionLeft = fee;

            return await BuildTransaction(userPrivateKey, providerPublicKey, provisionLeft, answer, QuestionTxID);
        }

        public async Task<ApiTransaction> GetTransactionInfo(string txid)
        {
            int repeatLoop = 15;
        start:
            try
            {
                var result = await client.GetStringAsync($"/api/tx/{txid}");
                var output = JsonConvert.DeserializeObject<ApiTransaction>(result);

                if (output.blocktime != 0 && output.blockheight == 0)
                {
                    output.blockheight = await GetBlockHeight(output.blockhash);
                }
                return output;
            }
            catch (Exception)
            {
                if (repeatLoop > 0)
                {
                    Thread.Sleep(1000);
                    repeatLoop -= 1;
                    goto start;
                }
                else
                {
                    return null;
                }
            }

        }

        public async Task<TxID> BroadcastTransaction(Transaction tx)
        {
            var txBuilder = network.CreateTransactionBuilder();
            if (txBuilder.Verify(tx))
            {
                throw new InvalidOperationException("Transaction cannot be verified.");
            }
            var hex = tx.ToHex();
            var data = new RawTX()
            {
                rawtx = hex
            };

            try
            {
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var result = await client.PostAsync($"/api/tx/send", content);
                result.EnsureSuccessStatusCode();
                var output = await result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TxID>(output);
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Transaction cannot be send through Insight-based API.");
            }

        }

        public async Task<ApiTransactionCollections> GetAllTransaction(string address, int limitHeight = 0)
        {
            int count = 3;
            while (count >= 0)
            {
                try
                {
                    int i = 0;
                    int totalPage = 1;
                    ApiTransactionCollections allTxs = new ApiTransactionCollections();
                    allTxs.txs = new List<ApiTransaction>();
                    bool stopLimit = false;
                    while (i < totalPage && !stopLimit)
                    {
                        var result = await client.GetStringAsync($"/api/txs/?address={address}&pageNum={i}");       //not respond when Disconnect
                        var tempTxs = JsonConvert.DeserializeObject<ApiTransactionCollections>(result);
                        totalPage = tempTxs.pagesTotal;
                        foreach (var tx in tempTxs.txs)
                        {
                            var height = await GetBlockHeight(tx.blockhash);
                            if (height > limitHeight)
                            {
                                allTxs.txs.Add(tx);
                            }
                            else
                            {
                                stopLimit = true;
                                break;
                            }
                        }
                        i++;
                    }
                    allTxs.pagesTotal = i;
                    return allTxs;
                }
                catch (Exception)
                {
                    Thread.Sleep(3000);
                    count--;
                    continue;
                }
            }
            
            throw new Exception("Cannot connect to API block explorer.");

        }

        public async Task<ApiTransactionCollections> GetAllTransactionByPage(string address, int page = 0)
        {
            ApiTransactionCollections allTxs = new ApiTransactionCollections();
            allTxs.txs = new List<ApiTransaction>();
            var result = await client.GetStringAsync($"/api/txs/?address={address}&pageNum={page}");
            var tempTxs = JsonConvert.DeserializeObject<ApiTransactionCollections>(result);
            return tempTxs;
            //foreach (var tx in tempTxs.txs)
            //{
            //    allTxs.txs.Add(tx);
            //}
            //allTxs.pagesTotal = page;
            //return allTxs;
        }

        public async Task<Address> GetAddress(string address)
        {
            var result = await client.GetStringAsync($"/api/addr/{address}");
            return JsonConvert.DeserializeObject<Address>(result);
        }

        public async Task<Sync> GetSync()
        {
            var result = await client.GetStringAsync($"/api/sync");
            return JsonConvert.DeserializeObject<Sync>(result);
        }

        public async Task<bool> IsConfirmedAsync(string txID)
        {
            var result = await client.GetStringAsync($"/api/tx{txID}");
            var tempTxs = JsonConvert.DeserializeObject<ApiTransaction>(result);
            return tempTxs.confirmations >= 1;
        }

        public async Task<int> GetBlockHeight(string blockhash)
        {
            var result = await client.GetStringAsync($"/api/block/{blockhash}");
            var blockData = JsonConvert.DeserializeObject<Block>(result);
            return blockData.height;
        }

        public async Task<decimal> GetBalance(string pubkey)        //Add Get Balance
        {
            var result = await client.GetStringAsync($"/api/addr/{pubkey}");
            var addr = JsonConvert.DeserializeObject<Address>(result);
            return addr.balance;
        }
    }
}
