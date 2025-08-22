using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class AuthenticationConstants
    {
        public static Guid TenantId = Guid.Empty;
        public const string Domain = "";

        public const string BearerScheme = "Bearer";
        public const string JwtCookieScheme = "JwtCookie";
        public const string OpenIdConnectScheme = "OpenIdConnect";

        public const string CookieKey = "leogcc.";
        public const string SessionStorageKey = "leogcc.href";

        public const string RedirectFlagMessage = "0F41C33B-DD96-4391-84F5-B6FB89CD539C";
    }
}
