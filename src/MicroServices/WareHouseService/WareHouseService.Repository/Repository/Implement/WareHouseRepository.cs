using Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouseService.Domain;
using WareHouseService.Repository;

namespace WareHouseService.Repository
{
    public class WareHouseRepository : DBRepository, IWareHouseRepository
    {
        public WareHouseDBContext Context { get; }
        public WareHouseRepository(WareHouseDBContext context) : base(context)
        {
            Context = context;
        }

        public IQueryable<T> GetAllWithPage<T>(TableQueryParameter<T> Parameter) where T : class
        {
            var result = Context.Set<T>().Skip((Parameter.pageIndex - 1) * Parameter.pageSize).Take(Parameter.pageSize);
            return result;
        }
    }
}
