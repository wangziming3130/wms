using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouseService.Service
{
    public interface ICellService
    {
        Task<bool> DeleteCellById(Guid id, Guid userId);
    }
}
