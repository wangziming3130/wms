using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouseService.Repository;

namespace WareHouseService.Service
{
    public interface IWHService
    {
        Task<bool> AddWareHouseAndArea();
        Task<AreaEntity> GetAreaById(Guid id);
        Task<string> GetUserNameById(Guid id);
    }
}
