using System;
using System.Collections.Generic;
using System.Text;
using NBitcoin;

namespace Thaismartcontract.API.Extension
{
    public static class BitcoinPubKeyAddressExtension
    {
        public static byte[] ExtractWitnessProgram(this BitcoinPubKeyAddress pubkey)
        {
            return pubkey.Hash.ToBytes();
        }
    }
}
