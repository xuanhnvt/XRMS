using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XRMS.Libraries.BaseObjects;
using XRMS.Business.Models;

namespace XRMS.Business.Repositories.DomainModel
{

    public interface IIdBaseObjectRepository<TIdObject> : IGenericRepository<TIdObject> where TIdObject : IdBaseObject<TIdObject>
    {
        TIdObject GetById(int id);
    }

    public interface IRestaurantRepository : IIdBaseObjectRepository<Restaurant>
    {

    }

    public interface IUserRepository : IIdBaseObjectRepository<User>
    {

    }

    public interface IUserRoleRepository : IGenericRepository<UserRole>
    {

    }
    public interface ITableRepository : IIdBaseObjectRepository<Table>
    {
        //Table GetById(int id);
    }

    public interface IAreaRepository : IIdBaseObjectRepository<Area>
    {
        //Area GetById(int id);
    }

    public interface IUnitRepository : IIdBaseObjectRepository<Unit>
    {

    }

    public interface IProductGroupRepository : IIdBaseObjectRepository<ProductGroup>
    {

    }

    public interface IProductRepository : IIdBaseObjectRepository<Product>
    {

    }

    public interface IMaterialGroupRepository : IIdBaseObjectRepository<MaterialGroup>
    {

    }

    public interface IMaterialRepository : IIdBaseObjectRepository<Material>
    {

    }

    public interface IOrderRepository : IGenericRepository<Order>
    {
        Order GetById(long id);
    }
    public interface IOrderItemRepository : IGenericRepository<OrderItem>
    {

    }

    public interface IRecipeItemRepository : IGenericRepository<RecipeItem>
    {

    }

    public interface IReportOrderRepository : IGenericRepository<ReportOrder>
    {
        ReportOrder GetByReportCounter(long counter);
        ReportOrder GetByOrderId(long counter);
    }
    public interface IReportOrderItemRepository : IGenericRepository<ReportOrderItem>
    {

    }
    public interface IReportOrderEditionRepository : IGenericRepository<ReportOrderEdition>
    {

    }
    public interface IReportOrderItemEditionRepository : IGenericRepository<ReportOrderItemEdition>
    {

    }
    public interface IReportEventRepository : IGenericRepository<ReportEvent>
    {

    }
    public interface IReportMaterialRepository : IGenericRepository<ReportMaterial>
    {

    }
    public interface IMaterialLogRepository : IGenericRepository<MaterialLog>
    {

    }
}
