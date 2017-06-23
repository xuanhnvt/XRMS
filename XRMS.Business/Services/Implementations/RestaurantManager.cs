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
    /// This class implements the IUserManager for WPF purposes.
    /// </summary>
    [PartCreationPolicy(CreationPolicy.Shared)]
    [ExportService(ServiceType.Runtime, typeof(IRestaurantManager))]
    public class RestaurantManager : GenericIdBaseObjectManager<Restaurant>, IRestaurantManager
    {
        private UnitOfWork _uow;

        public RestaurantManager()
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

        public override Restaurant GetByKey(Restaurant itemWithKeys)
        {
            return (this as IRestaurantManager).GetById(itemWithKeys.Id);
        }

        public override List<Restaurant> GetList()
        {
            List<Restaurant> list = null;

            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    list = _uow.RestaurantRepository.GetAll().ToList();
                    if (list != null)
                    {
                        foreach (Restaurant item in list)
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


        public override bool Create(Restaurant item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            bool result = false;
            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    _uow.RestaurantRepository.Add(item);
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

        public override bool Update(Restaurant item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            bool result = false;
            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    _uow.RestaurantRepository.Update(item);
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

        public override bool Delete(Restaurant item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            bool result = false;
            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    _uow.RestaurantRepository.Remove(item);
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

        public Restaurant GetById(int id)
        {
            Restaurant item = null;
            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    item = _uow.RestaurantRepository.GetById(id);
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

        /*public Restaurant ReadRestaurantInfo()
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

        public bool UpdateRestaurantInfo(Restaurant item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            bool result = false;
            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    _uow.RestaurantRepository.Update(item);
                    _uow.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
            return result;
        }*/
    }
}
