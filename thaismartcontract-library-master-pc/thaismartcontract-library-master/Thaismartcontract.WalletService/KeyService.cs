using LiteDB;
using NBitcoin;
using System.IO;
using System.Linq;
using Thaismartcontract.API;

namespace Thaismartcontract.WalletService
{
    public class KeyService
    {
        private LiteDatabase db;
        public Network MainNetwork { get; private set; }
        private LiteCollection<KeyData> collection;

        /// <summary>
        /// A default constructor of key service. (Password to encrypt the data store is required.)
        /// Data store is automatically created if not exists.
        /// </summary>
        /// <param name="password">Password to encrypt the data store</param>
        /// <returns>A key service is used to manage user's seed and private key.</returns>
        public KeyService(string password)
        {
            //var connectionString = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "key.db");
            var connectionString = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}key.db";
            var network = NBitcoin.Altcoins.Digibyte.Instance.Mainnet;
            KeySetup(connectionString, network, password);
        }

        /// <summary>
        /// A constructor of key service. (Password to encrypt the database is required.)
        /// Data store is automatically created if not exists.
        /// </summary>
        /// <param name="connectionString">LiteDB connection string</param>
        /// <param name="password">Password to encrypt the data store</param>
        /// <returns>A key service is used to manage user's seed and private key.</returns>
        public KeyService(string connectionString, string password)
        {
            var network = NBitcoin.Altcoins.Digibyte.Instance.Mainnet;
            KeySetup(connectionString, network, password);
        }

        /// <summary>
        /// A constructor of key service. (Password to encrypt the database is required.)
        /// Data store is automatically created if not exists.
        /// </summary>
        /// <param name="connectionString">LiteDB connection string</param>
        /// <param name="network">NBitcoin-compatible blockchain network</param>
        /// <param name="password">Password to encrypt the data store</param>
        /// <returns>A key service is used to manage user's seed and private key.</returns>
        public KeyService(string connectionString, Network network, string password)
        {
            KeySetup(connectionString, network, password);
        }

        /// <summary>
        /// Private method to build the settings of key service.
        /// </summary>
        /// <param name="connectionString">LiteDB connection string</param>
        /// <param name="network">NBitcoin-compatible blockchain network</param>
        /// <param name="password">Password to encrypt the data store</param>
        /// <returns>A key service is used to manage user's seed and private key.</returns>
        private void KeySetup(string connectionString, Network network, string password)
        {
            MainNetwork = network;
            var conn = new ConnectionString(connectionString)
            {
                Password = password
            };
            db = new LiteDatabase(conn);
            collection = db.GetCollection<KeyData>();
        }

        /// <summary>
        /// Check if data store contains the user's seed or private key.
        /// </summary>
        /// <returns>True if there is one. Otherwise is false.</returns>
        public bool HasSetupKey()
        {
            return collection.Count() > 0;
        }

        /// <summary>
        /// Generate key pair with seed in Thai language.
        /// </summary>
        /// <returns>A Cryptokeypair object contains seed, private key, and public key. (BIP44)</returns>
        public CryptoKeyPair GenerateKey()
        {
            var hdGenerator = new DigibyteHDGenerator(MainNetwork, "Thai");
            return hdGenerator.KeyPair(0);
        }

        /// <summary>
        /// Retrieve key pair from a local data store.
        /// </summary>
        /// <returns>A Cryptokeypair object contains seed, private key, and public key. (BIP44)</returns>
        public CryptoKeyPair GetKey()
        {
            try
            {
                var data = collection.FindAll().FirstOrDefault();
                CryptoKeyPair result = new CryptoKeyPair();
                if (!string.IsNullOrEmpty(data?.Seed))
                {
                    var hdGenerator = new DigibyteHDGenerator(MainNetwork, "Thai", data.Seed);
                    return hdGenerator.KeyPair(0);
                }
                else if (!string.IsNullOrEmpty(data?.PrivateKey))
                {
                    var secretKey = new BitcoinSecret(data.PrivateKey, MainNetwork);
                    result.SecretKey = secretKey;
                    result.PublicKey = secretKey.GetAddress();
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (System.Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Update or Insert a Cryptokeypair object to local data store.
        /// </summary>
        /// <param name="keyPair">A Cryptokeypair object.</param>
        /// <returns>True if operation is successful. Otherwise returns false.</returns>
        public bool SaveKey(CryptoKeyPair keyPair)
        {
            try
            {
                collection.Delete(c => true);
                var data = new KeyData()
                {
                    Seed = keyPair.Seed,
                    PrivateKey = keyPair.SecretKeyWif,
                    PublicKey = keyPair.PublicKeyWif
                };
                collection.Upsert(data);
                return true;
            }
            catch (System.Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Retrieve a Cryptokeypair information according to the given seed.
        /// </summary>
        /// <param name="seed">15-word seed in Thai language</param>
        /// <returns>A Cryptokeypair object contains seed, private key, and public key. (BIP44)</returns>
        public CryptoKeyPair ParseSeed(string seed)
        {
            if (string.IsNullOrEmpty(seed))
            {
                return null;
            }
            try
            {
                var hdGenerator = new DigibyteHDGenerator(MainNetwork, "Thai", seed);
                return hdGenerator.KeyPair(0);
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieve a Cryptokeypair information according to the given private key.
        /// </summary>
        /// <param name="privateKey">Bitcoin-compatible private key</param>
        /// <returns>A Cryptokeypair object contains private key, and public key. (BIP44)</returns>
        public CryptoKeyPair ParsePrivateKey(string privateKey)
        {
            try
            {
                var secret = new BitcoinSecret(privateKey, MainNetwork);
                return new CryptoKeyPair()
                {
                    Seed = "",
                    SecretKey = secret,
                    PublicKey = secret.GetAddress()
                };
            }
            catch (System.Exception)
            {
                return null;
            }

        }

        /// <summary>
        /// Close LiteDB data store. Necessary when changing password of the data store.
        /// </summary>
        /// <returns>None.</returns>
        public void CloseDB()
        {
            db.Dispose();
        }

        /// <summary>
        /// Delete current LiteDB data store. All data will be destroyed.
        /// </summary>
        /// <returns>Exception is throw if there is an error.</returns>
        public void DeleteDB()
        {
            db.Dispose();
            try
            {
                var keyPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "key.db");
                File.Delete(keyPath);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }

    public class KeyData
    {
        public string Seed { get; set; }
        [BsonId]
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
    }
}
