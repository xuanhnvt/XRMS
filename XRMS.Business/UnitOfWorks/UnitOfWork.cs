using System;
using System.Linq;
using System.Data.Entity;

using XRMS.Data.EntityFramework;
using XRMS.Business.Repositories;
using XRMS.Business.Repositories.DomainModel;

namespace XRMS.Business.UnitOfWorks
{
    /// <summary>
    /// The "Unit of Work"
    ///     1) decouples the repos from the controllers
    ///     2) decouples the DbContext and EF from the controllers
    ///     3) manages the UoW
    /// </summary>
    /// <remarks>
    /// This class implements the "Unit of Work" pattern in which
    /// the "UoW" serves as a facade for querying and saving to the database.
    /// Querying is delegated to "repositories".
    /// Each repository serves as a container dedicated to a particular
    /// root entity type such as a <see cref="Url"/>.
    /// A repository typically exposes "Get" methods for querying and
    /// will offer add, update, and delete methods if those features are supported.
    /// The repositories rely on their parent UoW to provide the interface to the
    /// data layer (which is the EF DbContext in this example).
    /// </remarks>
    public class UnitOfWork : IUnitOfWork
    {
        //private readonly DbContext _context;
        private bool _disposed = false;

        public UnitOfWork(DbContext context, IRepositoryProvider repositoryProvider)
        {
            //CreateDbContext();

            DbContext = context;

            // Do NOT enable proxied entities, else serialization fails
            DbContext.Configuration.ProxyCreationEnabled = false;

            // Load navigation properties explicitly (avoid serialization trouble)
            DbContext.Configuration.LazyLoadingEnabled = false;

            // Because Web API will perform validation, I don't need/want EF to do so
            DbContext.Configuration.ValidateOnSaveEnabled = false;
            RepositoryProvider = repositoryProvider;
            RepositoryProvider.DbContext = context;
        }

        protected void CreateDbContext()
        {
            DbContext = new XRMSEntities();

            // Do NOT enable proxied entities, else serialization fails
            DbContext.Configuration.ProxyCreationEnabled = false;

            // Load navigation properties explicitly (avoid serialization trouble)
            DbContext.Configuration.LazyLoadingEnabled = false;

            // Because Web API will perform validation, I don't need/want EF to do so
            DbContext.Configuration.ValidateOnSaveEnabled = false;
        }

        /// <summary>
        /// Save pending changes to the database
        /// </summary>
        public void SaveChanges()
        {
            DbContext.SaveChanges();
        }
        protected IRepositoryProvider RepositoryProvider { get; set; }

        public IGenericRepository<T> GetStandardRepo<T>() where T : class
        {
            return RepositoryProvider.GetRepositoryForEntityType<T>();
        }
        public T GetRepo<T>() where T : class
        {
            return RepositoryProvider.GetRepository<T>();
        }

        //private XRMSEntities DbContext { get; set; }
        private DbContext DbContext { get; set; }

        // repositories
        private IRestaurantRepository _restaurantRepository = null;
        public IRestaurantRepository RestaurantRepository
        {
            get
            {
                if (_restaurantRepository == null)
                    _restaurantRepository = GetRepo<IRestaurantRepository>();
                return _restaurantRepository;
            }
        }

        private ITableRepository _tableRepository = null;
        public ITableRepository TableRepository
        {
            get
            {
                if (_tableRepository == null)
                    _tableRepository = GetRepo<ITableRepository>();
                return _tableRepository;
            }
        }

        private IAreaRepository _areaRepository = null;
        public IAreaRepository AreaRepository
        {
            get
            {
                if (_areaRepository == null)
                    _areaRepository = GetRepo<IAreaRepository>();
                return _areaRepository;
            }
        }

        private IUnitRepository _unitRepository = null;
        public IUnitRepository UnitRepository
        {
            get
            {
                if (_unitRepository == null)
                    _unitRepository = GetRepo<IUnitRepository>();
                return _unitRepository;
            }
        }

        private IProductGroupRepository _productGroupRepository = null;
        public IProductGroupRepository ProductGroupRepository
        {
            get
            {
                if (_productGroupRepository == null)
                    _productGroupRepository = GetRepo<IProductGroupRepository>();
                return _productGroupRepository;
            }
        }

        private IProductRepository _productRepository = null;
        public IProductRepository ProductRepository
        {
            get
            {
                if (_productRepository == null)
                    _productRepository = GetRepo<IProductRepository>();
                return _productRepository;
            }
        }

        private IMaterialGroupRepository _materialGroupRepository = null;
        public IMaterialGroupRepository MaterialGroupRepository
        {
            get
            {
                if (_materialGroupRepository == null)
                    _materialGroupRepository = GetRepo<IMaterialGroupRepository>();
                return _materialGroupRepository;
            }
        }

        private IMaterialRepository _materialRepository = null;
        public IMaterialRepository MaterialRepository
        {
            get
            {
                if (_materialRepository == null)
                    _materialRepository = GetRepo<IMaterialRepository>();
                return _materialRepository;
            }
        }

        private IRecipeItemRepository _recipeItemRepository = null;
        public IRecipeItemRepository RecipeItemRepository
        {
            get
            {
                if (_recipeItemRepository == null)
                    _recipeItemRepository = GetRepo<IRecipeItemRepository>();
                return _recipeItemRepository;
            }
        }

        private IOrderRepository _orderRepository = null;
        public IOrderRepository OrderRepository
        {
            get
            {
                if (_orderRepository == null)
                    _orderRepository = GetRepo<IOrderRepository>();
                return _orderRepository;
            }
        }

        private IOrderItemRepository _orderItemRepository = null;
        public IOrderItemRepository OrderItemRepository
        {
            get
            {
                if (_orderItemRepository == null)
                    _orderItemRepository = GetRepo<IOrderItemRepository>();
                return _orderItemRepository;
            }
        }

        private IUserRepository _userRepository = null;
        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = GetRepo<IUserRepository>();
                return _userRepository;
            }
        }

        private IUserRoleRepository _userRoleRepository = null;
        public IUserRoleRepository UserRoleRepository
        {
            get
            {
                if (_userRoleRepository == null)
                    _userRoleRepository = GetRepo<IUserRoleRepository>();
                return _userRoleRepository;
            }
        }

        private IReportEventRepository _reportEventRepository = null;
        public IReportEventRepository ReportEventRepository
        {
            get
            {
                if (_reportEventRepository == null)
                    _reportEventRepository = GetRepo<IReportEventRepository>();
                return _reportEventRepository;
            }
        }

        private IReportOrderRepository _reportOrderRepository = null;
        public IReportOrderRepository ReportOrderRepository
        {
            get
            {
                if (_reportOrderRepository == null)
                    _reportOrderRepository = GetRepo<IReportOrderRepository>();
                return _reportOrderRepository;
            }
        }

        private IReportOrderEditionRepository _reportOrderEditionRepository = null;
        public IReportOrderEditionRepository ReportOrderEditionRepository
        {
            get
            {
                if (_reportOrderEditionRepository == null)
                    _reportOrderEditionRepository = GetRepo<IReportOrderEditionRepository>();
                return _reportOrderEditionRepository;
            }
        }

        private IReportOrderItemEditionRepository _reportOrderItemEditionRepository = null;
        public IReportOrderItemEditionRepository ReportOrderItemEditionRepository
        {
            get
            {
                if (_reportOrderItemEditionRepository == null)
                    _reportOrderItemEditionRepository = GetRepo<IReportOrderItemEditionRepository>();
                return _reportOrderItemEditionRepository;
            }
        }

        private IReportOrderItemRepository _reportOrderItemRepository = null;
        public IReportOrderItemRepository ReportOrderItemRepository
        {
            get
            {
                if (_reportOrderItemRepository == null)
                    _reportOrderItemRepository = GetRepo<IReportOrderItemRepository>();
                return _reportOrderItemRepository;
            }
        }

        private IReportMaterialRepository _reportMaterialRepository = null;
        public IReportMaterialRepository ReportMaterialRepository
        {
            get
            {
                if (_reportMaterialRepository == null)
                    _reportMaterialRepository = GetRepo<IReportMaterialRepository>();
                return _reportMaterialRepository;
            }
        }
        public DbContext GetDbContext()
        {
            return DbContext;
        }

        public DateTime GetDbCurrentDatetime()
        {
            return (DbContext as XRMS.Data.EntityFramework.XRMSEntities)
                    .Database.SqlQuery<DateTime>("SELECT getdate()").AsEnumerable().First();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
                if (disposing)
                    DbContext.Dispose();
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
