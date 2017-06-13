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
    public class ReportEventRepository : GenericDomainRepository<ReportEvent, ReportEventEntity>, IReportEventRepository
    {
        public ReportEventRepository(DbContext dbContext) : base(dbContext)
        {

        }

        public override ReportEvent Add(ReportEvent domainModel)
        {
            if (domainModel == null)
                return null;
            try
            {
                domainModel.EventCounter = (this as GenericEntityRepository<ReportEventEntity>).GetBy(o => o.ReportCounter == domainModel.ReportCounter).Max(x => x.EventCounter) + 1;
                /*domainModel.EventCounter = (this as GenericEntityRepository<ReportEventEntity>).GetBy(o => o.ReportCounter == domainModel.ReportCounter)
                    .OrderByDescending(o => o.EventCounter).FirstOrDefault().EventCounter + 1;*/
            }
            catch (InvalidOperationException)
            {
                domainModel.EventCounter = 1;
            }
            return base.Add(domainModel);
        }

        protected override ReportEventEntity FindEntityModel(ReportEvent model)
        {
            return this.Find(model.ReportCounter, model.EventCounter);
        }
    }
}
