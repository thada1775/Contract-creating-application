using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thaismartcontract.Storage
{
    public interface IOption
    {
        string CredentialString { get; }
        string FolderID { get; set; }
        string Owner { get; }
        string Domain { get; }
        string FolderName { get; }
    }
}
