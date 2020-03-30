using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Thaismartcontract.WalletService.Extension
{
    public static class BitArrayExtension
    {
        public static string ToHexString(this BitArray array)
        {
            StringBuilder sb = new StringBuilder(array.Length / 4);

            for (int i = 0; i < array.Length; i += 4)
            {
                int v = (array[i] ? 8 : 0) |
                        (array[i + 1] ? 4 : 0) |
                        (array[i + 2] ? 2 : 0) |
                        (array[i + 3] ? 1 : 0);

                sb.Append(v.ToString("x1")); // Or "X1"
            }

            string result = sb.ToString();
            return result;
        }

        public static BitArray ToBitArray(this string hexData)
        {
            var int64 = Int64.Parse(hexData, NumberStyles.HexNumber);
            var bytes = BitConverter.GetBytes(int64);
            var bitArray = new BitArray(bytes);
            return bitArray;
        }
    }
}
