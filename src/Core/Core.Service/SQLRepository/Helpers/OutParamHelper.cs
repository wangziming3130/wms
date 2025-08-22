using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public class FilterOutParamResult<T>
    {
        public IQueryable<T> QueryableResult { get; set; }
        public int IntParamResult { get; set; }
    }
}
