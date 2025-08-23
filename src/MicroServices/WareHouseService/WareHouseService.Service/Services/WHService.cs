using Core.Utility;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouseService.Repository;

namespace WareHouseService.Service
{
    public class WHService : IWHService
    {
        private static readonly SiasunLogger logger = SiasunLogger.GetInstance(typeof(WHService));
        private ServiceFactory sf;
        public WHService(
            ServiceFactory serviceFactory)
        {
            sf = serviceFactory;
        }
        public async Task<bool> AddWareHouseAndArea()
        {
            var res = 0;
            var warehouse = new WareHouseEntity()
            {
                WAREHOUSE_NAME = "AlucardW",
                WAREHOUSE_CODE = "Alucardwc",
                WAREHOUSE_REMARK = "",
                CREATE_TIME = DateTime.Now,
                UPDATE_TIME = DateTime.Now

            };

            var wareHouseNew = sf.DBRepository.Add<WareHouseEntity>(warehouse);
            var area = new AreaEntity()
            {
                AREA_NAME = "AlucardA",
                WAREHOUSE_ID = wareHouseNew.WAREHOUSE_ID,
                AREA_CODE = "Alucardac",
                AREA_REMARK = "",
                CREATE_TIME = DateTime.Now,
                UPDATE_TIME = DateTime.Now

            };
            var areaNew = sf.DBRepository.Add<AreaEntity>(area);

            return true;
        }

        public async Task<AreaEntity> GetAreaById(Guid id)
        {
            var res = await sf.DBRepository.Filter<AreaEntity>(x => x.AREA_ID == id).FirstOrDefaultAsync();
            var resWithWH = await sf.DBRepository.Filter<AreaEntity>(x => x.AREA_ID == id).Include(u => u.WAREHOUSE).FirstOrDefaultAsync();

            return res;
        }
    }
}
