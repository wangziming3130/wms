using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemService.Service
{
    public interface IUserService
    {
        Task<bool> DeleteUserById(Guid userId);
    }
}
