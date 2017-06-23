using System.Data.Entity;

using XRMS.Data.EntityFramework;
using XRMS.Business.Models;

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
