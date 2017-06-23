using System.Data.Entity;

using XRMS.Data.EntityFramework;
using XRMS.Business.Models;

namespace XRMS.Business.Repositories.DomainModel.FromEF
{
    public class ReportOrderItemEditionRepository : GenericDomainRepository<ReportOrderItemEdition, ReportOrderItemEditionEntity>, IReportOrderItemEditionRepository
    {
        public ReportOrderItemEditionRepository(DbContext dbContext) : base(dbContext)
        {

        }

        /*public override ReportOrderItemEdition Add(ReportOrderItemEdition domainModel)
        {
            if (domainModel == null)
                return null;
            try
            {
                domainModel.EditionCounter = (this as GenericEntityRepository<ReportOrderItemEditionEntity>).GetBy(o => o.ReportCounter == domainModel.ReportCounter).Max(x => x.EditionCounter) + 1;
            }
            catch (InvalidOperationException)
            {
                domainModel.EditionCounter = 1;
            }
            return base.Add(domainModel);
        }
        */
        protected override ReportOrderItemEditionEntity FindEntityModel(ReportOrderItemEdition model)
        {
            return this.Find(model.ReportCounter, model.EditionCounter, model.Sequence);
        }
    }
}
