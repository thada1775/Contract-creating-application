namespace Thaismartcontract.Storage.GDrive
{
    public class GDriveOption : IOption
    {
        public string CredentialString { get; internal set; }
        public string FolderID { get; set; }
        public string Owner { get; internal set; }
        public string Domain { get; internal set; }
        public string FolderName { get; internal set; }
    }
}
