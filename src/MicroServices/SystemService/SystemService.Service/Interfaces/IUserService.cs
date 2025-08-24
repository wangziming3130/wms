using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemService.Repository;

namespace SystemService.Service
{
    public interface IUserService
    {
        Task<bool> DeleteUserById(Guid userId);
        Task<UserEntity> AddUser();
        Task<bool> UpdateUser(UserEntity user);
        Task<UserEntity> GetUserById(Guid userId);
    }
}
