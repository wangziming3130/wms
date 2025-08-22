using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemService.Repository
{
    [Serializable]
    public class RoleEntity
    {
        public Guid ROLE_ID { get; set; }
        public string? ROLE_NAME { get; set; }
        public string? ROLE_REMARK { get; set; }
        public DateTime? CREATE_TIME { get; set; }
        public DateTime? UPDATE_TIME { get; set; }

    }
}
