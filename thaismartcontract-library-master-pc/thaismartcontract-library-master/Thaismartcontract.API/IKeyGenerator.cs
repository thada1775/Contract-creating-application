using NBitcoin;

namespace Thaismartcontract.API
{
    public interface IKeyGenerator
    {
        CryptoKeyPair GenerateKeyPair();
        CryptoKeyPair GenerateKeyPairFromSeed();
        CryptoKeyPair GenerateKeyPairFromSeed(string seed);
    }

    public class CryptoKeyPair
    {
        public string Seed { get; set; }
        public BitcoinSecret SecretKey { get; set; }
        public BitcoinPubKeyAddress PublicKey { get; set; }

        public string SecretKeyWif => SecretKey.ToWif();
        public string PublicKeyWif => PublicKey.ToString();
    }
}
