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
    /// This class implements the IRecipeItemManager for WPF purposes.
    /// </summary>
    [PartCreationPolicy(CreationPolicy.Shared)]
    [ExportService(ServiceType.Runtime, typeof(IRecipeItemManager))]
    public class RecipeItemManager : GenericManager<RecipeItem>, IRecipeItemManager
    {
        private UnitOfWork _uow;
        //private IRepositoryProvider _provider = new RepositoryProvider(RepositoryFactories.Instance());

        public RecipeItemManager()
        {
            //_uow = new UnitOfWork(new RepositoryProvider(RepositoryFactories.Instance()));
        }

        public override RecipeItem GetByKey(RecipeItem itemWithKeys)
        {
            RecipeItem item = null;
            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    item = _uow.RecipeItemRepository
                        .GetBy(o => o.ProductId == itemWithKeys.ProductId
                                    && o.MaterialId == itemWithKeys.MaterialId
                                    && o.Sequence == itemWithKeys.Sequence)
                        .FirstOrDefault();
                    if (item != null)
                    {
                        // get data of inside class
                        item.MaterialInfo = _uow.MaterialRepository.GetById(item.MaterialId);
                        //item.Location.MarkOld();
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

        public override List<RecipeItem> GetList()
        {
            List<RecipeItem> list = null;

            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    list = _uow.RecipeItemRepository.GetAll().ToList();
                    if (list != null)
                    {
                        foreach (RecipeItem item in list)
                        {
                            item.MaterialInfo = _uow.MaterialRepository.GetById(item.MaterialId);
                            //item.Location.MarkOld();
                            item.MarkOld();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
            return list;
        }

        /// <summary>
        /// Find item from list
        /// </summary>
        public override RecipeItem FindItem(RecipeItem item, List<RecipeItem> list)
        {
            RecipeItem resultItem = null;
            try
            {
                if (item == null)
                    throw new ArgumentNullException("item");

                if (list == null)
                    throw new ArgumentNullException("list");

                resultItem = list.SingleOrDefault<RecipeItem>(x => x.ProductId == item.ProductId && x.MaterialId == item.MaterialId && x.Sequence == item.Sequence);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resultItem;
        }

        public override bool Create(RecipeItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            bool result = false;
            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    _uow.RecipeItemRepository.Add(item);
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

        public override bool Update(RecipeItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            bool result = false;
            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    _uow.RecipeItemRepository.Update(item);
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

        public override bool Delete(RecipeItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            bool result = false;
            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    _uow.RecipeItemRepository.Remove(item);
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
    }
}
