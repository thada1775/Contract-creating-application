using System;
using System.Text;

namespace Thaismartcontract.DocumentService
{
    public class DocumentService
    {
        public string Domain { get; private set; }   // Domain Name 21 bytes


        public DocumentService() : this("RMUTSB.AC.TH")
        {

        }

        public DocumentService(string domain)
        {
            if (domain.Length > 20)
            {
                throw new IndexOutOfRangeException("Too long domain (20 characters is the limit)");
            }

            foreach (var c in domain)
            {
                if (!((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9')))
                {
                    throw new FormatException("Domain contains invalid characters.");
                }
            }
            Domain = domain;
        }

        public string GetOPReturn(long signId, string accountId, byte role, byte[] documentHash)
        {
            var roleHexString = role.ToString();
            var documentHashHexString = BitConverter.ToString(documentHash).Replace("-", "").ToLower();
            var headHexString = BitConverter.ToString(Encoding.ASCII.GetBytes(Domain)).Replace("-", "").ToLower();
            var data = $"{headHexString}{signId.ToString("x16")}{accountId}{roleHexString}{documentHashHexString}";  // Total : 78 bytes
            return data;
        }

        public byte[] GetDigitalSignature(byte[] source, Algorithm algorithm)
        {

            return null;
        }

    }

    public enum Algorithm
    {
        SHA256,
        SHA3,
        Keccak
    }

    public enum SignRole
    {
        Signer = 0,
        Witness = 1
    }

}
