using NBitcoin;
using System.Text.RegularExpressions;

namespace Thaismartcontract.API
{
    public class DigibyteHDGenerator : IHDGenerator
    {
        private readonly Network network;
        public Mnemonic Mnemonic { get; private set; }
        public int[] Indices { get; private set; }
        private ExtKey _masterKey;
        private ExtPubKey _masterPubKey;
        private readonly KeyPath _keypath = new KeyPath("m/44'/20'/0'/0");
        public const bool IsHardened = true;
        private ExtKey _bip44privateKey;

        public DigibyteHDGenerator(Network network, string language = "English", string mnemonic = null)
        {
            this.network = network;
            Wordlist wordlist;
            if (language == "Thai")
            {
                wordlist = ThaiWordList.Thai;
            } else
            {
                wordlist = Wordlist.English;
            }
            
            if (string.IsNullOrEmpty(mnemonic))
            {
                Mnemonic = new Mnemonic(wordlist, WordCount.Fifteen);
            }
            else
            {
                RegexOptions options = RegexOptions.None;
                Regex regex = new Regex("[ ]{2,}", options);
                mnemonic = regex.Replace(mnemonic.Trim(), " ");
                Mnemonic = new Mnemonic(mnemonic, wordlist);
            }
            Indices = wordlist.ToIndices(Mnemonic.Words);
            Initialize();
        }

        private void Initialize()
        {
            _masterKey = Mnemonic.DeriveExtKey();
            _masterPubKey = _masterKey.Neuter();
            _bip44privateKey = _masterKey.Derive(_keypath);
        }

        public string GetMasterKey()
        {
            return _masterKey.ToString(network);
        }

        public string GetMasterPublicKey()
        {
            return _masterPubKey.ToString(network);
        }

        public BitcoinExtKey GetExtenedPrivateKey()
        {
            return _bip44privateKey.GetWif(network);
        }

        public BitcoinExtPubKey GetExtenedPublicKey()
        {
            return _bip44privateKey.Neuter().GetWif(network);
        }

        public CryptoKeyPair KeyPair(uint index)
        {
            var privateKey = _bip44privateKey.Derive(index).PrivateKey.GetBitcoinSecret(network);
            var publicKey = privateKey.GetAddress();
            return new CryptoKeyPair()
            {
                Seed = Mnemonic.ToString(),
                SecretKey = privateKey,
                PublicKey = publicKey
            };
        }
    }
}
