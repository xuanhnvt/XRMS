using System.Data.Entity;

using XRMS.Data.EntityFramework;
using XRMS.Business.Models;

namespace XRMS.Business.Repositories.DomainModel.FromEF
{
    public class ProductGroupRepository : GenericIdBaseObjectRepository<ProductGroup, ProductGroupEntity>, IProductGroupRepository
    {
        public ProductGroupRepository(DbContext dbContext) : base (dbContext)
        {

        }
    }
}
