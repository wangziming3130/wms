using Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemService.Domain;
using SystemService.Repository;

namespace SystemService.Repository
{
    public class UserRepository : DBRepository, IUserRepository
    {
        public SystemDBContext Context { get; }
        public UserRepository(SystemDBContext context) : base(context)
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
