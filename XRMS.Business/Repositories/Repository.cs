using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Data.Entity;
using Data.Contracts;

namespace Data.Helpers
{
/// <summary>
/// Interface for a class that can provide repositories by type.
/// The class may create the repositories dynamically if it is unable
/// to find one in its cache of repositories.
/// </summary>
/// <remarks>
/// Repositories created by this provider tend to require a <see cref="DbContext"/>
/// to retrieve data.
/// </remarks>
public interface IRepositoryProvider
{
    /// <summary>
    /// Get and set the <see cref="DbContext"/> with which to initialize a repository
    /// if one must be created.
    /// </summary>
    DbContext DbContext { get; set; }

    /// <summary>
    /// Get an <see cref="IRepository{T}"/> for entity type, T.
    /// </summary>
    /// <typeparam name="T">
    /// Root entity type of the <see cref="IRepository{T}"/>.
    /// </typeparam>
    IRepository<T> GetRepositoryForEntityType<T>() where T : class;

    /// <summary>
    /// Get a repository of type T.
    /// </summary>
    /// <typeparam name="T">
    /// Type of the repository, typically a custom repository interface.
    /// </typeparam>
    /// <param name="factory">
    /// An optional repository creation function that takes a <see cref="DbContext"/>
    /// and returns a repository of T. Used if the repository must be created.
    /// </param>
    /// <remarks>
    /// Looks for the requested repository in its cache, returning if found.
    /// If not found, tries to make one with the factory, fallingback to 
    /// a default factory if the factory parameter is null.
    /// </remarks>
    T GetRepository<T>(Func<DbContext, object> factory = null) where T : class;


    /// <summary>
    /// Set the repository to return from this provider.
    /// </summary>
    /// <remarks>
    /// Set a repository if you don't want this provider to create one.
    /// Useful in testing and when developing without a backend
    /// implementation of the object returned by a repository of type T.
    /// </remarks>
    void SetRepository<T>(T repository);
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using Data.Contracts;

namespace Data.Helpers
{
/// <summary>
/// A maker of Repositories.
/// </summary>
/// <remarks>
/// An instance of this class contains repository factory functions for different types.
/// Each factory function takes an EF <see cref="DbContext"/> and returns
/// a repository bound to that DbContext.
/// <para>
/// Designed to be a "Singleton", configured at web application start with
/// all of the factory functions needed to create any type of repository.
/// Should be thread-safe to use because it is configured at app start,
/// before any request for a factory, and should be immutable thereafter.
/// </para>
/// </remarks>
public class RepositoryFactories
{
    /// <summary>
    /// Return the runtime repository factory functions,
    /// each one is a factory for a repository of a particular type.
    /// </summary>
    /// <remarks>
    /// MODIFY THIS METHOD TO ADD CUSTOM FACTORY FUNCTIONS
    /// </remarks>
    private IDictionary<Type, Func<DbContext, object>> GetFactories()
    {
        return new Dictionary<Type, Func<DbContext, object>>
            {
               //{typeof(IArticleRepository), dbContext => new ArticleRepository(dbContext)},
               //{typeof(IUrlRepository), dbContext => new UrlRepository(dbContext)},
            };
    }

    /// <summary>
    /// Constructor that initializes with runtime repository factories
    /// </summary>
    public RepositoryFactories()
    {
        _repositoryFactories = GetFactories();
    }

    /// <summary>
    /// Constructor that initializes with an arbitrary collection of factories
    /// </summary>
    /// <param name="factories">
    /// The repository factory functions for this instance. 
    /// </param>
    /// <remarks>
    /// This ctor is primarily useful for testing this class
    /// </remarks>
    public RepositoryFactories(IDictionary<Type, Func<DbContext, object>> factories)
    {
        _repositoryFactories = factories;
    }

    /// <summary>
    /// Get the repository factory function for the type.
    /// </summary>
    /// <typeparam name="T">Type serving as the repository factory lookup key.</typeparam>
    /// <returns>The repository function if found, else null.</returns>
    /// <remarks>
    /// The type parameter, T, is typically the repository type 
    /// but could be any type (e.g., an entity type)
    /// </remarks>
    public Func<DbContext, object> GetRepositoryFactory<T>()
    {

        Func<DbContext, object> factory;
        _repositoryFactories.TryGetValue(typeof(T), out factory);
        return factory;
    }

    /// <summary>
    /// Get the factory for <see cref="IRepository{T}"/> where T is an entity type.
    /// </summary>
    /// <typeparam name="T">The root type of the repository, typically an entity type.</typeparam>
    /// <returns>
    /// A factory that creates the <see cref="IRepository{T}"/>, given an EF <see cref="DbContext"/>.
    /// </returns>
    /// <remarks>
    /// Looks first for a custom factory in <see cref="_repositoryFactories"/>.
    /// If not, falls back to the <see cref="DefaultEntityRepositoryFactory{T}"/>.
    /// You can substitute an alternative factory for the default one by adding
    /// a repository factory for type "T" to <see cref="_repositoryFactories"/>.
    /// </remarks>
    public Func<DbContext, object> GetRepositoryFactoryForEntityType<T>() where T : class
    {
        return GetRepositoryFactory<T>() ?? DefaultEntityRepositoryFactory<T>();
    }

    /// <summary>
    /// Default factory for a <see cref="IRepository{T}"/> where T is an entity.
    /// </summary>
    /// <typeparam name="T">Type of the repository's root entity</typeparam>
    protected virtual Func<DbContext, object> DefaultEntityRepositoryFactory<T>() where T : class
    {
        return dbContext => new EFRepository<T>(dbContext);
    }

    /// <summary>
    /// Get the dictionary of repository factory functions.
    /// </summary>
    /// <remarks>
    /// A dictionary key is a System.Type, typically a repository type.
    /// A value is a repository factory function
    /// that takes a <see cref="DbContext"/> argument and returns
    /// a repository object. Caller must know how to cast it.
    /// </remarks>
    private readonly IDictionary<Type, Func<DbContext, object>> _repositoryFactories;
    }
}


using System;
using System.Collections.Generic;
using System.Data.Entity;
using Data.Contracts;

namespace Data.Helpers
{
/// <summary>
/// Provides an <see cref="IRepository{T}"/> for a client request.
/// </summary>
/// <remarks>
/// Caches repositories of a given type so that repositories are only created once per provider.
/// create a new provider per client request.
/// </remarks>
public class RepositoryProvider : IRepositoryProvider
{
    public RepositoryProvider(RepositoryFactories repositoryFactories)
    {
        _repositoryFactories = repositoryFactories;
        Repositories = new Dictionary<Type, object>();
    }

    /// <summary>
    /// Get and set the <see cref="DbContext"/> with which to initialize a repository
    /// if one must be created.
    /// </summary>
    public DbContext DbContext { get; set; }

    /// <summary>
    /// Get or create-and-cache the default <see cref="IRepository{T}"/> for an entity of type T.
    /// </summary>
    /// <typeparam name="T">
    /// Root entity type of the <see cref="IRepository{T}"/>.
    /// </typeparam>
    /// <remarks>
    /// If can't find repository in cache, use a factory to create one.
    /// </remarks>
    public IRepository<T> GetRepositoryForEntityType<T>() where T : class
    {
        return GetRepository<IRepository<T>>(
            _repositoryFactories.GetRepositoryFactoryForEntityType<T>());
    }

    /// <summary>
    /// Get or create-and-cache a repository of type T.
    /// </summary>
    /// <typeparam name="T">
    /// Type of the repository, typically a custom repository interface.
    /// </typeparam>
    /// <param name="factory">
    /// An optional repository creation function that takes a DbContext argument
    /// and returns a repository of T. Used if the repository must be created and
    /// caller wants to specify the specific factory to use rather than one
    /// of the injected <see cref="RepositoryFactories"/>.
    /// </param>
    /// <remarks>
    /// Looks for the requested repository in its cache, returning if found.
    /// If not found, tries to make one using <see cref="MakeRepository{T}"/>.
    /// </remarks>
    public virtual T GetRepository<T>(Func<DbContext, object> factory = null) where T : class
    {
        // Look for T dictionary cache under typeof(T).
        object repoObj;
        Repositories.TryGetValue(typeof(T), out repoObj);
        if (repoObj != null)
        {
            return (T)repoObj;
        }

        // Not found or null; make one, add to dictionary cache, and return it.
        return MakeRepository<T>(factory, DbContext);
    }

    /// <summary>
    /// Get the dictionary of repository objects, keyed by repository type.
    /// </summary>
    /// <remarks>
    /// Caller must know how to cast the repository object to a useful type.
    /// <p>This is an extension point. You can register fully made repositories here
    /// and they will be used instead of the ones this provider would otherwise create.</p>
    /// </remarks>
    protected Dictionary<Type, object> Repositories { get; private set; }

    /// <summary>Make a repository of type T.</summary>
    /// <typeparam name="T">Type of repository to make.</typeparam>
    /// <param name="dbContext">
    /// The <see cref="DbContext"/> with which to initialize the repository.
    /// </param>        
    /// <param name="factory">
    /// Factory with <see cref="DbContext"/> argument. Used to make the repository.
    /// If null, gets factory from <see cref="_repositoryFactories"/>.
    /// </param>
    /// <returns></returns>
    protected virtual T MakeRepository<T>(Func<DbContext, object> factory, DbContext dbContext)
    {
        var f = factory ?? _repositoryFactories.GetRepositoryFactory<T>();
        if (f == null)
        {
            throw new NotImplementedException("No factory for repository type, " + typeof(T).FullName);
        }
        var repo = (T)f(dbContext);
        Repositories[typeof(T)] = repo;
        return repo;
    }

    /// <summary>
    /// Set the repository for type T that this provider should return.
    /// </summary>
    /// <remarks>
    /// Plug in a custom repository if you don't want this provider to create one.
    /// Useful in testing and when developing without a backend
    /// implementation of the object returned by a repository of type T.
    /// </remarks>
    public void SetRepository<T>(T repository)
    {
        Repositories[typeof(T)] = repository;
    }

    /// <summary>
    /// The <see cref="RepositoryFactories"/> with which to create a new repository.
    /// </summary>
    /// <remarks>
    /// Should be initialized by constructor injection
    /// </remarks>
    private RepositoryFactories _repositoryFactories;
    }
}

using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Data.Contracts;

namespace Data
{
/// <summary>
/// The EF-dependent, generic repository for data access
/// </summary>
/// <typeparam name="T">Type of entity for this Repository.</typeparam>
public class EFRepository<T> : IRepository<T> where T : class
{
    public EFRepository(DbContext dbContext)
    {
        if (dbContext == null)
            throw new ArgumentNullException("dbContext");
        DbContext = dbContext;
        DbSet = DbContext.Set<T>();
    }

    protected DbContext DbContext { get; set; }

    protected DbSet<T> DbSet { get; set; }

    public virtual IQueryable<T> GetAll()
    {
        return DbSet;
    }

    public virtual T GetById(int id)
    {
        //return DbSet.FirstOrDefault(PredicateBuilder.GetByIdPredicate<T>(id));
        return DbSet.Find(id);
    }

    public virtual void Add(T entity)
    {
        DbEntityEntry dbEntityEntry = DbContext.Entry(entity);
        if (dbEntityEntry.State != EntityState.Detached)
        {
            dbEntityEntry.State = EntityState.Added;
        }
        else
        {
            DbSet.Add(entity);
        }
    }

    public virtual void Update(T entity)
    {
        DbEntityEntry dbEntityEntry = DbContext.Entry(entity);
        if (dbEntityEntry.State == EntityState.Detached)
        {
            DbSet.Attach(entity);
        }
        dbEntityEntry.State = EntityState.Modified;
    }

    public virtual void Delete(T entity)
    {
        DbEntityEntry dbEntityEntry = DbContext.Entry(entity);
        if (dbEntityEntry.State != EntityState.Deleted)
        {
            dbEntityEntry.State = EntityState.Deleted;
        }
        else
        {
            DbSet.Attach(entity);
            DbSet.Remove(entity);
        }
    }

    public virtual void Delete(int id)
    {
        var entity = GetById(id);
        if (entity == null) return; // not found; assume already deleted.
        Delete(entity);
    }
    }
}

using System;
using Data.Contracts;
using Data.Helpers;
using Models;

namespace Data
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
public class UnitOfWork : IUnitOfWork, IDisposable
{
    public UnitOfWork(IRepositoryProvider repositoryProvider)
    {
        CreateDbContext();

        repositoryProvider.DbContext = DbContext;
        RepositoryProvider = repositoryProvider;
    }

    // repositories
    public IRepository<DASH_PYLVR> DASH_PYLVRs { get { return GetStandardRepo<DASH_PYLVR>(); } }
    public IRepository<DASH_SickRecord> DASH_SickRecords { get { return GetStandardRepo<DASH_SickRecord>(); } }
    public IRepository<EMDET> EMDETs { get { return GetStandardRepo<EMDET>(); } }
    public IRepository<EMLVA> EMLVAs { get { return GetStandardRepo<EMLVA>(); } }
    public IRepository<EMLVE> EMLVEs { get { return GetStandardRepo<EMLVE>(); } }
    public IRepository<EMMPO> EMMPOs { get { return GetStandardRepo<EMMPO>(); } }
    public IRepository<EMPOS> EMPOSs { get { return GetStandardRepo<EMPOS>(); } }
    public IRepository<EVENTLOG> EVENTLOGs { get { return GetStandardRepo<EVENTLOG>(); } }
    public IRepository<IDMSTAGING> IDMSTAGINGs { get { return GetStandardRepo<IDMSTAGING>(); } }
    public IRepository<PP_BRADFORD> PP_BRADFORDs { get { return GetStandardRepo<PP_BRADFORD>(); } }
    public IRepository<PP_BRADFORD_SCORES> PP_BRADFORD_SCORESs { get { return GetStandardRepo<PP_BRADFORD_SCORES>(); } }
    public IRepository<PSDET> PSDETs { get { return GetStandardRepo<PSDET>(); } }
    public IRepository<PSLDW> PSLDWs { get { return GetStandardRepo<PSLDW>(); } }
    public IRepository<UPZ88> UPZ88s { get { return GetStandardRepo<UPZ88>(); } }

    /// <summary>
    /// Save pending changes to the database
    /// </summary>
    public void Commit()
    {
        //System.Diagnostics.Debug.WriteLine("Committed");
        DbContext.SaveChanges();
    }

    protected void CreateDbContext()
    {
        DbContext = new CHRISCSEntities();

        // Do NOT enable proxied entities, else serialization fails
        DbContext.Configuration.ProxyCreationEnabled = false;

        // Load navigation properties explicitly (avoid serialization trouble)
        DbContext.Configuration.LazyLoadingEnabled = false;

        // Because Web API will perform validation, I don't need/want EF to do so
        DbContext.Configuration.ValidateOnSaveEnabled = false;
    }

    protected IRepositoryProvider RepositoryProvider { get; set; }

    private IRepository<T> GetStandardRepo<T>() where T : class
    {
        return RepositoryProvider.GetRepositoryForEntityType<T>();
    }
    private T GetRepo<T>() where T : class
    {
        return RepositoryProvider.GetRepository<T>();
    }

    private CHRISCSEntities DbContext { get; set; }

    #region IDisposable

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (DbContext != null)
            {
                DbContext.Dispose();
            }
        }
    }

    #endregion
    }
}


using System.Linq;

namespace Data.Contracts
{
    public interface IRepository<T> where T : class
    {
    IQueryable<T> GetAll();
    T GetById(int id);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
    void Delete(int id);
    }
}

using Models;

namespace Data.Contracts
{
/// <summary>
/// Interface for the "Unit of Work"
/// </summary>
public interface IUnitOfWork
{
    // Save pending changes to the data store.
    void Commit();

    // Repositories
    IRepository<DASH_PYLVR> DASH_PYLVRs { get; }
    IRepository<DASH_SickRecord> DASH_SickRecords { get; }
    IRepository<EMDET> EMDETs { get; }
    IRepository<EMLVA> EMLVAs { get; }
    IRepository<EMLVE> EMLVEs { get; }
    IRepository<EMMPO> EMMPOs { get; }
    IRepository<EMPOS> EMPOSs { get; }
    IRepository<EVENTLOG> EVENTLOGs { get; }
    IRepository<IDMSTAGING> IDMSTAGINGs { get; }
    IRepository<PP_BRADFORD> PP_BRADFORDs { get; }
    IRepository<PP_BRADFORD_SCORES> PP_BRADFORD_SCORESs { get; }
    IRepository<PSDET> PSDETs { get; }
    IRepository<PSLDW> PSLDWs { get; }
    IRepository<UPZ88> UPZ88s { get; }
    }
}
