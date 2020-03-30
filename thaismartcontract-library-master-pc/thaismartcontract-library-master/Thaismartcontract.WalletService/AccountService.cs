using LiteDB;
using NBitcoin;
using System.Text;
using Thaismartcontract.API.Extension;
using Thaismartcontract.WalletService.Extension;
using Thaismartcontract.WalletService.Model;

namespace Thaismartcontract.WalletService
{
    public class AccountService
    {
        private readonly LiteCollection<Account> account;
        private readonly WalletContract contract;
        private readonly ContractService contractService;
        private readonly LiteDatabase db;
        private readonly WalletService walletService;

        /// <summary>
        /// A default account service constructor from a given a wallet service.
        /// </summary>
        /// <param name="contractService">A service object that manage contract.</param>
        /// <param name="contract">Current electronic money contract</param>
        /// <param name="walletService">A service object that manage ledgers</param>
        /// <param name="db">A LiteDB datastore</param>
        /// <returns>AccountService object is used to process individual account operation such as balance in one private key and one contract.</returns>
        internal AccountService(ContractService contractService, WalletContract contract, WalletService walletService, LiteDatabase db)
        {
            this.contractService = contractService;
            this.contract = contract;
            this.walletService = walletService;
            this.db = db;
            byte[] myByte = Encoding.ASCII.GetBytes(contract.ID);
            var encrypted = NBitcoin.Crypto.Hashes.RIPEMD160(myByte, myByte.Length);
            var hashID = encrypted.ByteArrayToString();
            account = db.GetCollection<Account>($"Account-{hashID}");
        }

        /// <summary>
        /// Obtain the account according to the private key.
        /// </summary>
        /// <param name="privateKey">A Bitcoin-compatible private key</param>
        /// <returns>An account according to the given key.</returns>
        public Account GetAccount(BitcoinSecret privateKey)
        {
            return GetAccount(privateKey.GetAddress());
        }

        /// <summary>
        /// Obtain the account according to the public key.
        /// </summary>
        /// <param name="pubKeyAddress">A Bitcoin-compatible public key (string)</param>
        /// <returns>An account according to the given key.</returns>
        public Account GetAccount(string pubKeyAddress)
        {
            return GetAccount(new BitcoinPubKeyAddress(pubKeyAddress, contractService.MainNetwork));
        }

        /// <summary>
        /// Obtain the account according to the public key.
        /// </summary>
        /// <param name="pubKeyAddress">A Bitcoin-compatible public key</param>
        /// <returns>An account according to the given key.</returns>
        public Account GetAccount(BitcoinPubKeyAddress pubKeyAddress)
        {
            var witnessProgram = pubKeyAddress.ExtractWitnessProgram();
            return GetAccount(witnessProgram);
        }

        /// <summary>
        /// Obtain the account according to hash of the public key. This is used when public key is unknown.
        /// </summary>
        /// <param name="pubKeyAddress">RIPEMP160 hash of the public key</param>
        /// <returns>An account according to the given hash.</returns>
        public Account GetAccount(byte[] witnessProgram)
        {
            var myAccount = account.FindOne(a => a.WitnessProgram == witnessProgram);
            if (myAccount != null)
            {
                myAccount.TokenName = contract.NameString;
                myAccount.TokenUnit = contract.TokenString;
            } else
            {
                myAccount = new Account()
                {
                    Balance = 0,
                    Blockheight = 0,
                    TokenName = contract.NameString,
                    TokenUnit = contract.TokenString,
                    WitnessProgram = witnessProgram
                };
            }
            myAccount.AccountService = this;
            return myAccount;
        }

        /// <summary>
        /// Get contract service object that creates this account service.
        /// </summary>
        /// <returns>A service object that manage contract.</returns>
        public ContractService GetContractService()
        {
            return contractService;
        }

        /// <summary>
        /// Get the wallet contract object that owns this account service.
        /// </summary>
        /// <returns>An object that represents wallet contract.</returns>
        public WalletContract GetWalletContract()
        {
            return contract;
        }

        /// <summary>
        /// Get wall service object that creates this account service.
        /// </summary>
        /// <returns>A service object that manage wallet.</returns>
        public WalletService GetWalletService()
        {
            return walletService;
        }
    }
}