using System;
using System.Linq;

using System.Data.Entity;

using XRMS.Data.EntityFramework;
using XRMS.Business.Models;
using XRMS.Business.Repositories.EntityModel;

namespace XRMS.Business.Repositories.DomainModel.FromEF
{
    public class ReportOrderEditionRepository : GenericDomainRepository<ReportOrderEdition, ReportOrderEditionEntity>, IReportOrderEditionRepository
    {
        public ReportOrderEditionRepository(DbContext dbContext) : base(dbContext)
        {

        }

        public override ReportOrderEdition Add(ReportOrderEdition domainModel)
        {
            if (domainModel == null)
                return null;
            try
            {
                domainModel.EditionCounter = (this as GenericEntityRepository<ReportOrderEditionEntity>).GetBy(o => o.ReportCounter == domainModel.ReportCounter).Max(x => x.EditionCounter) + 1;
            }
            catch (InvalidOperationException)
            {
                domainModel.EditionCounter = 0;
            }
            return base.Add(domainModel);
        }

        protected override ReportOrderEditionEntity FindEntityModel(ReportOrderEdition model)
        {
            return this.Find(model.ReportCounter, model.EditionCounter);
        }
    }
}
