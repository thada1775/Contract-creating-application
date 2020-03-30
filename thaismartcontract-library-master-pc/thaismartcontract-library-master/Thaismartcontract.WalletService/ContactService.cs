using LiteDB;
using NBitcoin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Thaismartcontract.WalletService.Model;

namespace Thaismartcontract.WalletService
{
    public class ContactService
    {
        private LiteDatabase db;
        public Network MainNetwork { get; private set; }
        private LiteCollection<Contact> collection;

        /// <summary>
        /// A default constructor of the user's contact.
        /// </summary>
        /// <returns>A contact service is used to manage the user's contact.</returns>
        public ContactService()
        {
            //var connectionString = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "contact.db");
            var connectionString = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}contact.db";
            var network = NBitcoin.Altcoins.Digibyte.Instance.Mainnet;
            ContactSetup(connectionString, network);
        }

        /// <summary>
        /// Constructor of the user's contact.
        /// </summary>
        /// <param name="connectionString">LiteDB connection string.</param>
        /// <returns>A contact service is used to manage the user's contact.</returns>
        public ContactService(string connectionString)
        {
            var network = NBitcoin.Altcoins.Digibyte.Instance.Mainnet;
            ContactSetup(connectionString, network);
        }

        /// <summary>
        /// Constructor of the user's contact.
        /// </summary>
        /// <param name="connectionString">LiteDB connection string</param>
        /// <param name="network">NBitcion-compatible blockchain network</param>
        /// <returns>A contact service is used to manage the user's contact.</returns>
        public ContactService(string connectionString, Network network)
        {
            ContactSetup(connectionString, network);
        }

        /// <summary>
        /// Private method to build the ContactService from the constructor.
        /// </summary>
        /// <param name="connectionString">LiteDB connection string</param>
        /// <param name="network">NBitcion-compatible blockchain network</param>
        private void ContactSetup(string connectionString, Network network)
        {
            MainNetwork = network;
            db = new LiteDatabase(connectionString);
            collection = db.GetCollection<Contact>();
        }

        /// <summary>
        /// Insert or Update the user's contact. Public key is a primary key.
        /// </summary>
        /// <param name="name">Person's name or reference</param>
        /// <param name="publicKey">Valid Bitcoin-compatible Publickey (string)</param>
        /// <returns>An inserted or updated contact record.</returns>
        public Contact UpsertContact(string name, string publicKey)
        {
            try
            {
                var mycontact = collection.Find(c => c.Address == publicKey).FirstOrDefault();
                if (mycontact == null)
                {
                    mycontact = new Contact()
                    {
                        Name = name,
                        Address = publicKey,
                        AddressHash = new BitcoinPubKeyAddress(publicKey, MainNetwork).Hash.ToBytes()
                    };
                }
                else
                {
                    mycontact.Name = name;
                }
                collection.Upsert(mycontact);
                collection.EnsureIndex(c => c.Address);
                collection.EnsureIndex(c => c.AddressHash);
                collection.EnsureIndex(c => c.AddressHashString);
                return mycontact;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Delete user's contact by the given public key.
        /// </summary>
        /// <param name="publicKey">Valid Bitcoin-compatible public key (string)</param>
        /// <returns>True if the object is deleted. False if contact is not found.</returns>
        public bool DeleteContact(string publicKey)
        {
            try
            {
                return collection.Delete(publicKey);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Display the most readable information of the contact.
        /// </summary>
        /// <param name="hashpubkey">RIPEMD160 hash of the Bitcoin-compatible public key</param>
        /// <returns>The most readable information. Name --> Public key --> Hash of public key.</returns>
        public string GetContact(string hashpubkey)
        {
            var mycontact = collection.FindOne(c => c.AddressHashString == hashpubkey);
            if (mycontact == null)
            {
                return hashpubkey;
            }
            else if (!string.IsNullOrEmpty(mycontact.Name))
            {
                return mycontact.Name;
            }
            else
            {
                return mycontact.Address;
            }
        }

        /// <summary>
        /// List of the user's contact in the data store.
        /// </summary>
        /// <returns>A list of Contact object.</returns>
        public List<Contact> GetContact()
        {
            return collection.FindAll().ToList();
        }
    }
}
