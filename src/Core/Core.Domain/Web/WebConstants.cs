using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class WebConstants
    {
        public const int DefaultMaxFileSize = int.MaxValue;

        public const string DefaultCorsPolicyName = "CommonCorsPolicy";
    }

    public class HttpClientDefault
    {
        public const string IgnoreDangerous = "IgnoreDangerous";
    }
}
