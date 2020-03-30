using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Thaismartcontract.Storage.GDrive;

namespace Thaismartcontract.Storage
{
    public interface IStorageManager
    {
        Task SetBuilder(IOptionBuilder builder);
        Task<FileStorage> UploadAsync(byte[] file, string filename);
        Task<FileStorage> UploadAsync(Stream file, string filename);
        Task<byte[]> DownloadAsync(string id, string type = "fileid");
        Task<int> DeleteAsync(string fileId, string filename);
        Task<string> DownloadLink(string fileId, string type = "fileid");
        Task<List<string>> ListFileAsync();
        Task<string> ProvisionDirectory();
        Task<FileStorage> GetInfo(string id, string type = "fileid");
    }
}
