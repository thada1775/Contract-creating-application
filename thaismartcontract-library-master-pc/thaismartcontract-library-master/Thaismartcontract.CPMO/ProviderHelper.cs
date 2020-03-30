using NBitcoin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Thaismartcontract.API;
using Thaismartcontract.API.Model;
using Thaismartcontract.CPMO.Extension;
using Thaismartcontract.Storage;
using Thaismartcontract.Storage.GDrive;

namespace Thaismartcontract.CPMO
{
    public class ProviderHelper : Helper
    {
        private readonly IStorageManager storageManager;

        /// <summary>
        /// A constructor of helper class for provider part which handle document management and consent ask.
        /// </summary>
        /// <param name="privateKey">User's private key</param>
        /// <param name="network">NBitcoin-compatible blockchain network</param>
        /// <param name="builder">Option builder for IStorageManager</param>
        public ProviderHelper(string privateKey, IOptionBuilder builder, Network network = null) : base(privateKey, network)
        {          
            storageManager = new GDriveStorageManager();
            storageManager.SetBuilder(builder);
        }

        /// <summary>
        /// A constructor of helper class for provider part which handle document management and consent ask.
        /// </summary>
        /// <param name="privateKey">User's private key</param>
        /// <param name="network">NBitcoin-compatible blockchain network</param>
        /// <param name="builder">Option builder for IStorageManager</param>
        /// <param name="api">Insight-based block explorer (>0.4)</param>
        public ProviderHelper(string privateKey, IInsightAPI api, IOptionBuilder builder, Network network = null) : base(privateKey, network, api)
        {
            storageManager = new GDriveStorageManager();
            storageManager.SetBuilder(builder);           
        }

        /// <summary>
        /// Upload stream object to the cloud.
        /// </summary>
        /// <param name="stream">Stream object contains contract file to upload</param>
        /// <param name="filename">Constract filename</param>
        /// <returns>A FileStorage object contains file's information on the cloud.</returns>
        public Task<FileStorage> UploadFile(Stream stream, string filename) => storageManager.UploadAsync(stream, filename);

        /// <summary>
        /// Upload stream object to the cloud.
        /// </summary>
        /// <param name="file">Byte array object contains contract file to upload</param>
        /// <param name="filename">Constract filename</param>
        /// <returns>A FileStorage object contains file's information on the cloud.</returns>
        public Task<FileStorage> UploadFile(byte[] file, string filename) => storageManager.UploadAsync(file, filename);

        /// <summary>
        /// Retrieve file from the cloud based on Keccak-256 hash.
        /// </summary>
        /// <param name="hash">Keccak-256 hash value of the file</param>
        /// <returns>A FileStorage object contains file's information on the cloud.</returns>
        public Task<FileStorage> GetFileStorageByHash(string hash) => storageManager.GetInfo(hash, "hash");

        /// <summary>
        /// Retrieve file from the cloud based on file ID
        /// </summary>
        /// <param name="id">FileID provided by cloud service</param>
        /// <returns>A FileStorage object contains file's information on the cloud.</returns>
        public Task<FileStorage> GetFileById(string id) => storageManager.GetInfo(id, "fileid");

        /// <summary>
        /// Retrieve download link of the file on the cloud.
        /// </summary>
        /// <param name="fileStorage">A FileStorage object contains file's information on the cloud</param>
        /// <returns>Download link of the document.</returns>
        public Task<string> GetDownloadLink(FileStorage fileStorage) => storageManager.DownloadLink(fileStorage.FileId, "fileid");

        /// <summary>
        /// Retrieve binary data of the file on the cloud.
        /// </summary>
        /// <param name="fileStorage">A FileStorage object contains file's information on the cloud</param>
        /// <returns>A byte array represent binary data of the file.</returns>
        public Task<byte[]> GetFile(FileStorage fileStorage) => storageManager.DownloadAsync(fileStorage.FileId, "fileid");

        /// <summary>
        /// Create consent to the client according to the provided contract.
        /// </summary>
        /// <param name="contract">A FileStorage object represents the binding contract</param>
        /// <param name="userAddress">User's public key</param>
        /// <returns>Signed NBitcoin-compatible transaction. Use ToHex() in case of manual broadcast.</returns>
        public Task<Transaction> CreateConsent(FileStorage contract, string userAddress)
        {
            var domain = "CPMO-TH:ASK:".AsStringToHexString();
            var hash = contract.Hash;
            // domain:12 hash:32  ==> 44 bytes
            var data = $"{domain}{hash}";
            var userPubKeyAddress = new BitcoinPubKeyAddress(userAddress, MainNetwork);
            return api.BuildTransactionQuestion(privateKey, userPubKeyAddress, data);
        }
        
    }
}
