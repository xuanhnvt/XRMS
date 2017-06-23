using System.Data.Entity;

using XRMS.Data.EntityFramework;
using XRMS.Business.Models;

namespace XRMS.Business.Repositories.DomainModel.FromEF
{
    public class TableRepository : GenericIdBaseObjectRepository<Table, TableEntity>, ITableRepository
    {
        public TableRepository(DbContext dbContext) : base (dbContext)
        {

        }

        /*public Table GetById(int id)
        {
            //TableEntity entityModel = (this as GenericEntityRepository<TableEntity>).GetBy(x => x.Id == id).FirstOrDefault();
            //return ToDomainModel(entityModel);

            return this.GetBy(x => x.Id == id).FirstOrDefault();
        }

        public override Table Add(Table domainModel)
        {
            if (domainModel == null)
                return null;

            domainModel.Id = (this as GenericEntityRepository<TableEntity>).GetAll().Max(x => x.Id) + 1;
            //domainModel.Id = this.GetAll().Max(x => x.Id) + 1;
            return base.Add(domainModel);
        }

        protected override TableEntity FindEntityModel(Table table)
        {
            return this.Find(table.Id);
        }
        */

        /*protected override Table ToDomainModel(TableEntity entityModel)
        {
            if (entityModel == null)
                return null;

            return new Table
            {
                Id = entityModel.Id,
                Code = entityModel.Code,
                Name = entityModel.Name,
                Capacity = entityModel.Capacity,
                Description = entityModel.Description,
                State = entityModel.State,
                CurrentOrderId = entityModel.CurrentOrderId,
                LocationId = entityModel.LocationId
            };
        }

        protected override TableEntity ToEntityModel(Table domainModel, bool forUpdating)
        {
            if (domainModel == null)
                return null;

            return new TableEntity
            {
                Id = domainModel.Id,
                Code = domainModel.Code,
                Name = domainModel.Name,
                Capacity = domainModel.Capacity,
                Description = domainModel.Description,
                State = domainModel.State,
                CurrentOrderId = domainModel.CurrentOrderId,
                LocationId = domainModel.LocationId
            };
        }*/
    }
}
