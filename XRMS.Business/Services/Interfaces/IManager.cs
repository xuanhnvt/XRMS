using System;
using System.Collections.Generic;

using XRMS.Libraries.BaseObjects;
using XRMS.Business.Models;

namespace XRMS.Business.Services
{
    public interface IAreaManager : IGenericManager<Area>, IGenericIdBaseObjectManager<Area>
    {
        //Area GetById(int id);
    }

    public interface ITableManager : IGenericManager<Table>, IGenericIdBaseObjectManager<Table>
    {
        List<Table> GetFreeTables();
    }

    public interface IUnitManager : IGenericManager<Unit>, IGenericIdBaseObjectManager<Unit>
    {

    }

    public interface IProductGroupManager : IGenericManager<ProductGroup>, IGenericIdBaseObjectManager<ProductGroup>
    {

    }

    public interface IProductManager : IGenericManager<Product>, IGenericIdBaseObjectManager<Product>
    {
        void FetchProductRecipes(Product item);
    }

    public interface IMaterialGroupManager : IGenericManager<MaterialGroup>, IGenericIdBaseObjectManager<MaterialGroup>
    {

    }

    public interface IMaterialManager : IGenericManager<Material>, IGenericIdBaseObjectManager<Material>
    {

    }

    public interface IRecipeItemManager : IGenericManager<RecipeItem>
    {

    }

    public interface IOrderManager : IGenericManager<Order>
    {
        void FetchOrderItems(Order item);
        void Lock(Order item);
        void Unlock(Order item);
        void CheckOutOrder(User actor, Order item);
        void CancelOrder(User actor, Order item);
        void BillOrder(User actor, Order item);
        void UpdatePrintCount(User actor, Order item);
        void UpdateDiscountPercent(Order item);
        void UpdateVatEnable(Order item);
        void UpdateServiceCharge(Order item);
    }

    public interface IOrderItemManager : IGenericManager<OrderItem>
    {
        void SetOrderItemState(OrderItem item);
        void SetOutOfKitchenProcess(OrderItem item);
    }

    public interface IReportOrderItemEditionManager : IGenericManager<ReportOrderItemEdition>
    {
        List<ReportOrderItemEdition> GetEdititonReportOfOrder(Order item);
    }

    public interface IUserManager : IGenericManager<User>, IGenericIdBaseObjectManager<User>
    {

    }
    
    public interface IUserRoleManager : IGenericManager<UserRole>
    {

    }

    public interface IRestaurantManager : IGenericManager<Restaurant>, IGenericIdBaseObjectManager<Restaurant>
    {
        DateTime GetDbCurrentDatetime();
        //Restaurant ReadRestaurantInfo();
        //bool UpdateRestaurantInfo(Restaurant item);
    }
}
