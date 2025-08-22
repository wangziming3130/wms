using Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemService.Domain;

namespace SystemService.Repository
{
    public interface IUserRepository : IDBRepository
    {
        public IQueryable<T> GetAllWithPage<T>(TableQueryParameter<T> Parameter) where T : class;
    }
}
