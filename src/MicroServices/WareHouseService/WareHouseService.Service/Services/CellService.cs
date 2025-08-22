using Core.Utility;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouseService.Repository;

namespace WareHouseService.Service
{
    public class CellService : ICellService
    {
        private static readonly SiasunLogger logger = SiasunLogger.GetInstance(typeof(CellService));
        private ServiceFactory _serviceFactory;
        public CellService(
            ServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }


        public async Task<bool> DeleteCellById(Guid id, Guid userId)
        {
            var res = 0;
            //var userToDelete = _serviceFactory.DBRepository.Find<CellEntity>(x => x.USER_ID == userId);

            //res = _serviceFactory.DBRepository.Delete(userToDelete);

            return res > 0 ? true : false;
        }
    }
}
