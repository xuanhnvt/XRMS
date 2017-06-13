using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using System.Data.Entity;
using System.Data.Entity.Infrastructure;

using XRMS.Data.EntityFramework;
using XRMS.Business.Models;
using XRMS.Business.Repositories.EntityModel;

namespace XRMS.Business.Repositories.DomainModel.FromEF
{
    public class RecipeItemRepository : GenericDomainRepository<RecipeItem, RecipeItemEntity>, IRecipeItemRepository
    {
        public RecipeItemRepository(DbContext dbContext) : base(dbContext)
        {

        }

        protected override RecipeItemEntity FindEntityModel(RecipeItem model)
        {
            return this.Find(model.Sequence, model.ProductId, model.MaterialId);
            //return this.Find(model.Id);
        }
    }
}
