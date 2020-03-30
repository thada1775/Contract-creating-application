using LiteDB;
using NBitcoin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thaismartcontract.API;
using Thaismartcontract.API.Extension;
using Thaismartcontract.WalletService.Extension;
using Thaismartcontract.WalletService.Model;

namespace Thaismartcontract.WalletService
{
    public class ContractService
    {
        private LiteDatabase db;
        private LiteCollection<WalletContract> collection;
        private IInsightAPI insightAPI;
        public Network MainNetwork { get; private set; }
        private const string Domain = "RMUTSB.AC.TH";
        private string DomainHex;

        /// <summary>
        /// A default constructor of contract service
        /// </summary>
        /// <returns>A contract service is used to manage electronic money contract in data store.</returns>
        public ContractService()
        {
            var connectionString = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}tsc-wallet.db";
            //var connectionString = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "tsc-wallet.db");
            var api = new DigibyteAPI();
            var network = NBitcoin.Altcoins.Digibyte.Instance.Mainnet;
            ContractSetup(connectionString, api, network);
        }

        /// <summary>
        /// A constructor of contract service
        /// </summary>
        /// <param name="api">Bitpay Insight compatible API (>0.4)</param>
        /// <param name="connectionString">LiteDB connection string</param>
        /// <returns>A contract service is used to manage electronic money contract in data store.</returns>
        public ContractService(string connectionString, IInsightAPI api)
        {
            var network = NBitcoin.Altcoins.Digibyte.Instance.Mainnet;
            ContractSetup(connectionString, api, network);
        }

        /// <summary>
        /// A constructor of contract service
        /// </summary>
        /// <param name="api">Bitpay Insight compatible API (>0.4)</param>
        /// <param name="connectionString">LiteDB connection string</param>
        /// <param name="network">NBitcoin-compatible blockchain network</param>
        /// <returns>A contract service is used to manage electronic money contract in data store.</returns>
        public ContractService(string connectionString, IInsightAPI api, Network network)
        {
            ContractSetup(connectionString, api, network);
        }

        /// <summary>
        /// A private method to setup contract service object.
        /// </summary>
        /// <param name="api">Bitpay Insight compatible API (>0.4)</param>
        /// <param name="connectionString">LiteDB connection string</param>
        /// <param name="network">NBitcoin-compatible blockchain network</param>
        /// <returns>A contract service is used to manage electronic money contract in data store.</returns>
        private void ContractSetup(string connectionString, IInsightAPI api, Network network)
        {
            insightAPI = api;
            MainNetwork = network;
            db = new LiteDatabase(connectionString);
            collection = db.GetCollection<WalletContract>();
            DomainHex = BitConverter.ToString(Encoding.Default.GetBytes(Domain)).Replace("-", "");
            DomainHex = DomainHex.PadLeft(32, '0').ToLower();
        }


        /// <summary>
        /// A builder of wallet service according to the given wallet contract.
        /// </summary>
        /// <param name="constract">Electronic money contract object</param>
        /// <param name="privateKey">The ledgers on this wallet service will be on behalf of the private key</param>
        /// <returns>A wallet service that manage the ledgers in the given wallet contract.</returns>
        public WalletService CreateWalletService(WalletContract constract, BitcoinSecret privateKey)
        {
            if (constract == null)
            {
                throw new ArgumentNullException("constract cannot be null.");
            }
            return new WalletService(constract, this, db, privateKey, insightAPI, 0.01m);
        }

        /// <summary>
        /// Retrieve all the wallet contracts from the local data store.
        /// </summary>
        /// <returns>A list of wallet contracts.</returns>
        public List<WalletContract> FindLocalContract()
        {
            return collection.FindAll().ToList();
        }

        /// <summary>
        /// Retrieve the wallet contract according to transaction ID from the local data store. If not found, retrieve through the blockchain API.
        /// </summary>
        /// <param name="txid">Bitcoin-compatible transaction ID</param>
        /// <returns>Return a wallet contract if found. Return null object if not found.</returns>
        public async Task<WalletContract> FindContract(string txid)
        {
            WalletContract existing = null;
            existing = collection.Find(c => c.ID == txid).FirstOrDefault();

            if (existing != null)
            {
                return existing;
            }
            else
            {
                try
                {
                    var transaction = await insightAPI.GetTransactionInfo(txid);
                    var detectedWallet = new WalletContract();
                    detectedWallet.ID = txid;
                    detectedWallet.OwnerPublicAddress = transaction.GetOwnerAddress();
                    var op_return = transaction.GetOP_RETURN();
                    if (!op_return.StartsWith(DomainHex))
                    {
                        return null;
                    }
                    var startBit = 32;
                    detectedWallet.NameHex = op_return.Substring(startBit, 16);
                    startBit += 16;
                    detectedWallet.TokenHex = op_return.Substring(startBit, 16);
                    startBit += 16;
                    detectedWallet.TotalSupply = BitConverterExtension.ToDecimal(op_return.Substring(startBit, 32));
                    startBit += 32;
                    detectedWallet.NoOfDecimal = ushort.Parse(op_return.Substring(startBit, 4), System.Globalization.NumberStyles.HexNumber);
                    startBit += 4;
                    detectedWallet.Conditions = op_return.Substring(startBit, 16).StringToByteArray();
                    startBit += 16;
                    detectedWallet.StartingBlock = transaction.blockheight;
                    detectedWallet.LastSyncedBlock = transaction.blockheight;
                    byte[] myByte = Encoding.ASCII.GetBytes(detectedWallet.ID);
                    var encrypted = NBitcoin.Crypto.Hashes.RIPEMD160(myByte, myByte.Length);
                    var hashID = encrypted.ByteArrayToString();
                    db.DropCollection($"Ledger-{hashID}");
                    db.DropCollection($"Account-{hashID}");
                    UpdateContract(detectedWallet, true);
                    return detectedWallet;
                }
                catch (Exception)
                {
                    return null;
                }

            }
        }

        public WalletContract FindLocalContract(byte[] Name)
        {
            return collection.FindOne(c => c.Name == Name);
        }

        /// <summary>
        /// Retrieve wallet contract from the local data store by hex-string.
        /// </summary>
        /// <param name="NameHex">A contract name (hex-string)</param>
        /// <returns>Return a wallet contract if found. Return null object if not found.</returns>
        public WalletContract FindLocalContract(string NameHex)
        {
            var Name = Encoding.ASCII.GetBytes(NameHex);
            return FindLocalContract(Name);
        }

        /// <summary>
        /// Update or Insert wallet contract object to local data store.
        /// </summary>
        /// <param name="contract">A wallet contract object</param>
        /// <param name="reIndex">Re-index after insertion or updation. (Optional, default is false)</param>
        /// <returns>None.</returns>
        public void UpdateContract(WalletContract contract, bool reIndex = false)
        {
            collection.Upsert(contract);
            if (reIndex)
            {
                collection.EnsureIndex(c => c.ID);
                collection.EnsureIndex(c => c.Name);
                collection.EnsureIndex(c => c.OwnerPublicAddress);
            }
        }
        /// <summary>
        /// Retrieve the wallet contract according to transaction ID only from the local data store.
        /// </summary>
        /// <param name="txid">Bitcoin-compatible transaction ID</param>
        /// <returns>Return a wallet contract if found. Return null object if not found.</returns> 
        public WalletContract LoadContract(string txid)
        {
            return collection.FindOne(c => c.ID == txid);
        }

        /// <summary>
        /// Create and broadcast a Electronic money wallet contract to blockchain.
        /// </summary>
        /// <param name="creator">Private key of the creator of wallet contract</param>
        /// <param name="Name">Maximum 8 bytes of binary value of token name </param>
        /// <param name="TokenName">Maximum 8 bytes of binary value of token unit</param>
        /// <param name="TotalSupply">Maximum supply of electronic money</param>
        /// <param name="NoOfDecimal">Maximum number of digit after decimal point. Dust value than this will be cut off</param>
        /// <param name="autopublish">Auto publish the contract to blockchain</param>
        /// <returns>A local-store of wallet contract. Throw exception if there is invalid provided value.</returns>
        public async Task<WalletContract> CreateContract(BitcoinSecret creator, byte[] Name, byte[] TokenName, decimal TotalSupply, ushort NoOfDecimal, bool autopublish = true)
        {
            var newContract = new WalletContract()
            {
                Name = Name,
                TokenName = TokenName,
                TotalSupply = TotalSupply,
                NoOfDecimal = NoOfDecimal,
                OwnerPublicAddress = creator.GetAddress().ToString(),
                LastSyncedBlock = 0,
                Conditions = new byte[8]
            };

            try
            {
                ValidateContract(newContract);
            }
            catch (Exception)
            {
                throw;
            }

            if (autopublish)
            {
                await PublishContract(newContract, creator);
            }
            return newContract;

        }

        /// <summary>
        /// Validate wallet contract.
        /// </summary>
        /// <param name="newContract"></param>
        /// <returns>Nothing if valid. But throw exception with reasons if invalid.</returns>
        private void ValidateContract(WalletContract newContract)
        {
            if (newContract.Name.Length < 8)
            {
                var emptyName = new byte[8 - newContract.Name.Length];
                for (int i = 0; i < emptyName.Length; i++)
                {
                    emptyName[i] = 0;
                }
                newContract.Name = emptyName.Concat(newContract.Name).ToArray();
            }
            else if (newContract.Name.Length > 8)
            {
                throw new OverflowException("Contract Name can't be greater than 8 bytes.");
            }

            if (newContract.TokenName.Length > 8)
            {
                throw new OverflowException("Token Name can't be greater than 8 bytes.");
            }
            else if (newContract.TokenName.Length < 8)
            {
                var emptyName = new byte[8 - newContract.TokenName.Length];
                for (int i = 0; i < emptyName.Length; i++)
                {
                    emptyName[i] = 0;
                }
                newContract.TokenName = emptyName.Concat(newContract.TokenName).ToArray();
            }

            if (newContract.TotalSupply < 0)
            {
                throw new OverflowException("Total supply must be positive value.");
            }
        }


        /// <summary>
        /// Publish a wallet contract to blockchain and save the published to local data store.
        /// </summary>
        /// <param name="newContract">A to-be-published wallet contract.</param>
        /// <param name="creator">Private key of the creator</param>
        /// <returns>Return transaction ID if published. Throw exception if there is an error.</returns>
        public async Task<string> PublishContract(WalletContract newContract, BitcoinSecret creator)
        {
            try
            {
                ValidateContract(newContract);
            }
            catch (Exception)
            {
                throw;
            }

            var Data = $"{DomainHex}{newContract.NameHex}{newContract.TokenHex}{BitConverterExtension.GetHexBytes(newContract.TotalSupply)}" +
                       $"{newContract.NoOfDecimal.ToString("x4")}{newContract.Conditions.ByteArrayToString()}";

            var transaction = await insightAPI.BuildTransaction(creator, creator.GetAddress(), creator.GetAddress(), Data.ToLower());
            var txId = await insightAPI.BroadcastTransaction(transaction);
            newContract.ID = txId.txid;
            var status = await insightAPI.GetSync();
            newContract.StartingBlock = status.blockChainHeight;
            newContract.LastSyncedBlock = status.blockChainHeight;
            UpdateContract(newContract, true);
            return newContract.ID;
        }

    }
}
