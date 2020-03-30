using LiteDB;
using System.Collections;
using System.Linq;
using System.Text;
using Thaismartcontract.WalletService.Extension;

namespace Thaismartcontract.WalletService.Model
{
    public class WalletContract
    {
        [BsonId]
        public string ID { get; set; }
        public byte[] Name { get; set; }
        [BsonIgnore]
        public string NameHex
        {
            get
            {
                return Name.ByteArrayToString();
            }
            set
            {
                Name = value.StringToByteArray();
            }
        }
        [BsonIgnore]
        public string NameString
        {
            get
            {
                return Encoding.Default.GetString(Name.SkipWhile(t => t == 0).ToArray());
            }
        }

        public byte[] TokenName { get; set; }
        [BsonIgnore]
        public string TokenHex
        {
            get
            {
                return TokenName.ByteArrayToString();
            }
            set
            {
                TokenName = value.StringToByteArray();
            }
        }
        [BsonIgnore]
        public string TokenString
        {
            get
            {
                return Encoding.Default.GetString(TokenName.SkipWhile(t => t == 0).ToArray());
            }
        }
        public decimal TotalSupply { get; set; }
        public ushort NoOfDecimal { get; set; }

        public byte[] Conditions { get; set; }
        
        public string OwnerPublicAddress { get; set; }
        public string ReferenceContractID { get; set; }
        public string[] ExtensionTx { get; set; }


        public int StartingBlock { get; set; }
        public int LastSyncedBlock { get; set; }
        public string Jsonconvert { get; private set; }

        public override string ToString()
        {
            return $"{ID}:{NameHex}:{TokenHex}:{TotalSupply}:{NoOfDecimal}";
        }
    }
}
