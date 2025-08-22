using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public class UpdateSpecification<T>
    {
        /// <summary>
        /// Set each column you want to update, Columns that belong to the primary key cannot be updated.
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public UpdateSpecification<T> ColumnsToUpdate(params Expression<Func<T, object>>[] properties)
        {
            Properties = properties;
            return this;
        }

        public Expression<Func<T, object>>[] Properties { get; set; }
    }
}
