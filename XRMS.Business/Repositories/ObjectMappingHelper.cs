using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using AutoMapper;
using XRMS.Data.EntityFramework;
using XRMS.Business.Models;
using XRMS.Libraries.BaseObjects;

namespace XRMS.Business.Repositories
{
    public static class ObjectMappingHelper
    {
        public static void Configure(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Restaurant, RestaurantEntity>().ReverseMap();
            cfg.CreateMap<User, UserEntity>().ReverseMap();
            cfg.CreateMap<UserRole, UserRoleEntity>().ReverseMap();
            cfg.CreateMap<Area, AreaEntity>().ReverseMap();
            //cfg.CreateMap<Table, TableEntity>().ReverseMap();
            cfg.CreateMap<Table, TableEntity>()
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => (byte)src.State))
                .ReverseMap()
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => (TableState)src.State));
            cfg.CreateMap<Unit, UnitEntity>().ReverseMap();
            cfg.CreateMap<ProductGroup, ProductGroupEntity>().ReverseMap();
            cfg.CreateMap<Product, ProductEntity>().ReverseMap();
            cfg.CreateMap<MaterialGroup, MaterialGroupEntity>().ReverseMap();
            cfg.CreateMap<Material, MaterialEntity>().ReverseMap();
            cfg.CreateMap<MaterialLog, MaterialLogEntity>().ReverseMap();
            cfg.CreateMap<RecipeItem, RecipeItemEntity>().ReverseMap();
            cfg.CreateMap<User, UserEntity>().ReverseMap();
            cfg.CreateMap<UserRole, UserRoleEntity>().ReverseMap();
            cfg.CreateMap<Order, OrderEntity>()
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => (byte)src.State))
                .ReverseMap()
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => (OrderState)src.State));
            cfg.CreateMap<OrderItem, OrderItemEntity>()
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => (byte)src.State))
                .ReverseMap()
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => (OrderItemState)src.State));

            // report entity mapping
            cfg.CreateMap<ReportOrder, ReportOrderEntity>()
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => (byte)src.State))
                .ReverseMap()
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => (OrderState)src.State));
            cfg.CreateMap<ReportOrderItem, ReportOrderItemEntity>()
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => (byte)src.State))
                .ReverseMap()
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => (OrderItemState)src.State));
            cfg.CreateMap<ReportMaterial, ReportMaterialEntity>().ReverseMap();
            cfg.CreateMap<ReportEvent, ReportEventEntity>().ReverseMap();
            cfg.CreateMap<ReportOrderEdition, ReportOrderEditionEntity>().ReverseMap();
            cfg.CreateMap<ReportOrderItemEdition, ReportOrderItemEditionEntity>().ReverseMap();

            /*cfg.CreateMap<Table, TableEntity>()
                .ForMember(d => d.FullName, opt => opt.MapFrom(s => s.Name));

            cfg.CreateMap<Customer, CustomerDto>()
                .ForMember(c => c.Orders, opt => opt.Ignore())
                .ReverseMap();*/
        }

        public static void Setup()
        {
            Mapper.Initialize(Configure);
        }
    }
}
