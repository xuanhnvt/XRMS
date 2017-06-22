using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Collections.ObjectModel;

using System.ComponentModel.Composition;
using MEFedMVVM.ViewModelLocator;

//using DeepEqual;
//using DeepEqual.Syntax;
using Csla.Core;
using Cinch;
using XRMS.Business;
using XRMS.Business.Models;
using XRMS.Business.Services;
using XRMS.Libraries.BaseClasses;
using XRMS.Libraries.Helpers;
using XRMS.Libraries.MVVM;

namespace XRMS.Presentation.ViewModels
{
    /// This is a workspace type ViewModel, which is created by some code in the <c>MainWindowViewModel</c>
    /// and the DataTemplate in the <c>MainWindow.xaml</c>
    /// 
    /// This ViewModel is also able to operate in design mode by using this ViewModel at runtime or by using
    /// data provided by a Design time ViewModel that inherits from this one.
    /// 
    /// This ViewModel also demonstrates EventToCommand where the XAML fires an Command in this ViewModel
    /// based on some RoutedEvent in the XAML.
    [ExportViewModel("KitchenOrdersViewModel")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class KitchenOrdersViewModel : Cinch.ViewModelBase
    {
        #region Private Data Members
        private List<OrderItem> _entireOrderItemList;
        private ObservableCollection<OrderItem> _completedOrderItemList;

        private OrderItemList _kitchenOrderItemList = new OrderItemList();
        private OrderItem _selectedKitchenOrderItem;

        IMessageBoxService messageBoxService = null;
        IUIVisualizerService uiVisualizerService = null;

        IOrderItemManager _orderItemManager = null;
        #endregion // Private Data Members

        #region Constructors
        [ImportingConstructor]
        public KitchenOrdersViewModel(IMessageBoxService messageBoxService, IUIVisualizerService uiVisualizerService,
                                        IOrderItemManager orderItemManager)

        {
            #region Obtain Services
            this.messageBoxService = messageBoxService;
            this.uiVisualizerService = uiVisualizerService;
            this._orderItemManager = orderItemManager;
            #endregion

            this.RefreshCommand = new CommandBase<object>(o => this.ExecuteRefreshCommand());

            this.StartProcessCommand = new CommandBase<OrderItem>(o => this.ExecuteStartProcessCommand(o), o => this.CanExecuteStartProcessCommand(o));
            this.StopProcessCommand = new CommandBase<OrderItem>(o => this.ExecuteStopProcessCommand(o), o => this.CanExecuteStopProcessCommand(o));
            this.SelectOrderItemCommand = new CommandBase<OrderItem>(o => this.ExecuteSelectOrderItemCommand(o), o => this.CanExecuteSelectOrderItemCommand(o));

            this.DisplayName = "Kitchen Management";
            this.InitializeKitchenOrderItemList();

            Mediator.Instance.Register(this);
        }
        #endregion // Constructors

        #region Public Properties

        /// <summary>
        /// A item list that have just been read from database.
        /// </summary>
        public List<OrderItem> EntireOrderItemList
        {
            get { return _entireOrderItemList; }
            set { _entireOrderItemList = value; NotifyPropertyChanged("EntireOrderItemList"); }
        }

        /// <summary>
        /// A item list that have just removed from current list.
        /// </summary>
        public ObservableCollection<OrderItem> CompletedOrderItemList
        {
            get { return _completedOrderItemList; }
            set { _completedOrderItemList = value; NotifyPropertyChanged("CompletedOrderItemList"); }
        }

        /// <summary>
        /// A item list.
        /// </summary>
        public OrderItemList KitchenOrderItemList
        {
            get { return _kitchenOrderItemList; }
            set { _kitchenOrderItemList = value; NotifyPropertyChanged("KitchenOrderItemList"); }
        }

        /// <summary>
        /// The currently-selected item.
        /// </summary>
        public OrderItem SelectedKitchenOrderItem
        {
            get { return _selectedKitchenOrderItem; }
            set { _selectedKitchenOrderItem = value; NotifyPropertyChanged("SelectedKitchenOrderItem"); }
        }

        #endregion // Public Properties

        #region Command Properties

        /// <summary>
        /// Returns the command that, when invoked, refresh to get data
        /// </summary>
        public CommandBase<object> RefreshCommand { get; set; }

        /// <summary>
        /// Returns the command that, when invoked, request to process item
        /// </summary>
        public CommandBase<OrderItem> StartProcessCommand { get; set; }

        public CommandBase<OrderItem> StopProcessCommand { get; set; }

        public CommandBase<OrderItem> SelectOrderItemCommand { get; set; }

        #endregion // Command Properties

        #region Private Method Members

        /// <summary>
        /// Initializes the item list.
        /// </summary>
        private void InitializeKitchenOrderItemList()
        {
            try
            {
                _orderItemManager.GetList().Where(o => o.IsKitchenProcessCompleted != true).ToList().ForEach(x => {
                    x.MarkAsChild();
                    KitchenOrderItemList.Add(x);
                    if(x.State == OrderItemState.Served || x.IsCancelled == true)
                    {
                        x.MarkUpdated();
                    }
                    });
            }
            catch (Exception ex)
            {
                this.messageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        /// <summary>
        /// Initializes this ViewModel.
        /// </summary>
        private void PopulateKitchenOrderItemList()
        {
            try
            {
                OrderItem tempSelectedKitchenOrderItem = SelectedKitchenOrderItem;
                SelectedKitchenOrderItem = null;

                // get entire list
                EntireOrderItemList = _orderItemManager.GetList();

                List<OrderItem> UpdatedOrderItemList = EntireOrderItemList.Where(o => o.IsKitchenProcessCompleted != true).ToList();

                OrderItem searchItem = null;
                // check with current list, if there is changing, need update
                foreach (OrderItem item in UpdatedOrderItemList)
                {
                    searchItem = this.KitchenOrderItemList.SingleOrDefault<OrderItem>(x => x.OrderId == item.OrderId && x.Sequence == item.Sequence);
                    // no have in current list, it means it is new item
                    if (searchItem == null)
                    {
                        /*item.EdittedQuantity = item.Quantity;
                        item.OldQuantity = item.Quantity;
                        item.Quantity = 0;*/
                        item.MarkAsChild();
                        item.MarkUpdated();
                        item.AddChangeLog("New order " + item.Quantity + " -> ");
                        // add this item to list

                        // add message into item
                        // ...
                        KitchenOrderItemList.Add(item);
                    }
                    // item exist in current list, check if there is change
                    else
                    {
                        if (item.IsCancelled == true)
                        {
                            if (searchItem.IsCancelled != item.IsCancelled)
                            {
                                searchItem.MarkUpdated();
                                searchItem.MarkCancelled();

                                searchItem.AddChangeLog("Cancelled");
                            }
                        }
                        else
                        {
                            switch (searchItem.State)
                            {
                                case OrderItemState.Ordered:
                                    if (item.State != OrderItemState.Ordered)
                                    {
                                        searchItem.State = item.State;
                                        //searchItem.EdittedQuantity = item.Quantity - searchItem.Quantity;
                                        //searchItem.UpdatedQuantity = item.Quantity;
                                        searchItem.MarkUpdated();

                                        searchItem.AddChangeLog(searchItem.State.ToString() + " -> ");
                                    }
                                    else
                                    {
                                        if (item.Quantity != searchItem.Quantity)
                                        {
                                            searchItem.State = item.State;
                                            searchItem.SetOldQuantity();
                                            searchItem.Quantity = item.Quantity;
                                            //searchItem.UpdatedQuantity = item.Quantity;
                                            searchItem.MarkUpdated();

                                            searchItem.AddChangeLog("Change quantity " + searchItem.EdittedQuantity.ToString() + " -> ");
                                        }
                                    }
                                    break;
                                case OrderItemState.Processing:
                                    // do nothing
                                    // must care about problem: undo processing
                                    // can chage quantity at this state? Wondering Who will be permitted?

                                    if (item.State > OrderItemState.Processing)
                                    {
                                        searchItem.State = item.State;
                                        searchItem.MarkUpdated();

                                        searchItem.AddChangeLog(searchItem.State.ToString() + " -> ");

                                    }
                                    break;
                                case OrderItemState.Ready:
                                    // check new state (if there), dont care other state
                                    if (item.State == OrderItemState.Served)
                                    {
                                        searchItem.State = item.State;
                                        searchItem.MarkUpdated();

                                        searchItem.AddChangeLog(searchItem.State.ToString() + " -> ");
                                    }
                                    break;
                                case OrderItemState.Served:
                                    {
                                        // do nothing
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }

                // check with current list, is there item cancelled?
                foreach (OrderItem item in KitchenOrderItemList.ToList())
                {
                    searchItem = UpdatedOrderItemList.SingleOrDefault<OrderItem>(x => x.OrderId == item.OrderId && x.Sequence == item.Sequence);
                    // no have in current list, it means it is new item
                    if (searchItem == null)
                    {
                        // search in entire list
                        searchItem = EntireOrderItemList.Where(o => o.IsKitchenProcessCompleted == true).SingleOrDefault<OrderItem>(x => x.OrderId == item.OrderId && x.Sequence == item.Sequence);
                        if (searchItem != null)
                        {
                            // if this item is served
                            if (item.State == OrderItemState.Served || item.IsCancelled == true)
                            {
                                // if user acknowledged this item
                                if (item.IsAcknownledged == true)
                                {
                                    // need remove it from list
                                    KitchenOrderItemList.Remove(item);
                                }
                                //else // user not acknowledge it, let it display for user
                            }
                            else
                            {
                                // after long time, list is updated but this item is remove from kitchen list
                                item.State = searchItem.State;
                                item.IsCancelled = searchItem.IsCancelled;
                                item.Quantity = searchItem.Quantity;
                                
                                item.MarkUpdated();
                                item.AddChangeLog("Order item was processed out.");
                            }
                        }
                        // order of this item is checked out or cancelled
                        else
                        {
                            if (item.State == OrderItemState.OutOfKitchen)
                            {
                                if (item.IsAcknownledged == true)
                                {
                                    // need remove it from list
                                    KitchenOrderItemList.Remove(item);
                                }
                            }
                            else
                            {
                                item.State = OrderItemState.OutOfKitchen;
                                item.MarkUpdated();
                                item.AddChangeLog("Order is checked out.");
                            }
                        }
                    }

                }
                Sort(KitchenOrderItemList);

                if (tempSelectedKitchenOrderItem != null && tempSelectedKitchenOrderItem.IsUpdated != true)
                    SelectedKitchenOrderItem = tempSelectedKitchenOrderItem;

                // update completed item list
                CompletedOrderItemList = new ObservableCollection<OrderItem>((List<OrderItem>)(KitchenOrderItemList as IEditableCollection).GetDeletedList());
            }
            catch (Exception ex)
            {
                this.messageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        private void Sort(ObservableCollection<OrderItem> collection)
        {
            List<OrderItem> sorted = collection.OrderByDescending(x => x.State).ToList();
            for (int i = 0; i < sorted.Count(); i++)
                collection.Move(collection.IndexOf(sorted[i]), i);
        }

        /// <summary>
        /// ExecuteRefreshCommand
        /// </summary>
        private void ExecuteRefreshCommand()
        {
            this.PopulateKitchenOrderItemList();
        }

        /// <summary>
        /// Logic to determine if StartProcessCommand can execute
        /// </summary>
        private Boolean CanExecuteStartProcessCommand(OrderItem item)
        {
            return item != null && item.State == OrderItemState.Ordered && item.IsCancelled != true;
        }

        /// <summary>
        /// Logic to determine if StopProcessCommand can execute
        /// </summary>
        private Boolean CanExecuteStopProcessCommand(OrderItem item)
        {
                return item != null && item.State == OrderItemState.Processing && item.IsCancelled != true;
        }

        /// <summary>
        /// ExecuteStartProcessCommand
        /// </summary>
        private void ExecuteStartProcessCommand(OrderItem item)
        {
            try
            {
                item.State = OrderItemState.Processing;
                _orderItemManager.SetOrderItemState(item);
            }
            catch (Exception ex)
            {
                this.messageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        /// <summary>
        /// ExecuteStopProcessCommand
        /// </summary>
        private void ExecuteStopProcessCommand(OrderItem item)
        {
            try
            {
                item.State = OrderItemState.Ready;
                _orderItemManager.SetOrderItemState(item);
            }

            catch (Exception ex)
            {
                this.messageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        private void ExecuteSelectOrderItemCommand(OrderItem item)
        {
            try
            {
                //if (SelectedOrder != null)
                //this.PopulateSelectedOrder();
                //messageBoxService.ShowInformation("ExecuteSelectOrderItemCommand");
                if (item != null && item.IsUpdated == true)
                {
                    item.Acknowledge();
                    if (item.State == OrderItemState.OutOfKitchen)
                        return;
                    //messageBoxService.ShowInformation("ExecuteSelectOrderItemCommand");
                    // clear log on item
                    // ....

                    if (item.State == OrderItemState.Served || item.IsCancelled == true)
                    {
                        // update item in database for remove this item from this list next time
                        //searchItem.IsOutOfKitchenProcess = true;
                        _orderItemManager.SetOutOfKitchenProcess(item);
                    }
                }

            }

            catch (Exception ex)
            {
                this.messageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        private Boolean CanExecuteSelectOrderItemCommand(OrderItem item)
        {
            return item != null;
        }

        protected override void OnDispose()
        {
            //Mediator.Instance.UnregisterHandler<T>("Updated" + typeof(T).Name + "Successfully", HandleReceivedMessage);
            //Mediator.Instance.UnregisterHandler<T>("Created" + typeof(T).Name + "Successfully", HandleReceivedMessage);
            Mediator.Instance.Unregister(this);
            base.OnDispose();
        }

        /// <summary>
        /// Mediator callback from MainWindowViewModel
        /// </summary>
        [MediatorMessageSink("MainWindowRequestRefreshingData")]
        private void MainWindowRequestRefreshingDataSink(Boolean dummy)
        {
            //this.Refresh();
            this.PopulateKitchenOrderItemList();
        }
        #endregion // Private Method Members
    }
}
