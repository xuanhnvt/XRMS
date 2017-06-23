using System.Data.Entity;

using XRMS.Data.EntityFramework;
using XRMS.Business.Models;

namespace XRMS.Business.Repositories.DomainModel.FromEF
{
    public class ReportMaterialRepository : GenericDomainRepository<ReportMaterial, ReportMaterialEntity>, IReportMaterialRepository
    {
        public ReportMaterialRepository(DbContext dbContext) : base(dbContext)
        {

        }

        /*public override ReportMaterialEdition Add(ReportMaterialEdition domainModel)
        {
            if (domainModel == null)
                return null;
            try
            {
                domainModel.EditionCounter = (this as GenericEntityRepository<ReportMaterialEditionEntity>).GetBy(o => o.ReportCounter == domainModel.ReportCounter).Max(x => x.EditionCounter) + 1;
            }
            catch (InvalidOperationException)
            {
                domainModel.EditionCounter = 1;
            }
            return base.Add(domainModel);
        }
        */
        protected override ReportMaterialEntity FindEntityModel(ReportMaterial model)
        {
            return this.Find(model.ReportCounter, model.OrderItemSequence, model.MaterialCode);
        }
    }
}
