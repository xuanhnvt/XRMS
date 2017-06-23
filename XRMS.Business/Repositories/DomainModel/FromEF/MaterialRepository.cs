using System;
using System.Data.Entity;

using XRMS.Data.EntityFramework;
using XRMS.Business.Models;

namespace XRMS.Business.Repositories.DomainModel.FromEF
{
    public class MaterialRepository : GenericIdBaseObjectRepository<Material, MaterialEntity>, IMaterialRepository
    {
        public MaterialRepository(DbContext dbContext) : base(dbContext)
        {

        }
    }
}