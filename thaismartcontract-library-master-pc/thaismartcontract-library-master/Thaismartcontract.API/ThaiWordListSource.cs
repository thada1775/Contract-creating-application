using NBitcoin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Thaismartcontract.API
{
    public class ThaiWordListSource : IWordlistSource
    {
        public Task<Wordlist> Load(string name)
        {
            throw new NotImplementedException();
        }
    }
}
