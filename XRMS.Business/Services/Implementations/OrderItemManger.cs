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
    /// This class implements the IOrderItemManager for WPF purposes.
    /// </summary>
    [PartCreationPolicy(CreationPolicy.Shared)]
    [ExportService(ServiceType.Runtime, typeof(IOrderItemManager))]
    public class OrderItemManager : GenericManager<OrderItem>, IOrderItemManager
    {
        private UnitOfWork _uow;
        //private IRepositoryProvider _provider = new RepositoryProvider(RepositoryFactories.Instance());

        public OrderItemManager()
        {
            //_uow = new UnitOfWork(new RepositoryProvider(RepositoryFactories.Instance()));
        }

        public override OrderItem GetByKey(OrderItem itemWithKeys)
        {
            OrderItem item = null;
            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    item = _uow.OrderItemRepository
                        .GetBy(o => o.OrderId == itemWithKeys.OrderId
                                    && o.Sequence == itemWithKeys.Sequence)
                        .FirstOrDefault();
                    if (item != null)
                    {
                        // get data of inside class
                        item.ProductInfo = _uow.ProductRepository.GetById(item.ProductId);
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

        public override List<OrderItem> GetList()
        {
            List<OrderItem> list = null;

            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    list = _uow.OrderItemRepository.GetBy(o => o.IsAlwaysReady == false).ToList();
                    if (list != null)
                    {
                        foreach (OrderItem item in list)
                        {
                            item.ProductInfo = _uow.ProductRepository.GetById(item.ProductId);
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
        public override OrderItem FindItem(OrderItem item, List<OrderItem> list)
        {
            OrderItem resultItem = null;
            try
            {
                if (item == null)
                    throw new ArgumentNullException("item");

                if (list == null)
                    throw new ArgumentNullException("list");

                resultItem = list.SingleOrDefault<OrderItem>(x => x.OrderId == item.OrderId && x.Sequence == item.Sequence);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resultItem;
        }

        public override bool Create(OrderItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            bool result = false;
            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    _uow.OrderItemRepository.Add(item);
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

        public override bool Update(OrderItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            bool result = false;
            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    _uow.OrderItemRepository.Update(item);
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

        public override bool Delete(OrderItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            bool result = false;
            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    _uow.OrderItemRepository.Remove(item);
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

        public void SetOrderItemState(OrderItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    // get order item
                    OrderItem orderItem = _uow.OrderItemRepository.GetBy(o => o.OrderId == item.OrderId && o.Sequence == item.Sequence).FirstOrDefault();
                    orderItem.State = item.State;
                    _uow.OrderItemRepository.Update(orderItem);

                    // commit
                    _uow.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        public void SetOutOfKitchenProcess(OrderItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    // get order item
                    OrderItem orderItem = _uow.OrderItemRepository.GetBy(o => o.OrderId == item.OrderId && o.Sequence == item.Sequence).FirstOrDefault();
                    orderItem.IsKitchenProcessCompleted = true;
                    _uow.OrderItemRepository.Update(orderItem);

                    // commit
                    _uow.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }
    }
}
