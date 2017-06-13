using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Csla;
using Csla.Server;

using System.Data.Entity;
using System.Data.Entity.Infrastructure;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using XRMS.Business.Repositories.EntityModel;
using XRMS.Libraries.BaseObjects;
using XRMS.Libraries.CslaBase;

namespace XRMS.Business.Repositories.DomainModel
{
    public abstract class GenericDomainRepository<TDomainModel, TEntityModel> : GenericEntityRepository<TEntityModel>, IGenericRepository<TDomainModel> where TDomainModel: CslaBusinessBase<TDomainModel> where TEntityModel : class
    {

        public GenericDomainRepository(DbContext dbContext) : base (dbContext)
        {

        }

        public new virtual IEnumerable<TDomainModel> GetAll()
        {
            //return null;
            return (IEnumerable<TDomainModel>) ToDomainModels(base.GetAll());
        }

        public virtual IEnumerable<TDomainModel> GetBy(System.Linq.Expressions.Expression<Func<TDomainModel, bool>> predicate)
        {
            return base.GetAll().AsQueryable().ProjectTo<TDomainModel>()
               .Where(predicate).ToList();
        }

        public virtual TDomainModel Add(TDomainModel domainModel)
        {
            return (ToDomainModel(base.Add(ToEntityModel(domainModel))));
        }

        public virtual TDomainModel Remove(TDomainModel domainModel)
        {
            // find the correct entity in database first, then remove
            TEntityModel entityModel = FindEntityModel(domainModel);
            return (ToDomainModel(base.Remove(entityModel)));
        }

        public virtual void Update(TDomainModel domainModel)
        {
            base.Update(ToEntityModel(domainModel));
        }

        /*public new virtual void Save()
        {
            base.Save();
        }*/

        protected virtual IEnumerable<TDomainModel> ToDomainModels(IEnumerable<TEntityModel> entityModels)
        {
            if (entityModels == null)
                return null;

            //return Mapper.Map<IEnumerable<TEntityModel>, IEnumerable<TDomainModel>>(entityModels);
            return entityModels.AsEnumerable().Select(entity => Mapper.Map<TEntityModel, TDomainModel>(entity));

        }
        /*protected IEnumerable<TDomainModel> ToDomainModels(IEnumerable<TEntityModel> entityModels)
        {
            if (entityModels == null)
                return null;

            List<TDomainModel> models = new List<TDomainModel>();
            foreach (TEntityModel entity in entityModels)
                models.Add(ToDomainModel(entity));

            return models;
        }*/
        protected virtual TEntityModel ToEntityModel(TDomainModel domainModel)
        {
            return Mapper.Map<TEntityModel>((TDomainModel)domainModel);
        }
        protected virtual TDomainModel ToDomainModel(TEntityModel entityModel)
        {
            TDomainModel item = Csla.DataPortal.Create<TDomainModel>();
            Mapper.Map<TEntityModel, TDomainModel>(entityModel, item);
            item.MarkOld();
            return item;
            //return Mapper.Map<TDomainModel>((TEntityModel)entityModel);
        }

        protected abstract TEntityModel FindEntityModel(TDomainModel domainModel);
    }
}
