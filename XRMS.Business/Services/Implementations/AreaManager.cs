using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.Composition;

using XRMS.Data.EntityFramework;
using XRMS.Business.Models;
using XRMS.Business.Repositories;
using XRMS.Business.UnitOfWorks;

using MEFedMVVM.ViewModelLocator;

namespace XRMS.Business.Services
{
    /// <summary>
    /// This class implements the IAreaManager for WPF purposes.
    /// </summary>
    [PartCreationPolicy(CreationPolicy.Shared)]
    [ExportService(ServiceType.Runtime, typeof(IAreaManager))]
    public class AreaManager : GenericIdBaseObjectManager<Area>, IAreaManager
    {
        private IUnitOfWork _uow;
        //private IRepositoryProvider _provider = new RepositoryProvider(RepositoryFactories.Instance());

        public AreaManager ()
        {
            //_uow = new UnitOfWork(new RepositoryProvider(RepositoryFactories.Instance()));
        }

        public override Area GetByKey(Area itemWithKeys)
        {
            return (this as IAreaManager).GetById(itemWithKeys.Id);
        }

        public override List<Area> GetList()
        {
            List<Area> list = null;

            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    list = _uow.AreaRepository.GetAll().ToList();
                    if (list != null)
                    {
                        foreach (Area item in list)
                            item.MarkOld();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
            return list;
        }


        public override bool Create(Area item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            bool result = false;
            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    _uow.AreaRepository.Add(item);
                    _uow.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
            return result;
        }

        public override bool Update(Area item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            bool result = false;
            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    _uow.AreaRepository.Update(item);
                    _uow.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
            return result;
        }

        public override bool Delete(Area item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            bool result = false;
            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    _uow.AreaRepository.Remove(item);
                    _uow.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
            return result;
        }

        public Area GetById(int id)
        {
            Area item = null;
            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    item = _uow.AreaRepository.GetById(id);
                    if (item != null)
                    {
                        item.MarkOld();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
            return item;
        }
    }
}
