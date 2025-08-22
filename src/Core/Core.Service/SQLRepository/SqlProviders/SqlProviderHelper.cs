using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public class SqlProviderHelper
    {
        public static MemberExpression GetMemberExpression(ParameterExpression parameter, string field)
        {
            if (field.Contains("."))
            {
                var items = field.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                MemberExpression result = null;
                foreach (var item in items)
                {
                    if (result == null)
                    {
                        result = Expression.PropertyOrField(parameter, item);
                    }
                    else
                    {
                        result = Expression.PropertyOrField(result, item);
                    }
                }
                return result;
            }
            else
            {
                return Expression.PropertyOrField(parameter, field);
            }
        }

        public static string GetSearchKeyword(string searchKeyword)
        {
            return $"%{searchKeyword.Trim()}%";
        }
    }
}
