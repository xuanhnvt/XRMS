using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.Composition;
using System.Data.Entity.Migrations;

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
    /// This class implements the IOrderManager for WPF purposes.
    /// </summary>
    [PartCreationPolicy(CreationPolicy.Shared)]
    [ExportService(ServiceType.Runtime, typeof(IOrderManager))]
    public class OrderManager : GenericManager<Order>, IOrderManager
    {
        private UnitOfWork _uow;
        //private IRepositoryProvider _provider = new RepositoryProvider(RepositoryFactories.Instance());

        public OrderManager()
        {
            //_uow = new UnitOfWork(new RepositoryProvider(RepositoryFactories.Instance()));
        }

        public override Order GetByKey(Order itemWithKeys)
        {
            return GetById(itemWithKeys.Id);
        }

        public override List<Order> GetList()
        {
            List<Order> list = null;

            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    //list = _uow.OrderRepository.GetAll().ToList();
                    list = _uow.OrderRepository.GetBy(o => o.State < OrderState.Finished && o.IsCancelled != true).ToList();
                    if (list != null)
                    {
                        foreach (Order item in list)
                        {
                            item.Table = _uow.TableRepository.GetById(item.TableId);
                            item.CreatorUser = _uow.UserRepository.GetById(item.CreatorId);
                            item.LockKeeper = _uow.UserRepository.GetById(item.LockKeeperId);

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


        public override bool Create(Order item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            bool result = false;
            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));

                    // add order
                    // get latest order code definition
                    XRMS.Data.EntityFramework.CodeDefinitionEntity code = (_uow.GetDbContext() as XRMS.Data.EntityFramework.XRMSEntities).CodeDefinitionEntities.Where(o => o.Type == "ORDER").FirstOrDefault();

                    // set order data
                    item.Code = code.Prefix + (code.LastIndex + 1).ToString().PadLeft(7, '0');
                    // temporarily set id of user is 1
                    //item.CreatorId = 1;
                    // set order time
                    item.OrderDatetime = _uow.GetDbCurrentDatetime();
                    Order resultItem = _uow.OrderRepository.Add(item);
                    resultItem.CreatorUser = _uow.UserRepository.GetById(item.CreatorId);

                    // add order report
                    ReportOrder report = new ReportOrder();
                    report.Code = resultItem.Code;
                    report.CreatorName = resultItem.CreatorUser.Fullname;
                    report = _uow.ReportOrderRepository.Add(report);

                    // add order edition history
                    ReportOrderEdition orderEdition = new ReportOrderEdition();
                    orderEdition.ReportCounter = report.ReportCounter;
                    orderEdition.EditionUser = resultItem.CreatorUser.Fullname;
                    orderEdition.EditionDate = resultItem.OrderDatetime;
                    orderEdition = _uow.ReportOrderEditionRepository.Add(orderEdition);

                    foreach (OrderItem orderItem in item.OrderItems)
                    {
                        // assign order id and sequence
                        orderItem.OrderId = item.Id;
                        orderItem.CreateDatetime = _uow.GetDbCurrentDatetime();
                        _uow.OrderItemRepository.Add(orderItem);

                        // add order item edition history
                        ReportOrderItemEdition itemEdition = new ReportOrderItemEdition();
                        itemEdition.ReportCounter = orderEdition.ReportCounter;
                        itemEdition.EditionCounter = orderEdition.EditionCounter;
                        itemEdition.Sequence = orderItem.Sequence;
                        itemEdition.ProductCode = orderItem.ProductInfo.Code;
                        itemEdition.ProductName = orderItem.ProductInfo.Name;
                        itemEdition.EditionType = 0;
                        itemEdition.EdittedQuantity = orderItem.EdittedQuantity;

                        itemEdition = _uow.ReportOrderItemEditionRepository.Add(itemEdition);
                    }

                    // create order report and order item report
                    // ...

                    // update last index
                    code.LastIndex++;
                    (_uow.GetDbContext() as XRMS.Data.EntityFramework.XRMSEntities).Set<CodeDefinitionEntity>().AddOrUpdate(code);

                    // set table state to busy, and order id matched with table
                    Table table = _uow.TableRepository.GetById(item.TableId);
                    table.State = TableState.Busy;
                    //table.State = 1;
                    table.CurrentOrderId = item.Id;
                    _uow.TableRepository.Update(table);

                    // add event report
                    ReportEvent reportEvent = new ReportEvent();
                    reportEvent.ReportCounter = report.ReportCounter;
                    reportEvent.EventClass = 0;
                    reportEvent.EventDate = _uow.GetDbCurrentDatetime();
                    reportEvent.Text = resultItem.CreatorUser.Fullname + " created order " + resultItem.Code + " for table \"" + table.Name + "\"";

                    reportEvent = _uow.ReportEventRepository.Add(reportEvent);

                    // commit
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

        public override bool Update(Order item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            bool result = false;
            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    // get order report, useful for writing report
                    ReportOrder report = _uow.ReportOrderRepository.GetByOrderCode(item.Code);
                    item.Table = _uow.TableRepository.GetById(item.TableId);

                    Order order = _uow.OrderRepository.GetById(item.Id);


                    // change table
                    if (order.TableId != item.TableId)
                    {
                        // assign order to new table and free old table
                        // set table state to busy, and order id matched with table
                        Table oldTable = _uow.TableRepository.GetById(order.TableId);
                        oldTable.State = TableState.Free;
                        //oldTable.State = 0;
                        oldTable.CurrentOrderId = 0;
                        _uow.TableRepository.Update(oldTable);

                        Table newTable = item.Table;
                        newTable.State = TableState.Busy;
                        //newTable.State = 1;
                        newTable.CurrentOrderId = item.Id;
                        _uow.TableRepository.Update(newTable);

                        // add event report
                        ReportEvent reportEvent = new ReportEvent();
                        reportEvent.ReportCounter = report.ReportCounter;
                        reportEvent.EventClass = 0;
                        reportEvent.EventDate = _uow.GetDbCurrentDatetime();
                        //reportEvent.Text = "Test";
                        reportEvent.Text = item.LockKeeper.Fullname + " changed order " + item.Code + " from table \"" + oldTable.Name + "\"" + " to \"" + newTable.Name + "\"";
                        reportEvent = _uow.ReportEventRepository.Add(reportEvent);
                    }

                    // update order
                    item.EditDatetime = _uow.GetDbCurrentDatetime();
                    _uow.OrderRepository.Update(item);

                    /*item.EditDatetime = _uow.GetDbCurrentDatetime();
                    _uow.OrderRepository.Update(item);*/

                    // add order edition history
                    ReportOrderEdition orderEdition = new ReportOrderEdition();
                    orderEdition.ReportCounter = report.ReportCounter;
                    orderEdition.EditionUser = item.CreatorUser.Fullname;
                    orderEdition.EditionDate = _uow.GetDbCurrentDatetime();
                    orderEdition = _uow.ReportOrderEditionRepository.Add(orderEdition);

                    // process deleted list first
                    foreach (OrderItem orderItem in (List<OrderItem>)(item.OrderItems as IEditableCollection).GetDeletedList())
                    {
                        if (!orderItem.IsNew)
                            _uow.OrderItemRepository.Remove(orderItem);
                    }

                    foreach (OrderItem orderItem in item.OrderItems)
                    {
                        if (orderItem.IsDirty)
                        {
                            byte editionType = 0;
                            if (orderItem.IsNew == false)
                            {
                                try
                                {
                                    _uow.OrderItemRepository.Update(orderItem);
                                    if (orderItem.IsCancelled == true)
                                    {
                                        // cancel
                                        editionType = 3;
                                    }
                                    else
                                    {
                                        if (orderItem.EdittedQuantity > 0)
                                        {
                                            // increase
                                            editionType = 1;
                                        }
                                        else
                                        {
                                            // decrease
                                            editionType = 2;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception(ex.Message + "\nOrderItem: " + orderItem.OrderId.ToString() + " " + orderItem.Sequence.ToString());
                                }
                            }
                            else
                            {
                                // assign order id and sequence
                                orderItem.OrderId = item.Id;
                                orderItem.CreateDatetime = _uow.GetDbCurrentDatetime();
                                _uow.OrderItemRepository.Add(orderItem);
                            }

                            // add order item edition history
                            ReportOrderItemEdition itemEdition = new ReportOrderItemEdition();
                            itemEdition.ReportCounter = orderEdition.ReportCounter;
                            itemEdition.EditionCounter = orderEdition.EditionCounter;
                            itemEdition.Sequence = orderItem.Sequence;
                            itemEdition.ProductCode = orderItem.ProductInfo.Code;
                            itemEdition.ProductName = orderItem.ProductInfo.Name;
                            itemEdition.EditionType = editionType;
                            itemEdition.EdittedQuantity = orderItem.EdittedQuantity;

                            itemEdition = _uow.ReportOrderItemEditionRepository.Add(itemEdition);
                        }
                    }

                    // error when insert event report 2 times, need one more commit
                    _uow.SaveChanges();

                    // add event report
                    ReportEvent reportEvent1 = new ReportEvent();
                    reportEvent1.ReportCounter = report.ReportCounter;
                    reportEvent1.EventClass = 0;
                    reportEvent1.EventDate = _uow.GetDbCurrentDatetime();
                    reportEvent1.Text = item.LockKeeper.Fullname + " editted order " + item.Code + " for table \"" + item.Table.Name + "\"";

                    reportEvent1 = _uow.ReportEventRepository.Add(reportEvent1);

                    // commit
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

        public override bool Delete(Order item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            bool result = false;
            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    _uow.OrderRepository.Remove(item);
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

        public Order GetById(long id)
        {
            Order item = null;
            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    item = _uow.OrderRepository.GetById(id);
                    if (item != null)
                    {
                        item.Table = _uow.TableRepository.GetById(item.TableId);
                        item.CreatorUser = _uow.UserRepository.GetById(item.CreatorId);
                        item.LockKeeper = _uow.UserRepository.GetById(item.LockKeeperId);

                        // get order items
                        List<OrderItem> itemList = _uow.OrderItemRepository.GetBy(o => o.OrderId == item.Id).ToList();
                        // get material info of each item
                        foreach (OrderItem detail in itemList)
                        {
                            detail.ProductInfo = _uow.ProductRepository.GetById(detail.ProductId);
                            detail.ProductInfo.Unit = _uow.UnitRepository.GetById(detail.ProductInfo.UnitId);
                            detail.SetOldQuantity();
                            detail.EdittedQuantity = 0;
                            MarkOld(detail);
                            MarkAsChild(detail);
                            item.OrderItems.Add(detail);
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

        public void FetchOrderItems(Order item)
        {
            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    if (item != null)
                    {
                        // get order items
                        decimal subTotalPrice = 0;
                        using (BypassPropertyChecks(item))
                        {
                            var obj = (OrderItemList)MethodCaller.CreateInstance(typeof(OrderItemList));
                            //var obj = (OrderItemList) DataPortal.CreateChild<OrderItemList>();
                            MarkAsChild(obj);
                            var rlce = obj.RaiseListChangedEvents;
                            obj.RaiseListChangedEvents = false;
                            List<OrderItem> itemList = _uow.OrderItemRepository.GetBy(o => o.OrderId == item.Id).ToList();
                            // get material info of each orderItem item
                            foreach (OrderItem detail in itemList)
                            {
                                detail.ProductInfo = _uow.ProductRepository.GetById(detail.ProductId);
                                detail.ProductInfo.Unit = _uow.UnitRepository.GetById(detail.ProductInfo.UnitId);
                                detail.SetOldQuantity();
                                detail.EdittedQuantity = 0;
                                MarkOld(detail);
                                MarkAsChild(detail);
                                obj.Add(detail);
                                //item.Recipes.Add(detail);
                                if (detail.IsCancelled != true)
                                {
                                    subTotalPrice += detail.ItemPrice;
                                }
                            }
                            obj.RaiseListChangedEvents = rlce;
                            LoadProperty(item, Order.OrderItemsProperty, obj);
                            //MarkAsChild(item.Recipes);
                        }

                        item.SubTotalPrice = subTotalPrice;
                        /*item.VatPrice = item.VatEnable ? (item.SubTotalPrice * 10 / 100) : 0;
                        item.DiscountPrice = (-1) * (item.SubTotalPrice * item.DiscountPercent / 100);
                        item.TotalPrice = item.SubTotalPrice + item.VatPrice + item.DiscountPrice + item.SpecialDiscount + item.ServiceCharge;
                        item.Change = item.Cash - item.TotalPrice;*/

                        item.MarkOld();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        public void Lock(Order item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    // get order
                    Order order = _uow.OrderRepository.GetById(item.Id);
                    order.LockState = true;
                    order.LockKeeperId = item.LockKeeperId;
                    _uow.OrderRepository.Update(order);

                    // commit
                    _uow.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        public void UpdateDiscountPercent(Order item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    // get order
                    Order order = _uow.OrderRepository.GetById(item.Id);
                    order.DiscountPercent = item.DiscountPercent;
                    _uow.OrderRepository.Update(order);

                    // commit
                    _uow.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        public void UpdateServiceCharge(Order item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    // get order
                    Order order = _uow.OrderRepository.GetById(item.Id);
                    order.ServiceCharge = item.ServiceCharge;
                    _uow.OrderRepository.Update(order);

                    // commit
                    _uow.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        public void UpdateVatEnable(Order item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    // get order
                    Order order = _uow.OrderRepository.GetById(item.Id);
                    order.VatEnable = item.VatEnable;
                    _uow.OrderRepository.Update(order);

                    // commit
                    _uow.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        public void Unlock(Order item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    // get order
                    Order order = _uow.OrderRepository.GetById(item.Id);
                    order.LockState = false;
                    _uow.OrderRepository.Update(order);

                    // commit
                    _uow.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        public void CheckOutOrder(User actor, Order item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    
                    Order order = _uow.OrderRepository.GetById(item.Id);
                    // update order
                    order.State = OrderState.Finished;
                    order.CheckoutDatetime = _uow.GetDbCurrentDatetime();
                    _uow.OrderRepository.Update(order);

                    // set table state to free, and reset order id matched with table
                    Table table = _uow.TableRepository.GetById(order.TableId);
                    table.State = TableState.Free;
                    //table.State = 0;
                    table.CurrentOrderId = 0;
                    _uow.TableRepository.Update(table);

                    // get order report, useful for writing report
                    ReportOrder report = _uow.ReportOrderRepository.GetByOrderCode(item.Code);
                    report.TableCode = item.Table.Code;
                    report.TableName = item.Table.Name;
                    report.State = order.State;
                    report.OrderDatetime = order.OrderDatetime;
                    report.EditDatetime = order.EditDatetime;
                    report.BillDatetime = order.BillDatetime;
                    report.CheckoutDatetime = order.CheckoutDatetime;
                    report.SubTotalPrice = order.SubTotalPrice;
                    report.TotalPrice = order.TotalPrice;
                    report.ServiceCharge = order.ServiceCharge;
                    report.VatEnable = order.VatEnable;
                    report.VatPrice = order.VatPrice;
                    report.DiscountPercent = order.DiscountPercent;
                    report.DiscountPrice = order.DiscountPrice;
                    report.SpecialDiscount = order.SpecialDiscount;
                    report.Cash = order.Cash;
                    report.Change = order.Change;
                    report.PrintCount = order.PrintCount;
                    report.IsCancelled = order.IsCancelled;
                    report.CancelReason = order.CancelReason;
                    _uow.ReportOrderRepository.Update(report);

                    // report order item
                    foreach (OrderItem orderItem in item.OrderItems)
                    {
                        ReportOrderItem reportOrderItem = new ReportOrderItem();
                        reportOrderItem.ReportCounter = report.ReportCounter;
                        reportOrderItem.Sequence = orderItem.Sequence;
                        reportOrderItem.ProductCode = orderItem.ProductInfo.Code;
                        reportOrderItem.ProductName = orderItem.ProductInfo.Name;
                        reportOrderItem.ProductGroup = _uow.ProductGroupRepository.GetById(orderItem.ProductInfo.GroupId).Name;
                        reportOrderItem.UnitName = _uow.UnitRepository.GetById(orderItem.ProductInfo.UnitId).Name;
                        reportOrderItem.UnitPrice = orderItem.ProductInfo.Price;
                        reportOrderItem.Quantity = orderItem.Quantity;
                        reportOrderItem.State = orderItem.State;
                        reportOrderItem.CreateDatetime = orderItem.CreateDatetime;
                        reportOrderItem.StartDatetime = orderItem.StartDatetime;
                        reportOrderItem.StopDatetime = orderItem.StopDatetime;
                        reportOrderItem.ServeDatetime = orderItem.ServeDatetime;
                        reportOrderItem.IsCancelled = orderItem.IsCancelled;
                        reportOrderItem.IsKitchenProcessCompleted = orderItem.IsKitchenProcessCompleted;

                        _uow.ReportOrderItemRepository.Add(reportOrderItem);
                    }

                    // report material
                    List<Material> usedMaterials = new List<Material>();
                    foreach (OrderItem orderItem in item.OrderItems.Where(o => o.IsCancelled != true))
                    {
                        List<RecipeItem> recipeItems = _uow.RecipeItemRepository.GetBy(o => o.ProductId == orderItem.ProductInfo.Id).ToList();
                        foreach(RecipeItem recipeItem in recipeItems)
                        {
                            Material searchMaterial = usedMaterials.Where(o => o.Id == recipeItem.MaterialId).FirstOrDefault();
                            if (searchMaterial == null)
                            {
                                searchMaterial = _uow.MaterialRepository.GetById(recipeItem.MaterialId);
                                usedMaterials.Add(searchMaterial);
                            }
                            //Material searchMaterial = _uow.MaterialRepository.GetById(recipeItem.MaterialId);
                            ReportMaterial reportMaterial = new ReportMaterial();
                            reportMaterial.ReportCounter = report.ReportCounter;
                            reportMaterial.OrderItemSequence = orderItem.Sequence;
                            reportMaterial.MaterialCode = searchMaterial.Code;
                            reportMaterial.MaterialName = searchMaterial.Name;
                            reportMaterial.UnitName = "Test";
                            reportMaterial.Amount = orderItem.Quantity * recipeItem.UsedAmount;
                            _uow.ReportMaterialRepository.Add(reportMaterial);

                            // update material amount in storage
                            searchMaterial.UsageAmount += reportMaterial.Amount;
                            //_uow.MaterialRepository.Update(searchMaterial);
                        }
                    }

                    // update material amount in storage
                    foreach (Material material in usedMaterials)
                    {
                        _uow.MaterialRepository.Update(material);
                    }


                    // add event report
                    ReportEvent reportEvent1 = new ReportEvent();
                    reportEvent1.ReportCounter = report.ReportCounter;
                    reportEvent1.EventClass = 0;
                    reportEvent1.EventDate = _uow.GetDbCurrentDatetime();
                    reportEvent1.Text = actor.Fullname + " checked out order " + item.Code + " for table \"" + item.Table.Name + "\"";

                    reportEvent1 = _uow.ReportEventRepository.Add(reportEvent1);

                    // delete real time order
                    _uow.OrderRepository.Remove(order);

                    // commit
                    _uow.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        /// <summary>
        /// Cancel the order.
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="item"></param>
        public void CancelOrder(User actor, Order item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));

                    Order order = _uow.OrderRepository.GetById(item.Id);
                    // update order
                    order.IsCancelled = true;
                    order.CancelReason = item.CancelReason;
                    _uow.OrderRepository.Update(order);

                    // set table state to free, and reset order id matched with table
                    Table table = _uow.TableRepository.GetById(order.TableId);
                    table.State = TableState.Free;
                    //table.State = 0;
                    table.CurrentOrderId = 0;
                    _uow.TableRepository.Update(table);

                    // get order report, useful for writing report
                    ReportOrder report = _uow.ReportOrderRepository.GetByOrderCode(item.Code);
                    report.TableCode = item.Table.Code;
                    report.TableName = item.Table.Name;
                    report.State = order.State;
                    report.OrderDatetime = order.OrderDatetime;
                    report.EditDatetime = order.EditDatetime;
                    report.BillDatetime = order.BillDatetime;
                    report.CheckoutDatetime = order.CheckoutDatetime;
                    report.SubTotalPrice = order.SubTotalPrice;
                    report.TotalPrice = order.TotalPrice;
                    report.ServiceCharge = order.ServiceCharge;
                    report.VatEnable = order.VatEnable;
                    report.VatPrice = order.VatPrice;
                    report.DiscountPercent = order.DiscountPercent;
                    report.DiscountPrice = order.DiscountPrice;
                    report.SpecialDiscount = order.SpecialDiscount;
                    report.Cash = order.Cash;
                    report.Change = order.Change;
                    report.PrintCount = order.PrintCount;
                    report.IsCancelled = order.IsCancelled;
                    report.CancelReason = order.CancelReason;
                    _uow.ReportOrderRepository.Update(report);

                    // report order item
                    foreach (OrderItem orderItem in item.OrderItems)
                    {
                        ReportOrderItem reportOrderItem = new ReportOrderItem();
                        reportOrderItem.ReportCounter = report.ReportCounter;
                        reportOrderItem.Sequence = orderItem.Sequence;
                        reportOrderItem.ProductCode = orderItem.ProductInfo.Code;
                        reportOrderItem.ProductName = orderItem.ProductInfo.Name;
                        reportOrderItem.ProductGroup = _uow.ProductGroupRepository.GetById(orderItem.ProductInfo.GroupId).Name;
                        reportOrderItem.UnitName = _uow.UnitRepository.GetById(orderItem.ProductInfo.UnitId).Name;
                        reportOrderItem.UnitPrice = orderItem.ProductInfo.Price;
                        reportOrderItem.Quantity = orderItem.Quantity;
                        reportOrderItem.State = orderItem.State;
                        reportOrderItem.CreateDatetime = orderItem.CreateDatetime;
                        reportOrderItem.StartDatetime = orderItem.StartDatetime;
                        reportOrderItem.StopDatetime = orderItem.StopDatetime;
                        reportOrderItem.ServeDatetime = orderItem.ServeDatetime;
                        reportOrderItem.IsCancelled = orderItem.IsCancelled;
                        reportOrderItem.IsKitchenProcessCompleted = orderItem.IsKitchenProcessCompleted;

                        _uow.ReportOrderItemRepository.Add(reportOrderItem);
                    }
                    

                    // add event report
                    ReportEvent reportEvent1 = new ReportEvent();
                    reportEvent1.ReportCounter = report.ReportCounter;
                    reportEvent1.EventClass = 0;
                    reportEvent1.EventDate = _uow.GetDbCurrentDatetime();
                    reportEvent1.Text = actor.Fullname + " cancelled order " + item.Code + " for table \"" + item.Table.Name + "\"";

                    reportEvent1 = _uow.ReportEventRepository.Add(reportEvent1);

                    // delete real time order
                    _uow.OrderRepository.Remove(order);

                    // commit
                    _uow.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }
        
        /// <summary>
        /// Bill the order.
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="item"></param>
        public void BillOrder(User actor, Order item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));

                    Order order = _uow.OrderRepository.GetById(item.Id);
                    // update order
                    order.State = OrderState.Billed;
                    order.BillDatetime = _uow.GetDbCurrentDatetime();
                    order.SubTotalPrice = item.SubTotalPrice;
                    order.Cash = item.Cash;
                    _uow.OrderRepository.Update(order);

                    // set table state to billed
                    Table table = _uow.TableRepository.GetById(order.TableId);
                    table.State = TableState.Billed;
                    _uow.TableRepository.Update(table);

                    // get order report, useful for writing report
                    ReportOrder report = _uow.ReportOrderRepository.GetByOrderCode(item.Code);

                    // add event report
                    ReportEvent reportEvent1 = new ReportEvent();
                    reportEvent1.ReportCounter = report.ReportCounter;
                    reportEvent1.EventClass = 0;
                    reportEvent1.EventDate = _uow.GetDbCurrentDatetime();
                    reportEvent1.Text = actor.Fullname + " billed order " + item.Code + " for table \"" + item.Table.Name + "\"";

                    reportEvent1 = _uow.ReportEventRepository.Add(reportEvent1);

                    // commit
                    _uow.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        /// <summary>
        /// Print the order
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="item"></param>
        public void UpdatePrintCount(User actor, Order item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));

                    Order order = _uow.OrderRepository.GetById(item.Id);
                    if (order.State > OrderState.Printed)
                    {
                        // update order
                        order.PrintCount++;
                    }
                    else
                    {
                        order.State = OrderState.Printed;
                        order.PrintCount++;
                    }
                    _uow.OrderRepository.Update(order);

                    // get order report, useful for writing report
                    ReportOrder report = _uow.ReportOrderRepository.GetByOrderCode(item.Code);

                    // add event report
                    ReportEvent reportEvent1 = new ReportEvent();
                    reportEvent1.ReportCounter = report.ReportCounter;
                    reportEvent1.EventClass = 0;
                    reportEvent1.EventDate = _uow.GetDbCurrentDatetime();
                    reportEvent1.Text = actor.Fullname + " printed order " + item.Code + " for table \"" + item.Table.Name + "\"";

                    reportEvent1 = _uow.ReportEventRepository.Add(reportEvent1);

                    // commit
                    _uow.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        /// <summary>
        /// Find item from list
        /// </summary>
        public override Order FindItem(Order item, List<Order> list)
        {
            Order resultItem = null;
            try
            {
                if (item == null)
                    throw new ArgumentNullException("item");

                if (list == null)
                    throw new ArgumentNullException("list");

                resultItem = list.SingleOrDefault<Order>(x => x.Id == item.Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resultItem;
        }
    }
}