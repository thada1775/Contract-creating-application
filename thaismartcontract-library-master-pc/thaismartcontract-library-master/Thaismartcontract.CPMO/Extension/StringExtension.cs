using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thaismartcontract.CPMO.Extension
{
    public static class StringExtension
    {
        public static string AsStringToHexString(this string data)
        {
            return BitConverter.ToString(Encoding.ASCII.GetBytes(data)).Replace("-", "");
        }

        public static string AsHexStringToString(this string data)
        {
            byte[] ba = FromHex(data);
            return Encoding.ASCII.GetString(ba);
        }

        private static byte[] FromHex(string hex)
        {
            hex = hex.Replace("-", "");
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return raw;
        }
    }
}
