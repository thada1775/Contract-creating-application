using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;
using Thaismartcontract.WalletService.Extension;

namespace Thaismartcontract.WalletService.Model
{
    public class Account
    {
        [BsonId]
        public byte[] WitnessProgram { get; set; }
        [BsonIgnore]
        public string WitnessProgramHex
        {
            get
            {
                return WitnessProgram.ByteArrayToString();
            }
            set
            {
                WitnessProgram = value.StringToByteArray();
            }
        }

        public decimal Balance { get; set; }
        public int Blockheight { get; set; }

        [BsonIgnore]
        public string TokenName { get; set; }
        [BsonIgnore]
        public string TokenUnit { get; set; }

        public override string ToString()
        {
            return $"{WitnessProgramHex} has {TokenName} {Balance} {TokenUnit}";
        }

        [BsonIgnore]
        public AccountService AccountService { get; set; }
    }
}
