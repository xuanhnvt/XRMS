﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace XRMS.Data.EntityFramework
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class XRMSEntities : DbContext
    {
        public XRMSEntities()
            : base("name=XRMSEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AreaEntity> AREAS { get; set; }
        public virtual DbSet<TableEntity> TABLES { get; set; }
        public virtual DbSet<UserEntity> USERS { get; set; }
        public virtual DbSet<UserRoleEntity> PERMISSIONS { get; set; }
        public virtual DbSet<ProductGroupEntity> ProductGroupEntities { get; set; }
        public virtual DbSet<ProductEntity> PRODUCTS { get; set; }
        public virtual DbSet<UnitEntity> UNITS { get; set; }
        public virtual DbSet<MaterialGroupEntity> MaterialGroupEntities { get; set; }
        public virtual DbSet<MaterialEntity> MATERIALS { get; set; }
        public virtual DbSet<RecipeItemEntity> RECIPES { get; set; }
        public virtual DbSet<OrderItemEntity> OrderItemEntities { get; set; }
        public virtual DbSet<OrderEntity> ORDERS { get; set; }
        public virtual DbSet<CodeDefinitionEntity> CodeDefinitionEntities { get; set; }
        public virtual DbSet<MaterialLogEntity> MaterialLogEntities { get; set; }
        public virtual DbSet<ReportEventEntity> ReportEventEntities { get; set; }
        public virtual DbSet<ReportOrderEditionEntity> ReportOrderEditionEntities { get; set; }
        public virtual DbSet<ReportOrderItemEditionEntity> ReportOrderItemEditionEntities { get; set; }
        public virtual DbSet<ReportOrderItemEntity> ReportOrderItemEntities { get; set; }
        public virtual DbSet<ReportOrderEntity> REPORTS { get; set; }
        public virtual DbSet<RestaurantEntity> RestaurantEntities { get; set; }
        public virtual DbSet<ReportMaterialEntity> ReportMaterialEntities { get; set; }
    }
}