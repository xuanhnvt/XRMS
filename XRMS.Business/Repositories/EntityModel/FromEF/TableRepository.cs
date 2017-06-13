using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity;

using XRMS.Data.EntityFramework;

namespace XRMS.Business.Repositories.EntityModel.FromEF
{
    public class TableRepository : GenericEntityRepository<TableEntity>, ITableRepository
    {
        public TableRepository (DbContext dbContext) : base (dbContext)
        {

        }

        public TableEntity GetById(int id)
        {
            var query = this.GetAll().FirstOrDefault(x => x.Id == id);
            return query;
        }
    }
}
