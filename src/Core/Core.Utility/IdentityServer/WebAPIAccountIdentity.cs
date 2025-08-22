using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utility
{
    public class WebAPIAccountIdentity : AccountIdentity
    {
        public WebAPIAccountIdentity() { }

        public string APIName { get; set; }
        public string APIMode { get; set; }

        public WebAPIAccountIdentity(List<string> codes)
        {
            var scopes = new List<int>();
            var int_codes = new List<int>();
            foreach (var code in codes)
            {
                int value;
                if (int.TryParse(code, out value))
                {
                    var scope = value / 10000 * 10000;
                    scopes.Add(scope);
                    int_codes.Add(value);
                }
                else
                {
                    throw new System.Exception("WebAPIAccountIdentity create failed with a bad fomate scope");
                }
            }
            this.Codes = int_codes;
            this.Scopes = scopes;
        }

        public WebAPIAccountIdentity(AccountIdentity identity, List<string> scopes) : this(scopes)
        {
            this.Identity = identity.Identity;
            this.LastModified = identity.LastModified;
            this.GroupIds = identity.GroupIds;
        }
        public List<int> Scopes { get; private set; }

        public List<int> Codes { get; private set; }
    }
}
