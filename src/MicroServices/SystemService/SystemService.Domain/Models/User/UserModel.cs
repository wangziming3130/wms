using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemService.Domain
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Account { get; set; }
        public string? Password { get; set; }
        public string? Avatar { get; set; }
        public DateTime? PasswordTime { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }

    }
}
