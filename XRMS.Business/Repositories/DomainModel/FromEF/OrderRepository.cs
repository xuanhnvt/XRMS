using System;
using System.Linq;
using System.Data.Entity;

using XRMS.Data.EntityFramework;
using XRMS.Business.Models;
using XRMS.Business.Repositories.EntityModel;

namespace XRMS.Business.Repositories.DomainModel.FromEF
{
    public class OrderRepository : GenericDomainRepository<Order, OrderEntity>, IOrderRepository
    {
        public OrderRepository(DbContext dbContext) : base(dbContext)
        {

        }

        public Order GetById(long id)
        {
            //TEntityModel entityModel = (this as GenericEntityRepository<TEntityModel>).GetBy(x => x.Id == id).FirstOrDefault();
            //return ToDomainModel(entityModel);

            // use this code temporarily
            return ToDomainModel(this.Find(id));

            // use below code, maybe it take long time if table have so many records
            //return this.GetAll().FirstOrDefault(x => x.Id == id);

            // use below code, it throw an exception, fix later if I have time
            //return this.GetBy(x => x.Id == id).FirstOrDefault();
        }

        public override Order Add(Order domainModel)
        {
            if (domainModel == null)
                return null;
            try
            {
                domainModel.Id = (this as GenericEntityRepository<OrderEntity>).GetAll().Max(x => x.Id) + 1;
            }
            catch (InvalidOperationException)
            {
                domainModel.Id = 1;
            }
            //domainModel.Id = this.GetAll().Max(x => x.Id) + 1;
            return base.Add(domainModel);
        }

        protected override OrderEntity FindEntityModel(Order model)
        {
            return this.Find(model.Id);
        }
    }
}
