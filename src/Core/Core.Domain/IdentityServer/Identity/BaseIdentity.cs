using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    [Serializable]
    public class BaseIdentity : IIdentity
    {
        public Guid ID { get; set; }
        public string AccountID { get; set; }
        public string Name { get; set; }
        public string UPN { get; set; }
        public string AADObjectId { get; set; }
        public string LoginName { get; set; }
        public string DisplayName { get; set; }
        public string LogName { get { return this.UPN.GetLoginNameFromUPN(); } }
        public string AuditName { get { return this.UPN.GetLoginNameFromUPN(); } }
        public UserType AccountType { get; set; }
        public bool IsAuthenticated { get; set; }
        public string AuthenticationType { get; set; }
        public BaseSecurity Security { get; set; }
        public long LastModified { get; set; }
        public DateTime ExpiredTime { get; set; }
        public string SecuredSessionID { get; set; }
        public AccountMode AccountMode { get; set; }
        public bool Simulatable { get; set; }
        public string StartUrl { get; set; }
        public BaseIdentity SimulateIdentity { get; set; }
        public BaseIdentity SwitchIdentity { get; set; }
        public string AuthenticationScheme { get; set; }
    }

    [Serializable]
    public class BaseSecurity
    {
        public long LastModified { get; set; }
        public string EmailAddress { get; set; }
        public List<UserRoleName> RoleNames { get; set; }
        public List<string> RoleCustomizeNames { get; set; }
        public List<PermissionLevel> Permissions { get; set; }
        //public LCMSAuthorizationState AuthState { get; set; }
    }

    [Serializable]
    public class CookieIdentity
    {
        public long LastModified { get; set; }
        public string UPN { get; set; }
        public DateTime ExpiredTime { get; set; }
    }

    public static class IdentityNameExtensions
    {
        public static string GetLoginNameFromUPN(this string upnName)
        {
            if (!string.IsNullOrEmpty(upnName))
            {
                if (upnName.Contains('@'))
                {
                    return upnName.Substring(0, upnName.IndexOf('@'));
                }
            }
            return upnName;
        }
    }
}
