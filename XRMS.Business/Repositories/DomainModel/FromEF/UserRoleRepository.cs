using System;
using System.Linq;

using System.Data.Entity;

using XRMS.Data.EntityFramework;
using XRMS.Business.Models;
using XRMS.Business.Repositories.EntityModel;

namespace XRMS.Business.Repositories.DomainModel.FromEF
{
    public class UserRoleRepository : GenericDomainRepository<UserRole, UserRoleEntity>, IUserRoleRepository
    {
        public UserRoleRepository(DbContext dbContext) : base(dbContext)
        {

        }

        public override UserRole Add(UserRole domainModel)
        {
            if (domainModel == null)
                return null;
            try
            {
                domainModel.Id = (this as GenericEntityRepository<UserRoleEntity>).GetAll().Max(x => x.Id);
                domainModel.Id += 1;
            }
            catch (InvalidOperationException)
            {
                domainModel.Id = 1;
            }
            //domainModel.Id = this.GetAll().Max(x => x.Id) + 1;
            return base.Add(domainModel);
        }

        protected override UserRoleEntity FindEntityModel(UserRole model)
        {
            return this.Find(model.Id);
        }
    }
}
