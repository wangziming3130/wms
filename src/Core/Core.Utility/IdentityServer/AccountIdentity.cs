using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Core.Utility
{
    public class AccountIdentity
    {
        public AccountIdentity() { }
        public AccountIdentity(BaseIdentity bi)
        {
            this.Identity = bi;
        }

        private BaseIdentity _identity;
        /// <summary>
        /// Do not use it directly in plugins, use the Get Properties instead in case of simulation.
        /// </summary>
        public BaseIdentity Identity
        {
            get { return _identity; }
            set { _identity = value; }
        }

        [XmlIgnore]
        private BaseIdentity InUseIdentity
        {
            get
            {
                if (Identity != null)
                {
                    if (Identity.AccountMode == AccountMode.Simulate)
                    {
                        return Identity.SimulateIdentity;
                    }
                    else if (Identity.AccountMode == AccountMode.Switch)
                    {
                        return Identity.SwitchIdentity;
                    }
                }
                return Identity;
            }
        }

        public long LastModified { get; set; }

        [XmlIgnore]
        public bool Simulatable
        {
            get { return Identity != null ? Identity.Simulatable : false; }
        }

        [XmlIgnore]
        public bool IsSimulate
        {
            get { return Identity != null ? Identity.AccountMode == AccountMode.Simulate : false; }
        }

        /// <summary>
        /// Represents the authentication type of current user.Type: Enum(AuthType).
        /// Enum Values: 0-Invalid, 1-ADFS, 2-Local, 3-Windows.
        /// </summary>
        /// 
        [XmlIgnore]
        public AuthType AuthenticationType
        {
            get { return InUseIdentity != null ? (AuthType)Enum.Parse(typeof(AuthType), InUseIdentity.AuthenticationType) : AuthType.Invalid; }
        }

        [XmlIgnore]
        /// <summary>
        /// Represents whether current user is authenticated or not. Type: Boolean.
        /// </summary>
        public bool IsAuthenticated
        {
            get { return InUseIdentity != null ? InUseIdentity.IsAuthenticated : false; }
        }

        /// <summary>
        /// Represents the ID property of this user.
        /// The value comes from [Id] in table [Common_User].
        /// Type: Guid.
        /// </summary>
        [XmlIgnore]
        public Guid ID
        {
            get { return InUseIdentity != null ? InUseIdentity.ID : Guid.Empty; }
        }

        /// <summary>
        /// Represents the Account ID property of this user.
        /// When user type is student, the value comes from [StudentId] in table[Common_Student];
        /// When user type is staff, the value comes from [StaffCode] in table [Common_Staff].
        /// </summary>
        [XmlIgnore]
        public string AccountId
        {
            get { return InUseIdentity != null ? InUseIdentity.AccountID : null; }
        }

        /// <summary>
        /// Represents the type of current user.Type: Enum(UserType).
        /// Enum Values: 0-Invalid, 1-Student, 2-Staff.
        /// </summary>
        [XmlIgnore]
        public UserType AccountType
        {
            get { return InUseIdentity != null ? InUseIdentity.AccountType : UserType.Invalid; }
        }

        /// <summary>
        /// Represents the Full Name of the current user.
        /// The value comes from the [FullName] in table [Common_User]
        /// </summary>
        [XmlIgnore]
        public string UPNName
        {
            get { return InUseIdentity != null ? InUseIdentity.Name : null; }
        }

        /// <summary>
        /// Represents the User Principal Name of the current user.
        /// The value comes from the [UPN] in table [Common_User]
        /// </summary>
        [XmlIgnore]
        public string RealUPN
        {
            get { return InUseIdentity != null ? InUseIdentity.UPN : null; }
        }

        /// <summary>
        /// Represents the AAD ObjectId of the current user.
        /// The value comes from the [AADObjectId] in table [Common_User]
        /// </summary>
        [XmlIgnore]
        public string AADObjectId
        {
            get { return InUseIdentity != null ? InUseIdentity.AADObjectId : null; }
        }

        /// <summary>
        /// Represents the mode of current user.Type: Enum(AccountMode).
        /// Enum Values: 0-Normal, 1-Simulate, 2-Switch.
        /// </summary>
        [XmlIgnore]
        public AccountMode AccountMode
        {
            get { return Identity != null ? Identity.AccountMode : AccountMode.Normal; }
        }

        /// <summary>
        /// Represents the Display Name of the current user.
        /// The value comes from the [DisplayName] in table [Common_User]
        /// </summary>
        [XmlIgnore]
        public string DisplayName
        {
            get { return InUseIdentity?.DisplayName; }
        }

        /// <summary>
        /// Represents the User Name of the current user.
        /// The value comes from the [Username] in table [Common_User]
        /// </summary>
        [XmlIgnore]
        public string LoginName
        {
            get { return InUseIdentity != null ? InUseIdentity.LoginName : null; }
        }

        /// <summary>
        /// Represents the Logger Name of the current user.
        /// </summary>
        [XmlIgnore]
        public string LogName
        {
            get { return InUseIdentity?.LoginName; }
        }

        /// <summary>
        /// Represents the Audit Name of the current user.
        /// </summary>
        [XmlIgnore]
        public string AuditName
        {
            get { return InUseIdentity?.DisplayName; }
        }


        //not used yet
        public string[] GroupIds { get; set; }

        [XmlIgnore]
        public List<UserRoleName> RoleNames
        {
            get { return InUseIdentity != null && InUseIdentity.Security != null ? InUseIdentity.Security.RoleNames : new List<UserRoleName>(); }
        }

        [XmlIgnore]
        public List<string> RoleCustomizeNames
        {
            get { return InUseIdentity != null && InUseIdentity.Security != null ? InUseIdentity.Security.RoleCustomizeNames : new List<string>(); }
        }

        [XmlIgnore]
        public List<PermissionLevel> Permissions
        {
            get { return InUseIdentity != null && InUseIdentity.Security != null ? InUseIdentity.Security.Permissions : new List<PermissionLevel>(); }
        }

        [XmlIgnore]
        public TimeSpan ExpiredTimeSpan
        {
            get
            {
                return Identity.ExpiredTime - DateTime.UtcNow;
            }
        }



        /// <summary>
        /// 模糊查询，判断当前用户是否为某一个角色。
        /// 假设ORGAdmin有10个PermissionLevel，CETAdmin有8个PermissionLevel，
        /// 而且ORGAdmin.PermssionLevels ⊋ CETAdmin.PermissonLevels，则返回True，其他情况返回False。
        /// ⊊（真包含于） ⊋（真包含）
        /// </summary>
        /// <param name="roleName">要判断的用户角色</param>
        /// <returns></returns>
        public bool IsInRole(UserRoleName roleName)
        {
            return true;
            //var roleMapping = UserRolePermissionMappingHelper.GetPermissions(roleName);
            //if (roleMapping == null)
            //{
            //    return false;
            //}
            //else
            //{
            //    return Permissions.Intersect(roleMapping).Count() == roleMapping.Count;
            //}
        }

        /// <summary>
        /// 模糊查询，判断当前用户是否为某一个角色。
        /// 假设ORGAdmin有10个PermissionLevel，CETAdmin有8个PermissionLevel，
        /// 而且ORGAdmin.PermssionLevels ⊋ CETAdmin.PermissonLevels，则返回True，其他情况返回False。
        /// ⊊（真包含于） ⊋（真包含）
        /// </summary>
        /// <param name="role">要判断的用户角色集合</param>
        /// <returns></returns>
        public bool IsInRoles(List<UserRoleName> roles)
        {
            foreach (var role in roles)
            {
                if (IsInRole(role))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsInRoleExact(UserRoleName roleName)
        {
            if (RoleNames.Contains(roleName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsInRolesExact(List<UserRoleName> roles)
        {
            foreach (var role in roles)
            {
                if (IsInRoleExact(role))
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasPermission(PermissionLevel permissionLevel)
        {
            return Permissions.Contains(permissionLevel);
        }



        //[Obsolete]
        //public LCMSAuthorizationState AuthState()
        //{
        //    return new LCMSAuthorizationState();
        //}
    }
}
