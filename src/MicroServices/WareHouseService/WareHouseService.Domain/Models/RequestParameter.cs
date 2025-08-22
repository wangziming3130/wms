using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouseService.Domain
{
    public class TableQueryParameter<T> where T : class
    {
        public T data { get; set; } //查询参数
        public int pageIndex { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public int totalCount { get; set; } = 0;
        /// <summary>
        /// 排序列
        /// </summary>
        public string sortColumn { get; set; } = "Id";
        /// <summary>
        /// 排序类型
        /// </summary>
        public string sortType { get; set; } = "desc";
    }
}
