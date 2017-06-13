using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XRMS.Data.EntityFramework;

namespace XRMS.Business.Repositories.EntityModel.FromEF
{
    public interface ITableRepository : IGenericRepository<TableEntity>
    {
        TableEntity GetById(int id);
    }
}
