using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemService.Repository
{
    [Serializable]
    public class UserEntity
    {
        public Guid USER_ID { get; set; }
        public string? USER_NAME { get; set; }
        public string? USER_ACCOUNT { get; set; }
        public string? USER_PASSWORD { get; set; }
        public string? USER_AVATAR { get; set; }
        public DateTime? USER_PASSWORD_TIME { get; set; }
        public DateTime? CREATE_TIME { get; set; }
        public DateTime? UPDATE_TIME { get; set; }
        //public string? TEST_INFO { get; set; }


    }
}
