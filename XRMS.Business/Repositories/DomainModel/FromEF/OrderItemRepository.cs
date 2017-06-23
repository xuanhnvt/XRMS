using System;
using System.Data.Entity;

using XRMS.Data.EntityFramework;
using XRMS.Business.Models;

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
