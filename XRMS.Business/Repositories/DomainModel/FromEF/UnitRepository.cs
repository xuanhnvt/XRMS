using System.Data.Entity;

using XRMS.Data.EntityFramework;
using XRMS.Business.Models;

namespace XRMS.Business.Repositories.DomainModel.FromEF
{
    public class UnitRepository : GenericIdBaseObjectRepository<Unit, UnitEntity>, IUnitRepository
    {
        public UnitRepository(DbContext dbContext) : base(dbContext)
        {

        }
    }
}
