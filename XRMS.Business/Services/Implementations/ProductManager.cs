using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.Composition;

using Csla.Core;
using Csla.Reflection;

using XRMS.Data.EntityFramework;
using XRMS.Business.Models;
using XRMS.Business.Repositories;
using XRMS.Business.UnitOfWorks;

using MEFedMVVM.ViewModelLocator;

namespace XRMS.Business.Services
{
    /// <summary>
    /// This class implements the IProductManager for WPF purposes.
    /// </summary>
    [PartCreationPolicy(CreationPolicy.Shared)]
    [ExportService(ServiceType.Runtime, typeof(IProductManager))]
    public class ProductManager : GenericIdBaseObjectManager<Product>, IProductManager
    {
        private UnitOfWork _uow;
        //private IRepositoryProvider _provider = new RepositoryProvider(RepositoryFactories.Instance());

        public ProductManager()
        {
            //_uow = new UnitOfWork(new RepositoryProvider(RepositoryFactories.Instance()));
        }

        public override Product GetByKey(Product itemWithKeys)
        {
            return (this as IProductManager).GetById(itemWithKeys.Id);
        }

        public override List<Product> GetList()
        {
            List<Product> list = null;

            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    list = _uow.ProductRepository.GetAll().ToList();
                    if (list != null)
                    {
                        foreach (Product item in list)
                        {
                            item.Group = _uow.ProductGroupRepository.GetById(item.GroupId);
                            item.Unit = _uow.UnitRepository.GetById(item.UnitId);

                            item.Unit.MarkOld();
                            item.Group.MarkOld();
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


        public override bool Create(Product item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            bool result = false;
            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    Product resultItem = _uow.ProductRepository.Add(item);
                    foreach (RecipeItem recipeItem in item.Recipes)
                    {
                        // assign product id
                        recipeItem.ProductId = item.Id;
                        _uow.RecipeItemRepository.Add(recipeItem);
                    }
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

        public override bool Update(Product item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            bool result = false;
            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    _uow.ProductRepository.Update(item);
                    // process deleted list first
                    foreach (RecipeItem recipeItem in (List<RecipeItem>)(item.Recipes as IEditableCollection).GetDeletedList())
                    {
                        if (!recipeItem.IsNew)
                            _uow.RecipeItemRepository.Remove(recipeItem);
                    }
                    foreach (RecipeItem recipeItem in item.Recipes)
                    {
                        // assign product id
                        if (!recipeItem.IsNew)
                        {
                            if (recipeItem.IsDirty)
                                _uow.RecipeItemRepository.Update(recipeItem);
                        }
                        else
                        {
                            _uow.RecipeItemRepository.Add(recipeItem);
                        }
                    }
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

        public override bool Delete(Product item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            bool result = false;
            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    _uow.ProductRepository.Remove(item);
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

        public Product GetById(int id)
        {
            Product item = null;
            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    item = _uow.ProductRepository.GetById(id);
                    if (item != null)
                    {
                        item.Group = _uow.ProductGroupRepository.GetById(item.GroupId);
                        item.Unit = _uow.UnitRepository.GetById(item.UnitId);
                        item.Unit.MarkOld();
                        item.Group.MarkOld();

                        // get recipes
                        List<RecipeItem> itemList = _uow.RecipeItemRepository.GetBy(o => o.ProductId == item.Id).ToList();
                        // get material info of each recipe item
                        foreach (RecipeItem detail in itemList)
                        {
                            //RecipeItem newRecipeItem = item.Recipes.AddNew();
                            //RecipeItem newRecipeItem = RecipeItem.NewObject();
                            //newRecipeItem.MarkAsChild();
                            /*newRecipeItem.Sequence = detail.Sequence;
                            newRecipeItem.ProductId = detail.ProductId;
                            newRecipeItem.MaterialId = detail.MaterialId;
                            newRecipeItem.UsedAmount = detail.UsedAmount;
                            newRecipeItem.MaterialInfo = _uow.MaterialRepository.GetById(newRecipeItem.MaterialId);
                            newRecipeItem.MaterialInfo.Unit = _uow.UnitRepository.GetById(newRecipeItem.MaterialInfo.UnitId);*/

                            detail.MaterialInfo = _uow.MaterialRepository.GetById(detail.MaterialId);
                            detail.MaterialInfo.Unit = _uow.UnitRepository.GetById(detail.MaterialInfo.UnitId);
                            MarkOld(detail);
                            MarkAsChild(detail);
                            item.Recipes.Add(detail);
                        }
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

        public void FetchProductRecipes(Product item)
        {
            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    if (item != null)
                    {
                        // get recipes
                        using (BypassPropertyChecks(item))
                        {
                            var obj = (RecipeItemList)MethodCaller.CreateInstance(typeof(RecipeItemList));
                            //var obj = (RecipeItemList) DataPortal.CreateChild<RecipeItemList>();
                            MarkAsChild(obj);
                            var rlce = obj.RaiseListChangedEvents;
                            obj.RaiseListChangedEvents = false;
                            List<RecipeItem> itemList = _uow.RecipeItemRepository.GetBy(o => o.ProductId == item.Id).ToList();
                            // get material info of each recipe item
                            foreach (RecipeItem detail in itemList)
                            {
                                detail.MaterialInfo = _uow.MaterialRepository.GetById(detail.MaterialId);
                                detail.MaterialInfo.Unit = _uow.UnitRepository.GetById(detail.MaterialInfo.UnitId);
                                MarkOld(detail);
                                MarkAsChild(detail);
                                obj.Add(detail);
                                //item.Recipes.Add(detail);
                            }
                            obj.RaiseListChangedEvents = rlce;
                            LoadProperty(item, Product.RecipesProperty, obj);
                            //MarkAsChild(item.Recipes);
                        }

                        item.MarkOld();
                    }
                    else
                    {
                        throw new ArgumentNullException("item");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }
    }
}