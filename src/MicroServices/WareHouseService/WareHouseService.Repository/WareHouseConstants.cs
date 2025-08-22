using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouseService.Repository
{
  
    public class WareHouseConstants
    {
        public static readonly string WareHouseTableName = "T_SYS_WAREHOUSE";

        public static readonly string WareHouseIdColumn = "WAREHOUSE_ID";
        public static readonly string WareHouseCodeColumn = "WAREHOUSE_CODE";
        public static readonly string WareHouseNameColumn = "WAREHOUSE_NAME";
        public static readonly string WareHouseRemarkColumn = "WAREHOUSE_REMARK";
        public static readonly string WareHouseCreateTimeColumn = "CREATE_TIME";
        public static readonly string WareHouseUpdateTimeColumn = "UPDATE_TIME";
    }

    public class AreaConstants
    {
        public static readonly string AreaTableName = "T_WH_AREA";

        public static readonly string AreaIdColumn = "AREA_ID";
        public static readonly string WareHouseIdColumn = "WAREHOUSE_ID";
        public static readonly string AreaCodeColumn = "AREA_CODE";
        public static readonly string AreaNameColumn = "AREA_NAME";
        public static readonly string AreaRemarkColumn = "AREA_REMARK";
        public static readonly string AreaCreateTimeColumn = "CREATE_TIME";
        public static readonly string AreaUpdateTimeColumn = "UPDATE_TIME";
    }
}
