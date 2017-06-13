using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity;
using System.Data.Entity.Infrastructure;

using XRMS.Data.EntityFramework;
using XRMS.Business.Models;
using XRMS.Business.Repositories.EntityModel;

namespace XRMS.Business.Repositories.DomainModel.FromEF
{
    public class RestaurantRepository : GenericIdBaseObjectRepository<Restaurant, RestaurantEntity>, IRestaurantRepository
    {
        public RestaurantRepository(DbContext dbContext) : base(dbContext)
        {

        }
    }
}
