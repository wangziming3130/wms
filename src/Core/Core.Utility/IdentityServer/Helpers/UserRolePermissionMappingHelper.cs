//using Core.Domain;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;

//namespace Core.Utility
//{
//    public static class UserRolePermissionMappingHelper
//    {
//        private static readonly AveLogger logger = AveLogger.GetInstance(MethodBase.GetCurrentMethod().ReflectedType);
//        static UserRolePermissionMappingHelper()
//        {
//            mappingDic = new Dictionary<UserRoleName, List<PermissionLevel>>();
//            List<UserRolePermission> mappingList = new List<UserRolePermission>();

//            string json = JsonHelper.ReadJson("DsSeedConfiguration.json");
//            if (!string.IsNullOrEmpty(json))
//            {
//                try
//                {
//                    mappingList = SerializerHelper.DeserializeByJsonConvert<List<UserRolePermission>>(json);
//                }
//                catch (Exception ex)
//                {
//                    logger.Error(ex.ToString());
//                }
//            }
//            foreach (UserRolePermission mapping in mappingList)
//            {
//                foreach (UserRoleName userRoleName in mapping.UserRoles)
//                {
//                    if (!mappingDic.ContainsKey(userRoleName))
//                    {
//                        mappingDic.Add(userRoleName, new List<PermissionLevel>() { mapping.PermissionLevel });
//                    }
//                    else
//                    {
//                        mappingDic[userRoleName].Add(mapping.PermissionLevel);
//                    }
//                }
//            }
//        }

//        private static Dictionary<UserRoleName, List<PermissionLevel>> mappingDic { get; set; }
//        public static Dictionary<UserRoleName, List<PermissionLevel>> Mappings
//        {
//            get
//            {
//                return mappingDic;
//            }
//        }

//        public static List<PermissionLevel> GetPermissions(UserRoleName roleName)
//        {
//            if (Mappings != null && Mappings.ContainsKey(roleName))
//            {
//                return Mappings[roleName];
//            }
//            return null;
//        }
//    }
//}
