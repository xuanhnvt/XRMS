using System.Data.Entity;

using XRMS.Data.EntityFramework;
using XRMS.Business.Models;

namespace XRMS.Business.Repositories.DomainModel.FromEF
{
    public class ReportOrderItemRepository : GenericDomainRepository<ReportOrderItem, ReportOrderItemEntity>, IReportOrderItemRepository
    {
        public ReportOrderItemRepository(DbContext dbContext) : base(dbContext)
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
        protected override ReportOrderItemEntity FindEntityModel(ReportOrderItem model)
        {
            return this.Find(model.ReportCounter, model.Sequence);
        }
    }
}
