using NBitcoin;
using NBitcoin.DataEncoders;

namespace Thaismartcontract.API.Model
{
    public class UTXO
    {
        public string address { get; set; }
        public string txid { get; set; }
        public int vout { get; set; }
        public string scriptPubKey { get; set; }
        public decimal amount { get; set; }

        public ICoin AsCoin()
        {
            var newCoin = new Coin(
                new OutPoint(uint256.Parse(txid), (uint)vout),
                new TxOut(new Money((long)(amount * Money.COIN)), new Script(Encoders.Hex.DecodeData(scriptPubKey))));
            return newCoin;
        }
    }

}
