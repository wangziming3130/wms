using Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouseService.Domain;

namespace WareHouseService.Repository
{
    public interface IWareHouseRepository : IDBRepository
    {
        public IQueryable<T> GetAllWithPage<T>(TableQueryParameter<T> Parameter) where T : class;
    }
}
