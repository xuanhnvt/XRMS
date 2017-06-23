using System;
using System.Data.Entity;

using XRMS.Data.EntityFramework;
using XRMS.Business.Models;

namespace XRMS.Business.Repositories.DomainModel.FromEF
{
    public class MaterialGroupRepository : GenericIdBaseObjectRepository<MaterialGroup, MaterialGroupEntity>, IMaterialGroupRepository
    {
        public MaterialGroupRepository(DbContext dbContext) : base(dbContext)
        {

        }
    }
}
