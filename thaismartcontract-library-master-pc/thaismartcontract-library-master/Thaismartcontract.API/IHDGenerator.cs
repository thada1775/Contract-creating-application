using NBitcoin;
using System;
using System.Collections.Generic;
using System.Text;

namespace Thaismartcontract.API
{
    public interface IHDGenerator
    {
        string GetMasterKey();
        string GetMasterPublicKey();
        BitcoinExtKey GetExtenedPrivateKey();
        BitcoinExtPubKey GetExtenedPublicKey();
        CryptoKeyPair KeyPair(uint index);
    }
}
