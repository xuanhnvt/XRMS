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
    public class OrderItemRepository : GenericDomainRepository<OrderItem, OrderItemEntity>, IOrderItemRepository
    {
        public OrderItemRepository(DbContext dbContext) : base(dbContext)
        {

        }

        protected override OrderItemEntity FindEntityModel(OrderItem model)
        {
            return this.Find(model.Sequence, model.OrderId);
        }
    }
}
