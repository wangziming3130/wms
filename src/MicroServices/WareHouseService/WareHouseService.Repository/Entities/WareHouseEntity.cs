using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouseService.Repository
{
    [Serializable]
    public class WareHouseEntity
    {
        public Guid WAREHOUSE_ID { get; set; }
        public string? WAREHOUSE_CODE { get; set; }
        public string? WAREHOUSE_NAME { get; set; }
        public string? WAREHOUSE_REMARK { get; set; }
        public DateTime? CREATE_TIME { get; set; }
        public DateTime? UPDATE_TIME { get; set; }

    }
}
