using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Org.BouncyCastle.Crypto.Digests;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Thaismartcontract.Storage.GDrive
{
    public class GDriveStorageManager : IStorageManager
    {
        static string[] Scopes = { DriveService.Scope.Drive };
        public DriveService service { get; private set; }
        private Permission allAccess;
        private GoogleCredential _credential;
        private IOptionBuilder _builder;
        private string folderID;

        public async Task SetBuilder(IOptionBuilder builder)
        {
            _builder = builder;
            if (string.IsNullOrEmpty(_builder.Option.FolderID) && string.IsNullOrEmpty(_builder.Option.FolderName))
            {
                throw new ArgumentNullException("Either folderID or folder name is not supplied.");
            }

            if (!string.IsNullOrEmpty(_builder.Option.FolderID) && !string.IsNullOrEmpty(_builder.Option.FolderName))
            {
                throw new ArgumentNullException("FolderID and folder name cannot be supplied at the same time.");
            }

            _credential = GoogleCredential.FromJson(_builder.Option.CredentialString).CreateScoped(Scopes);
            service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = _credential
            });
            if (!string.IsNullOrEmpty(_builder.Option.Domain))
            {
                allAccess = new Permission()
                {
                    Type = "domain",
                    Domain = _builder.Option.Domain,
                    Role = "reader"
                };
            }
            else
            {
                allAccess = new Permission()
                {
                    Type = "anyone",
                    Role = "reader"
                };
            }
            folderID = await ProvisionDirectory();
        }

        private byte[] CalculateHash(byte[] data)
        {
            var cloner = new byte[data.Length];
            Array.Copy(data, cloner, data.Length);
            var hasher = new KeccakDigest(256);
            hasher.BlockUpdate(cloner, 0, cloner.Length);
            var output = new byte[hasher.GetDigestSize()];
            hasher.DoFinal(output, 0);
            return output;
        }

        public async Task<int> DeleteAsync(string fileId, string filename)
        {
            try
            {
                await service.Files.Delete(fileId).ExecuteAsync();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<string> DownloadLink(string fileId, string type = "fileid")
        {
            if (type == "fileid")
            {
                return $"https://drive.google.com/file/d/{fileId}/preview";
                // DownloadLink
                //return $"https://drive.google.com/uc?id={fileId}&export=download";
            }
            else
            {
                var listRequest = service.Files.List();
                listRequest.Q = $"'{folderID}' in parents" + " and appProperties has { key = 'hash' and value='" + fileId + "' }";
                listRequest.PageSize = 1000;
                listRequest.Fields = "nextPageToken, files(id, name, size)";
                var fileList = await listRequest.ExecuteAsync();
                if (fileList.Files.Count > 0)
                {
                    var matchedFileId = fileList.Files[0].Id;
                    return $"https://drive.google.com/file/d/{matchedFileId}/preview";
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public async Task<FileStorage> UploadAsync(byte[] file, string filename)
        {
            string extension;
            if (filename.Contains("."))
            {
                extension = filename.Split('.').Last();
            }
            else
            {
                extension = "xxx";
            }

            #region Debugging
            // For create new folder
            //var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            //{
            //    Name = "ScienceDocument",
            //    MimeType = "application/vnd.google-apps.folder"
            //};
            //var request = _service.Files.Create(fileMetadata);
            //request.Fields = "id";
            //var newfolder = request.Execute();
            //Console.WriteLine("Folder ID: " + newfolder.Id);
            //var newfolderID = newfolder.Id;
            //var myPerm = new Permission()
            //{
            //    Type = "user",
            //    Role = "writer",
            //    EmailAddress = "utharn.b@rmutsb.ac.th"
            //};
            //var newfolderID = "1-oS2zC5YYA75E1_RJR5zQLgiT6hyiMdt";
            //_service.Permissions.Create(myPerm, newfolderID).Execute();
            //

            //Folder List
            //FilesResource.ListRequest listRequest = _service.Files.List();
            //listRequest.Q = $"trashed=false and parents='{folderID}'";
            ////listRequest.Q = $"trashed=false and mimeType='application/vnd.google-apps.folder'";
            //listRequest.PageSize = 10;
            //listRequest.Fields = "nextPageToken, files";
            //IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files;

            //foreach (var f in files)
            //{
            //    await DeleteAsync(f.Id, f.Name);
            //}

            #endregion
            try
            {
                var hash = BitConverter.ToString(CalculateHash(file)).Replace("-", "").ToLower();

                var listRequest = service.Files.List();
                listRequest.Q = $"'{folderID}' in parents" + " and appProperties has { key = 'hash' and value='" + hash + "' }";
                listRequest.Fields = "nextPageToken, files(id, name, size)";
                var fileList = await listRequest.ExecuteAsync();
                if (fileList.Files.Count > 0)
                    return new FileStorage()
                    {
                        FileId = fileList.Files[0].Id,
                        FileName = fileList.Files[0].Name,
                        FileType = extension,
                        Description = filename,
                        Size = fileList.Files[0].Size.GetValueOrDefault(),
                        Hash = hash,
                        Plan = "google"
                    };

                var metaData = new Dictionary<string, string>();
                metaData.Add("hash", hash);
                using (MemoryStream s1 = new MemoryStream(file))
                {
                    var uuidName = Guid.NewGuid().ToString() + "." + extension;
                    var uploadRequest = service.Files.Create(new Google.Apis.Drive.v3.Data.File()
                    {
                        Name = uuidName,
                        Parents = new List<string> { folderID },
                        CopyRequiresWriterPermission = true,
                        AppProperties = metaData,
                        Description = filename
                    }, s1, MimeTypes.GetMimeType(uuidName));

                    uploadRequest.Fields = "id,name,size,mimeType,createdTime,webContentLink,webViewLink,size,mimeType,modifiedTime,description,appProperties";
                    await uploadRequest.UploadAsync();

                    var uploadedFile = uploadRequest.ResponseBody;
                    await service.Permissions.Create(allAccess, uploadedFile.Id).ExecuteAsync();

                    return new FileStorage()
                    {
                        FileId = uploadedFile.Id,
                        FileName = uploadedFile.Name,
                        Description = uploadedFile.Description,
                        FileType = extension,
                        Size = file.Length,
                        Hash = uploadedFile.AppProperties["hash"],
                        Plan = "google",
                    };
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                if (e.InnerException != null)
                {
                    Debug.WriteLine(e.InnerException.Message);
                }
                throw;
            }

        }

        public async Task<byte[]> DownloadAsync(string id, string type = "fileid")
        {
            using (var stream = new MemoryStream())
            {

                if (type == "fileid")
                {
                    var request = service.Files.Get(id);
                    await request.DownloadAsync(stream);
                    return stream.ToArray();
                }
                else if (type == "hash")
                {
                    var listRequest = service.Files.List();
                    listRequest.Q = $"'{folderID}' in parents" + " and appProperties has { key = 'hash' and value='" + id + "' }";
                    listRequest.Fields = "nextPageToken, files(id, name, size)";
                    var fileList = await listRequest.ExecuteAsync();
                    if (fileList.Files.Count > 0)
                    {
                        var request = service.Files.Get(fileList.Files[0].Id);
                        await request.DownloadAsync(stream);
                        return stream.ToArray();
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }

            }
        }

        public async Task<FileStorage> GetInfo(string id, string type = "fileid")
        {
            Google.Apis.Drive.v3.Data.File cloudFile;
            if (type == "fileid")
            {
                var request = service.Files.Get(id);
                request.Fields = "id, name, mimeType, size, appProperties";
                cloudFile = await request.ExecuteAsync();

            }
            else if (type == "hash")
            {
                var listRequest = service.Files.List();
                listRequest.PageSize = 1000;
                listRequest.Fields = "nextPageToken, files(id, name, mimeType, size, appProperties)";
                listRequest.Q = $"'{folderID}' in parents" + " and appProperties has { key='hash' and value='" + id + "' }";
                var fileList = await listRequest.ExecuteAsync();
                if (fileList.Files.Count > 0)
                {
                    cloudFile = fileList.Files[0];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

            return new FileStorage()
            {
                FileId = cloudFile.Id,
                FileName = cloudFile.Name,
                FileType = cloudFile.MimeType,
                Hash = cloudFile.AppProperties["hash"],
                Plan = "google",
                Size = cloudFile.Size.GetValueOrDefault()
            };
        }

        public async Task<List<string>> ListFileAsync()
        {
            var result = new List<string>();
            string nextPageToken = string.Empty;
            long sumSize = 0;
            do
            {
                FilesResource.ListRequest listRequest = service.Files.List();
                listRequest.Q = $"'{folderID}' in parents";
                listRequest.PageSize = 1000;
                listRequest.Fields = "nextPageToken, files(id, name, size)";
                listRequest.PageToken = nextPageToken;
                var fileList = await listRequest.ExecuteAsync();
                nextPageToken = fileList.NextPageToken;
                var files = fileList.Files;
                var fileIDs = files.Select(f => f.Id);
                sumSize += files.Where(f => f.Size.HasValue).Sum(f => f.Size.Value);
                result.AddRange(fileIDs);
            } while (!string.IsNullOrEmpty(nextPageToken));

            Console.WriteLine("Total disk usage: " + sumSize);
            return result;
        }

        public async Task<List<Tuple<string, string>>> ListFileNameAsync()
        {
            
            var result = new List<Tuple<string, string>>();
            string nextPageToken = string.Empty;
            long sumSize = 0;
            do
            {
                FilesResource.ListRequest listRequest = service.Files.List();
                listRequest.Q = $"'{folderID}' in parents";
                listRequest.PageSize = 1000;
                listRequest.Fields = "nextPageToken, files(id, name, size)";
                listRequest.PageToken = nextPageToken;
                var fileList = await listRequest.ExecuteAsync();
                nextPageToken = fileList.NextPageToken;
                foreach (var file in fileList.Files)
                {
                    var newTuple = Tuple.Create(file.Id, file.Name);
                    result.Add(newTuple);
                }
                sumSize += fileList.Files.Where(f => f.Size.HasValue).Sum(f => f.Size.Value);
            } while (!string.IsNullOrEmpty(nextPageToken));

            Console.WriteLine("Total disk usage: " + sumSize);
            return result;
        }

        public Task<FileStorage> UploadAsync(Stream file, string filename)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return UploadAsync(memoryStream.ToArray(), filename);
            }

        }

        public async Task<string> ProvisionDirectory()
        {
            if (!string.IsNullOrEmpty(_builder.Option.FolderID))
            {
                return _builder.Option.FolderID;
            }

            var listRequest = service.Files.List();
            listRequest.Q = $"mimeType = 'application/vnd.google-apps.folder' and name = '{_builder.Option.FolderName}'";
            var dirList = await listRequest.ExecuteAsync();
            if (dirList.Files.Count() > 0)
            {
                return dirList.Files.First().Id;
            }
            else
            {
                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = _builder.Option.FolderName,
                    MimeType = "application/vnd.google-apps.folder"
                };
                var request = service.Files.Create(fileMetadata);
                request.Fields = "id";
                var newfolder = await request.ExecuteAsync();
                var newfolderID = newfolder.Id;
                var myPerm = new Permission()
                {
                    Type = "user",
                    Role = "writer",
                    EmailAddress = _builder.Option.Owner
                };
                await service.Permissions.Create(myPerm, newfolderID).ExecuteAsync();
                return newfolderID;
            }

        }
    }
}
