using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SH.Communication;

namespace SH.UWPToolKit
{
    internal class ParseOperationResult : IParseOperationResult
    {
        public IEnumerable<string> IPs { get; set; }

        public bool Success { get; set; }

        public string ErrorMessage { get; set; }
    }
}
