using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Thaismartcontract.Storage
{
    public class FileStorage
    {
        public string FileId { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string Description { get; set; }
        public long Size { get; set; }
        public string Plan { get; set; }
        public string Hash { get; set; }
    }
}
