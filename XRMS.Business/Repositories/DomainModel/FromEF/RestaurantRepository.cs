using System.Data.Entity;

using XRMS.Data.EntityFramework;
using XRMS.Business.Models;

namespace XRMS.Business.Repositories.DomainModel.FromEF
{
    public class RestaurantRepository : GenericIdBaseObjectRepository<Restaurant, RestaurantEntity>, IRestaurantRepository
    {
        public RestaurantRepository(DbContext dbContext) : base(dbContext)
        {

        }
    }
}
