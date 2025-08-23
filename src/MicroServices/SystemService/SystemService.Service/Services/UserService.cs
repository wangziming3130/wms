using Core.Utility;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemService.Repository;

namespace SystemService.Service
{
    public class UserService : IUserService
    {
        private static readonly SiasunLogger logger = SiasunLogger.GetInstance(typeof(UserService));
        private ServiceFactory _serviceFactory;
        public UserService(
            ServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }


        public async Task<bool> DeleteUserById(Guid userId)
        {
            var res = 0;
            var userToDelete = _serviceFactory.DBRepository.Find<UserEntity>(x => x.USER_ID == userId);
            var user = new UserEntity()
            {
                //USER_ID = Guid.NewGuid(),
                USER_NAME = "Alucard",

                USER_ACCOUNT = "AlucardA",
                USER_AVATAR = "",
                USER_PASSWORD = "Password",
                CREATE_TIME = DateTime.Now,
                UPDATE_TIME = DateTime.Now,
                USER_PASSWORD_TIME = DateTime.Now,

            };
            var userToDelete1 = _serviceFactory.DBRepository.Add(user);

            res = _serviceFactory.DBRepository.Delete(userToDelete);

            return res > 0;
        }
    }
}
