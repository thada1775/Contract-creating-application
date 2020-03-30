using NBitcoin;

namespace Thaismartcontract.API
{
    public class DigibyteKeyGenerator : IKeyGenerator
    {
        private Network _network = NBitcoin.Altcoins.Digibyte.Instance.Mainnet;
        public CryptoKeyPair GenerateKeyPair()
        {
            var output = new CryptoKeyPair();
            output.SecretKey = new Key().GetBitcoinSecret(_network);
            output.PublicKey = output.SecretKey.GetAddress();
            return output;
        }

        public CryptoKeyPair GenerateKeyPairFromSeed()
        {
            var hdGenerator = new DigibyteHDGenerator(_network, "Thai");
            var newkeyPair = hdGenerator.KeyPair(0);
            newkeyPair.Seed = hdGenerator.Mnemonic.ToString();
            return newkeyPair;
        }

        public CryptoKeyPair GenerateKeyPairFromSeed(string seed)
        {
            try
            {
                var hdGenerator = new DigibyteHDGenerator(_network, "Thai", seed);
                var newkeyPair = hdGenerator.KeyPair(0);
                newkeyPair.Seed = hdGenerator.Mnemonic.ToString();
                return newkeyPair;
            }
            catch (System.Exception)
            {
                return null;
            }
          
        }
    }
}
