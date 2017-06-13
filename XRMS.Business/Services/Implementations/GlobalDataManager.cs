using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;

using Csla;

using XRMS.Data.EntityFramework;
using XRMS.Business.Models;
using XRMS.Business.Repositories;
using XRMS.Business.Services;

using XRMS.Business.Repositories.DomainModel;
using XRMS.Business.UnitOfWorks;

using MEFedMVVM.ViewModelLocator;
using Cinch;

namespace XRMS.Business.Services
{
    /// <summary>
    /// This class implements the IUserManager for WPF purposes.
    /// </summary>
    [PartCreationPolicy(CreationPolicy.Shared)]
    [ExportService(ServiceType.Runtime, typeof(IGlobalDataManager))]
    public class GlobalDataManager : IGlobalDataManager
    {
        private UnitOfWork _uow;

        public GlobalDataManager()
        {
            //_uow = new UnitOfWork(new RepositoryProvider(RepositoryFactories.Instance()));
        }

        public DateTime GetDbCurrentDatetime()
        {
            DateTime currentDatetime = DateTime.Now;
            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    currentDatetime = _uow.GetDbCurrentDatetime();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
            return currentDatetime;
        }

        public Restaurant ReadRestaurantInfo()
        {
            Restaurant result = null;
            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    result = _uow.RestaurantRepository.GetById(1);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
            return result;
        }
    }
}
