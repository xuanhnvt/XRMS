using System;
using System.Data.Entity;

using XRMS.Data.EntityFramework;
using XRMS.Business.Models;

namespace XRMS.Business.Repositories.DomainModel.FromEF
{
    public class AreaRepository : GenericIdBaseObjectRepository<Area, AreaEntity>, IAreaRepository
    {
        public AreaRepository(DbContext dbContext) : base (dbContext)
        {

        }

        /*
        public Area GetById(int id)
        {
            //AreaEntity entityModel = (this as GenericEntityRepository<AreaEntity>).GetBy(x => x.Id == id).FirstOrDefault();
            //return ToDomainModel(entityModel);

            return this.GetBy(x => x.Id == id).FirstOrDefault();
        }

        public override Area Add(Area domainModel)
        {
            if (domainModel == null)
                return null;

            domainModel.Id = (this as GenericEntityRepository<AreaEntity>).GetAll().Max(x => x.Id) + 1;
            //domainModel.Id = this.GetAll().Max(x => x.Id) + 1;
            return base.Add(domainModel);
        }

        protected override AreaEntity FindEntityModel(Area area)
        {
            return this.Find(area.Id);
        }*/

        /*protected override Area ToDomainModel(AreaEntity entityModel)
        {
            if (entityModel == null)
                return null;
            return new Area
            {
                Id = entityModel.Id,
                Code = entityModel.Code,
                Name = entityModel.Name,
                Description = entityModel.Description
            };

            //return (Area) Mapper.Map<Area>(entityModel);
        }

        protected override AreaEntity ToEntityModel(Area domainModel)
        {
            if (domainModel == null)
                return null;

            return new AreaEntity
            {
                Id = domainModel.Id,
                Code = domainModel.Code,
                Name = domainModel.Name,
                Description = domainModel.Description
            };

            //AreaEntity entityModel = Mapper.Map<AreaEntity>(domainModel);
            // set id
            //entityModel.Id = forUpdating ? domainModel.Id : (this as GenericEntityRepository<AreaEntity>).GetAll().OrderByDescending(i => i.Id).FirstOrDefault().Id + 1;
            //return entityModel;
        }*/
    }
}