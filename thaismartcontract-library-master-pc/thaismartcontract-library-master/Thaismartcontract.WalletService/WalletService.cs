using LiteDB;
using NBitcoin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Thaismartcontract.API;
using Thaismartcontract.API.Extension;
using Thaismartcontract.API.Model;
using Thaismartcontract.WalletService.Extension;
using Thaismartcontract.WalletService.Model;

namespace Thaismartcontract.WalletService
{
    public class WalletService
    {
        private readonly LiteDatabase db;
        private readonly IInsightAPI api;
        private readonly LiteCollection<Ledger> ledger;
        private readonly LiteCollection<Account> account;
        private readonly WalletContract contract;
        private readonly ContractService contractService;
        private readonly BitcoinSecret userPrivateKey;
        private readonly decimal TransferFee;
        private readonly BitcoinPubKeyAddress ownerAddress;

        /// <summary>
        /// Retrieve the wallet contract that own this wallet service.
        /// </summary>
        /// <returns>A wallet contract object.</returns>
        public WalletContract GetWalletContract()
        {
            return contract;
        }

        /// <summary>
        /// Retrieve the contract service that own this wallet service.
        /// </summary>
        /// <returns>A wallet service object.</returns>
        public ContractService GetContractService()
        {
            return contractService;
        }

        /// <summary>
        /// A default constructor of Wallet service. (Need to initialized from a ContractService object.)
        /// </summary>
        /// <param name="contract">A wallet contract object</param>
        /// <param name="contractService">A wallet contract service object</param>
        /// <param name="db">LiteDB database object</param>
        /// <param name="userPrivateKey">User's private key</param>
        /// <param name="api">Insight-based compatible API (>0.4)</param>
        /// <param name="TransferFee">Fee for token transfer</param>
        /// <returns>A wallet service is used to obtain all the ledger from the wallet. Private key is used to create the ledger on the user's behalf.</returns>
        internal WalletService(WalletContract contract, ContractService contractService, LiteDatabase db, BitcoinSecret userPrivateKey, IInsightAPI api, decimal TransferFee)
        {
            this.TransferFee = TransferFee;
            this.userPrivateKey = userPrivateKey;
            this.contractService = contractService;
            this.contract = contract;
            this.api = api;
            this.db = db;
            byte[] myByte = Encoding.ASCII.GetBytes(contract.ID);
            var encrypted = NBitcoin.Crypto.Hashes.RIPEMD160(myByte, myByte.Length);
            var hashID = encrypted.ByteArrayToString();
            ledger = db.GetCollection<Ledger>($"Ledger-{hashID}");
            account = db.GetCollection<Account>($"Account-{hashID}");
            ownerAddress = new BitcoinPubKeyAddress(contract.OwnerPublicAddress, contractService.MainNetwork);
        }

        /// <summary>
        /// Query the blockchain API if there is any new unprocessed transactions.
        /// </summary>
        /// <returns>True if there is a new transaction. Otherwise returns false.</returns>
        public async Task<bool> IsNewTransactionAvailable()
        {
            var txs = await api.GetAllTransaction(contract.OwnerPublicAddress, contract.LastSyncedBlock);
            var ledgers = ledger.Find(l => l.Status == ProcessStatus.NotProcessed);
            return (txs.txs.Count() > 0) || (ledgers.Count() > 0);
        }

        /// <summary>
        /// Obtain the ledgers from blockchain and process the account balance.
        /// </summary>
        /// <param name="reset">If true, remove all existing ledgers and re-sync from the first ledger
        /// (Optional, default is false)</param>
        /// <returns>None.</returns>
        public async Task Rescan(bool reset = false)
        {
            int startSyncBlock = Math.Max(contract.LastSyncedBlock, contract.StartingBlock);
            if (reset)
            {
                startSyncBlock = contract.StartingBlock;
                contract.LastSyncedBlock = startSyncBlock;
                db.DropCollection($"Ledger-{contract.ID.Substring(0, 32)}");
                db.DropCollection($"Account-{contract.ID.Substring(0, 32)}");
            }
            var txs = await api.GetAllTransaction(contract.OwnerPublicAddress, startSyncBlock);
            await ProcessingLedger(txs);
            UpdateBalance(reset);
        }

        /// <summary>
        /// A private method to process transactions from API and store as ledgers in local data store.
        /// </summary>
        /// <param name="txs">Transactions from API</param>
        /// <returns>None.</returns>
        private async Task ProcessingLedger(ApiTransactionCollections txs)
        {
            if (txs.txs.Count() > 0)
            {
                foreach (var tx in txs.txs)
                {
                    tx.blockheight =  await api.GetBlockHeight(tx.blockhash);
                    var record = tx.ToLedger(contract, contractService);
                    if (record != null && !ledger.Exists(Query.EQ("TxId", record.TxId)))
                    {
                        record.Status = ProcessStatus.NotProcessed;
                        ledger.Upsert(record);
                    }
                }
                ledger.EnsureIndex(l => l.TxId);
                ledger.EnsureIndex(l => l.Blockheight);
                ledger.EnsureIndex(l => l.Operation);
                ledger.EnsureIndex(l => l.TokenSenderHash);
                ledger.EnsureIndex(l => l.TokenReceiverHash);
                ledger.EnsureIndex(l => l.Time);
                contract.LastSyncedBlock = txs.txs.Max(t => t.blockheight);
                contractService.UpdateContract(contract);
            }
        }

        /// <summary>
        /// A private method to process the account balance from the ledgers in local data store.
        /// </summary>
        /// <param name="reset">If true, the existing balance will be removed and process again (Optional, default is false)</param>
        /// <returns>None.</returns>
        private void UpdateBalance(bool reset = false)
        {
            var ledgers = ledger.Find(l => l.Status == ProcessStatus.NotProcessed);
            if (reset)
            {
                db.DropCollection($"Account-{contract.NameHex}");
                ledgers = ledger.FindAll();
            }

            foreach (var entry in ledgers)
            {
                switch (entry.Operation)
                {
                    case OperationCode.Transfer:
                        var fromAccount = account.FindOne(a => a.WitnessProgram == entry.TokenSenderHash);
                        if (fromAccount == null)
                        {
                            fromAccount = new Account()
                            {
                                WitnessProgram = entry.TokenSenderHash
                            };
                        }
                        else if (entry.Amount < (decimal)Math.Round(Math.Pow(0.1, contract.NoOfDecimal), contract.NoOfDecimal))
                        {
                            entry.Status = ProcessStatus.DustAmount;
                            ledger.Upsert(entry);
                        }
                        else if (fromAccount.Balance >= entry.Amount + TransferFee)
                        {
                            fromAccount.Balance -= entry.Amount + TransferFee;
                            var toAccount = account.FindOne(a => a.WitnessProgram == entry.TokenReceiverHash);
                            if (toAccount == null)
                            {
                                toAccount = new Account()
                                {
                                    WitnessProgram = entry.TokenReceiverHash
                                };
                            }
                            toAccount.Balance += entry.Amount;
                            fromAccount.Blockheight = entry.Blockheight;
                            toAccount.Blockheight = entry.Blockheight;
                            account.Upsert(fromAccount);
                            account.Upsert(toAccount);
                            var ownerAccount = account.FindOne(a => a.WitnessProgram == ownerAddress.ExtractWitnessProgram());
                            if (ownerAccount == null)
                            {
                                ownerAccount = new Account()
                                {
                                    WitnessProgram = entry.TokenReceiverHash
                                };
                            }
                            ownerAccount.Balance += TransferFee;
                            ownerAccount.Blockheight = entry.Blockheight;
                            account.Upsert(ownerAccount);
                            account.EnsureIndex(a => a.WitnessProgram);
                            entry.Status = ProcessStatus.Processed;
                            ledger.Upsert(entry);
                        }
                        else
                        {
                            entry.Status = ProcessStatus.FailedIgnore;
                            ledger.Upsert(entry);
                        }
                        break;
                    case OperationCode.Burn:
                        entry.Status = ProcessStatus.FeatureNotAvailable;
                        ledger.Upsert(entry);
                        break;
                    case OperationCode.Interest:
                        entry.Status = ProcessStatus.FeatureNotAvailable;
                        ledger.Upsert(entry);
                        break;
                    case OperationCode.Issue:
                        if (entry.TokenSenderHash.QuickCompare(ownerAddress.ExtractWitnessProgram()))
                        {
                            var ownerAccount = account.FindOne(a => a.WitnessProgram == ownerAddress.ExtractWitnessProgram());
                            if (ownerAccount == null)
                            {
                                ownerAccount = new Account()
                                {
                                    WitnessProgram = entry.TokenReceiverHash
                                };
                            }
                            ownerAccount.Balance += entry.Amount;
                            ownerAccount.Blockheight = entry.Blockheight;
                            account.Upsert(ownerAccount);
                            account.EnsureIndex(a => a.WitnessProgram);
                            entry.Status = ProcessStatus.Processed;
                        }
                        else
                        {
                            entry.Status = ProcessStatus.FailedIgnore;
                        }
                        ledger.Upsert(entry);
                        break;
                    default:
                        entry.Status = ProcessStatus.NotDefined;
                        ledger.Upsert(entry);
                        break;
                }
            }
            account.EnsureIndex(a => a.WitnessProgram);
        }

        /// <summary>
        /// A private method to build ledger settings.
        /// </summary>
        /// <param name="operation">Opertaion code of the ledger</param>
        /// <param name="toAddress">Receiver's Bitcoin-compatible address</param>
        /// <param name="amount">Amount without dust</param>
        /// <param name="referenceCode">Additional information attached to ledger</param>
        /// <returns>Ledger object ready to broadcast.</returns>
        private Ledger CreateLedgerPrivate(OperationCode operation, BitcoinPubKeyAddress toAddress, decimal? amount = null, string referenceCode = null)
        {
            var utxoReceiver = ownerAddress;
            return new Ledger(contractService, contract, operation, utxoReceiver, toAddress, amount, referenceCode);
        }

        /// <summary>
        /// Build a new ready-to-broadcast ledger.
        /// </summary>
        /// <param name="operation">Operation code of the ledger</param>
        /// <param name="toAddress">Receiver's Address (Support string, BitcoinPubKeyAddress, PubKey, Contact)</param>
        /// <param name="amount">Amount. Nullable if operation is Issue.</param>
        /// <param name="referenceCode">Additional information attached to ledger</param>
        /// <returns>Ledger object ready to broadcast.</returns>
        public Ledger CreateLedger(OperationCode operation, Object toAddress = null, decimal? amount = null, string referenceCode = null)
        {
            BitcoinPubKeyAddress toPubAddress = null;

            if (operation == OperationCode.Issue)
            {
                toAddress = contract.OwnerPublicAddress;
                amount = contract.TotalSupply;
            }

            if (toAddress != null)
            {
                try
                {
                    if (toAddress.GetType() == typeof(string))
                    {
                        toPubAddress = new BitcoinPubKeyAddress(toAddress.ToString(), contractService.MainNetwork);
                    }
                    else if (toAddress.GetType() == typeof(BitcoinPubKeyAddress))
                    {
                        toPubAddress = toAddress as BitcoinPubKeyAddress;
                    }
                    else if (toAddress.GetType() == typeof(PubKey))
                    {
                        toPubAddress = (toAddress as PubKey).GetAddress(contractService.MainNetwork);
                    }
                    else if (toAddress.GetType() == typeof(Contact))
                    {
                        toPubAddress = new BitcoinPubKeyAddress((toAddress as Contact).Address, contractService.MainNetwork);
                    }
                    else
                    {
                        throw new InvalidCastException("Unsupported Type of toAddress");
                    }
                }
                catch (Exception)
                {
                    throw new InvalidCastException("Unsupported Type of toAddress");
                }

            }
            byte[] toAddressByte = toPubAddress == null ? null : toPubAddress.ExtractWitnessProgram();
            return CreateLedgerPrivate(operation, toPubAddress, amount, referenceCode);
        }

        /// <summary>
        /// Broadcast a ledger to blockchain network.
        /// </summary>
        /// <param name="record">A ledger to broadcast.</param>
        /// <param name="waitForConfirm">Wait for ledger to store in a blockchain (Optional, default is true)</param>
        /// <returns>None. Transaction ID will be attached to the ledger object instead.</returns>
        public async Task BroadcastLedger(Ledger record, bool waitForConfirm = true)
        {
            var hexData = record.GetOP_RETURN();
            var transaction = await api.BuildTransaction(userPrivateKey, record.UtxoReceiver, record.TokenReceiver, hexData);
            var tx = await api.BroadcastTransaction(transaction);
            record.TxId = tx.txid;

            if (waitForConfirm)
            {
                var currentTx = new ApiTransaction();
                while (currentTx.blocktime == 0)
                {
                    currentTx = await api.GetTransactionInfo(tx.txid);
                    record.Blockheight = currentTx.blockheight;
                    Thread.Sleep(3000);
                }
                record.Status = ProcessStatus.NotProcessed;
                record.TokenSenderHash = userPrivateKey.PubKeyHash.ToBytes();
                ledger.Upsert(record);
                ledger.EnsureIndex(l => l.TxId);
                ledger.EnsureIndex(l => l.Blockheight);
                ledger.EnsureIndex(l => l.Operation);
                ledger.EnsureIndex(l => l.TokenSenderHash);
                ledger.EnsureIndex(l => l.TokenReceiverHash);
                ledger.EnsureIndex(l => l.Time);
            }
            else
            {
                record.Status = ProcessStatus.NotDefined;
            }


        }
        /// <summary>
        /// Retrieve the account service object that belongs to the wallet contract.
        /// </summary>
        /// <returns>Account service is used to retrieve the balance of particular account.</returns>
        public AccountService GetAccountService()
        {
            return new AccountService(contractService, contract, this, db);
        }

        //public List<Ledger> GetLedgers(int top = 5)
        //{
        //    var ledgers = ledger.Find(Query.All("Blockheight", Query.Descending), skip: 0, limit: 5).ToList();
        //    ledgers.ForEach(l => l.TokenName = contract.TokenName);
        //    return ledgers;
        //}

        /// <summary>
        /// Retrieve ledgers from the local data store in descending order of time.
        /// </summary>
        /// <param name="pubkeyHash">Filter the list only from given Pubkeyhash (Optional, default is empty)</param>
        /// <param name="skip">The number of skipped ledgers for paging (Optional, default is 0)</param>
        /// <param name="limit">Limit the number of retrieving ledgers (Optional, default is Maxvalue)</param>
        /// <returns>A list of ledgers from local data store.</returns>
        public List<Ledger> GetLedgers(string pubkeyHash = "", int skip = 0, int limit = int.MaxValue)
        {
            var ledgers = ledger.Find(Query.All("Blockheight", Query.Descending));
            if (string.IsNullOrEmpty(pubkeyHash))
            {
                ledgers = ledgers.Where(l => l.TokenReceiverHashHex == pubkeyHash || l.TokenSenderHashHex == pubkeyHash)
                                .Skip(skip)
                                .Take(limit + 1);
            }
            else
            {
                ledgers = ledgers.Take(limit);
            }
            var listLedgers = ledgers.ToList();
            listLedgers.ForEach(l => l.TokenName = contract.TokenName);
            return listLedgers;
        }

        /// <summary>
        /// Retrieve user's related ledgers from the local data store in descending order of time.
        /// </summary>
        /// <param name="skip">The number of skipped ledgers for paging (Optional, default is 0)</param>
        /// <param name="limit">Limit the number of retrieving ledgers (Optional, default is Maxvalue)</param>
        /// <returns>A list of ledgers from local data store.</returns>
        public List<Ledger> GetMyLedgers(int skip = 0, int limit = int.MaxValue)
        {
            var pubHashkey = userPrivateKey.GetAddress().Hash.ToString();
            return GetLedgers(pubHashkey, skip, limit);
        }
    }
}
