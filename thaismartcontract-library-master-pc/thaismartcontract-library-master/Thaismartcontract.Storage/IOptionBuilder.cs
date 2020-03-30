using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thaismartcontract.Storage.GDrive;

namespace Thaismartcontract.Storage
{
    public interface IOptionBuilder
    {
        IOption Option { get; }
        void UseGDrive(string connection);
    }
}
