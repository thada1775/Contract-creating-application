using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thaismartcontract.API
{
    public class APIOptionBuilder
    {
        private APIOptions _options;
        public APIOptions Options
        {
            get
            {
                return _options;
            }
        }

        public void UseDigibyte(string connection)
        {
            var rpcDict = connection.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(part => part.Split('='))
            .ToDictionary(split => split[0].ToLower(), split => split[1]);

            _options = new APIOptions()
            {
                BaseURL = rpcDict.ContainsKey("baseurl") ? rpcDict["baseurl"] : "https://explorer.Thaismartcontract.com/"
            };

        }
    }
}
