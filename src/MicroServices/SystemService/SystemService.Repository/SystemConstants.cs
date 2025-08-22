using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemService.Repository
{
    public class UserConstants
    {
        public static readonly string UserTableName = "T_SYS_USER";

        public static readonly string UserIdColumn = "USER_ID";
        public static readonly string UserNameColumn = "USER_NAME";
        public static readonly string UserAccountColumn = "USER_ACCOUNT";
        public static readonly string UserPasswordColumn = "USER_PASSWORD";
        public static readonly string UserAvatarColumn = "USER_AVATAR";
        public static readonly string UserPasswordTimeColumn = "USER_PASSWORD_TIME";
        public static readonly string UserCreateTimeColumn = "CREATE_TIME";
        public static readonly string UserUpdateTimeColumn = "UPDATE_TIME";
    }

    public class RoleConstants
    {
        public static readonly string RoleTableName = "T_SYS_ROLE";

        public static readonly string RoleIdColumn = "ROLE_ID";
        public static readonly string RoleNameColumn = "ROLE_NAME";
        public static readonly string RoleRemarkColumn = "ROLE_REMARK";
        public static readonly string RoleCreateTimeColumn = "CREATE_TIME";
        public static readonly string RoleUpdateTimeColumn = "UPDATE_TIME";
    }
}
