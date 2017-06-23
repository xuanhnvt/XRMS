using XRMS.Data.EntityFramework;

namespace XRMS.Business.Repositories.EntityModel.FromEF
{
    public interface ITableRepository : IGenericRepository<TableEntity>
    {
        TableEntity GetById(int id);
    }
}
