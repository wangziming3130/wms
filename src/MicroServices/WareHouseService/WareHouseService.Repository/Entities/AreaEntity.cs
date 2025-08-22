using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouseService.Repository
{
    [Serializable]
    public class AreaEntity
    {
        public Guid AREA_ID { get; set; }
        
        public string? AREA_CODE { get; set; }
        public string? AREA_NAME { get; set; }
        public string? AREA_REMARK { get; set; }
        public DateTime? CREATE_TIME { get; set; }
        public DateTime? UPDATE_TIME { get; set; }

        public Guid? WAREHOUSE_ID { get; set; }
        public virtual WareHouseEntity WAREHOUSE { get; set; }


    }
}
