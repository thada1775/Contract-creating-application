using System;
using System.IO;
using System.Linq;

namespace Thaismartcontract.Storage.GDrive
{
    public class GDriveOptionBuilder : IOptionBuilder
    {
        public string BasePath { get; }
        public GDriveOptionBuilder(string basePath)
        {
            BasePath = basePath;
        }

        public IOption Option { get; private set; }
        public void UseGDrive(string connection)
        {
            var rpcDict = connection.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(part => part.Split('='))
            .ToDictionary(split => split[0].ToLower(), split => split[1]);

            Option = new GDriveOption()
            {
                FolderID = rpcDict.ContainsKey("folderid") ? rpcDict["folderid"] : string.Empty,
                CredentialString = File.ReadAllText(Path.Combine(BasePath, rpcDict["credentialpath"])),
                Domain = rpcDict.ContainsKey("domain") ? rpcDict["domain"] : string.Empty,
                Owner = rpcDict.ContainsKey("owner") ? rpcDict["owner"] : "utharn.b@rmutsb.ac.th",
                FolderName = rpcDict.ContainsKey("foldername") ? rpcDict["foldername"] : string.Empty
            };
        }
    }
}
