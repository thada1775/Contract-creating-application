using NBitcoin;
using NBitcoin.DataEncoders;

namespace ConsoleApp1.Model
{
    public class UTXO
    {
        public string address { get; set; }
        public string txid { get; set; }
        public int vout { get; set; }
        public string scriptPubKey { get; set; }
        public decimal amount { get; set; }

        public ICoin ToCoin()
        {

            var newCoin = new Coin(
                new OutPoint(uint256.Parse(txid), (uint)vout),
                new TxOut(new Money((long)(amount * Money.COIN)), new Script(Encoders.Hex.DecodeData(scriptPubKey))));

            return newCoin;
        }
    }

}


//public ICoin ToStealthCoin()
//{
//    var newCoin = new StealthCoin(
//        new OutPoint(uint256.Parse(txid), (uint)vout),
//        new TxOut(new Money((long)(amount * Money.COIN)), new Script(Encoders.Hex.DecodeData(scriptPubKey))),
//        null,
//        new NBitcoin.Stealth.StealthMetadata(new Script(new byte[] { 45, 45, 45, 45 })));
//}