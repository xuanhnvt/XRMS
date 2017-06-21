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
    public class ReportOrderRepository : GenericDomainRepository<ReportOrder, ReportOrderEntity>, IReportOrderRepository
    {
        public ReportOrderRepository(DbContext dbContext) : base(dbContext)
        {

        }

        public ReportOrder GetByReportCounter(long counter)
        {
            //TEntityModel entityModel = (this as GenericEntityRepository<TEntityModel>).GetBy(x => x.Id == id).FirstOrDefault();
            //return ToDomainModel(entityModel);

            // use this code temporarily
            return ToDomainModel(this.Find(counter));

            // use below code, maybe it take long time if table have so many records
            //return this.GetAll().FirstOrDefault(x => x.Id == id);

            // use below code, it throw an exception, fix later if I have time
            //return this.GetBy(x => x.Id == id).FirstOrDefault();
        }

        public ReportOrder GetByOrderCode(string code)
        {
            ReportOrderEntity entityModel = (this as GenericEntityRepository<ReportOrderEntity>).GetBy(x => x.Code == code).FirstOrDefault();
            return ToDomainModel(entityModel);

            // use this code temporarily
            //return ToDomainModel(this.Find(counter));

            // use below code, maybe it take long time if table have so many records
            //return this.GetAll().FirstOrDefault(x => x.Id == id);

            // use below code, it throw an exception, fix later if I have time
            //return this.GetBy(x => x.Id == id).FirstOrDefault();
        }

        public override ReportOrder Add(ReportOrder domainModel)
        {
            if (domainModel == null)
                return null;
            try
            {
                domainModel.ReportCounter = (this as GenericEntityRepository<ReportOrderEntity>).GetAll().Max(x => x.ReportCounter) + 1;
            }
            catch (InvalidOperationException)
            {
                domainModel.ReportCounter = 1;
            }
            //domainModel.ReportCounter = this.GetAll().Max(x => x.ReportCounter) + 1;
            return base.Add(domainModel);
        }

        protected override ReportOrderEntity FindEntityModel(ReportOrder model)
        {
            return this.Find(model.ReportCounter);
        }
    }
}
