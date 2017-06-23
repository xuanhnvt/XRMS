using System;
using System.Data.Entity;

using XRMS.Business.Repositories.DomainModel;

namespace XRMS.Business.UnitOfWorks
{
    /// <summary>
    /// Interface for the "Unit of Work"
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        // Save pending changes to the data store.
        void SaveChanges();

        DbContext GetDbContext();
        DateTime GetDbCurrentDatetime();

        // Repositories
        IRestaurantRepository RestaurantRepository { get; }
        ITableRepository TableRepository { get; }
        IAreaRepository AreaRepository { get; }
        IUnitRepository UnitRepository { get; }
        IProductGroupRepository ProductGroupRepository { get; }
        IProductRepository ProductRepository { get; }
        IMaterialGroupRepository MaterialGroupRepository { get; }
        IMaterialRepository MaterialRepository { get; }
        IRecipeItemRepository RecipeItemRepository { get; }
        IOrderRepository OrderRepository { get; }
        IOrderItemRepository OrderItemRepository { get; }

        IUserRepository UserRepository { get; }
        IUserRoleRepository UserRoleRepository { get; }
        IReportEventRepository ReportEventRepository { get; }

        IReportOrderRepository ReportOrderRepository { get; }
        IReportOrderEditionRepository ReportOrderEditionRepository { get; }
        IReportOrderItemEditionRepository ReportOrderItemEditionRepository { get; }
        IReportOrderItemRepository ReportOrderItemRepository { get; }
        IReportMaterialRepository ReportMaterialRepository { get; }
    }
}
