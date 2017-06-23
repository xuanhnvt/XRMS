using System.Linq;

using System.Data.Entity;
using XRMS.Libraries.BaseObjects;

namespace XRMS.Business.Repositories.DomainModel
{
    public class GenericIdBaseObjectRepository<TIdObject, TEntityModel> : GenericDomainRepository<TIdObject, TEntityModel>, IIdBaseObjectRepository<TIdObject> where TIdObject : IdBaseObject<TIdObject> where TEntityModel : class
    {
        public GenericIdBaseObjectRepository(DbContext dbContext) : base (dbContext)
        {

        }

        public TIdObject GetById(int id)
        {
            //TEntityModel entityModel = (this as GenericEntityRepository<TEntityModel>).GetBy(x => x.Id == id).FirstOrDefault();
            //return ToDomainModel(entityModel);

            // use this code temporarily
            return ToDomainModel(this.Find(id));

            // use below code, maybe it take long time if table have so many records
            //return this.GetAll().FirstOrDefault(x => x.Id == id);

            // use below code, it throw an exception, fix later if I have time
            //return this.GetBy(x => x.Id == id).FirstOrDefault();
        }

        public override TIdObject Add(TIdObject domainModel)
        {
            if (domainModel == null)
                return null;

            //domainModel.Id = (this as GenericEntityRepository<TEntityModel>).GetAll().Max(x => x.Id) + 1;
            domainModel.Id = this.GetAll().Max(x => x.Id) + 1;
            return base.Add(domainModel);
        }

        protected override TEntityModel FindEntityModel(TIdObject model)
        {
            return this.Find(model.Id);
        }
    }
}