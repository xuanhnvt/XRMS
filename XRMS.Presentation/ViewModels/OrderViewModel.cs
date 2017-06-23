using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

using Cinch;

using XRMS.Business.Models;
using XRMS.Business.Services;
using XRMS.Libraries.MVVM;

namespace XRMS.Presentation.ViewModels
{
    public class OrderViewModel : ItemViewModelBase<Order>
    {
        #region Private Data Members
        private bool _isShowCancelledProduct;
        //services
        private ITableManager _tableManager = null;
        private List<Table> _freeTableList = null;

        private IProductGroupManager _productGroupManager = null;
        private List<ProductGroup> _productGroupList = null;

        private IProductManager _productManager = null;
        private List<Product> _availableProductList = null;

        private OrderItem _selectedOrderItem;

        #endregion // Private Data Members

        #region Constructors

        public OrderViewModel(IMessageBoxService messageBoxService, IOrderManager manager, ITableManager tableManager) : base(messageBoxService, manager)
        {
            // do initialization
            try
            {
                if (tableManager == null)
                {
                    throw new ArgumentNullException("tableManager");
                }
                _tableManager = tableManager;
                // populate the list of tables
                this.FreeTableList = _tableManager.GetFreeTables();

                _productGroupManager = new ProductGroupManager();
                this.ProductGroupList = _productGroupManager.GetList();
                // add one item, in case user select all group
                this.ProductGroupList.Insert(0, new ProductGroup
                {
                    Id = -1,
                    Code = "ALL",
                    Name = "All",
                    Description = "List of all products"
                });

                _productManager = new ProductManager();
                this.AvailableProductList = _productManager.GetList();

                // initialize command
                this.AddOrderItemCommand = new CommandBase<Product>(o => this.ExecuteAddOrderItemCommand(o), o => this.CanExecuteAddOrderItemCommand(o));
                this.CancelOrderItemCommand = new CommandBase<OrderItem>(o => this.ExecuteCancelOrderItemCommand(o), o => this.CanExecuteCancelOrderItemCommand(o));
                this.UnCancelOrderItemCommand = new CommandBase<OrderItem>(o => this.ExecuteUnCancelOrderItemCommand(o), o => this.CanExecuteUnCancelOrderItemCommand(o));
                this.IncreaseQuantityCommand = new CommandBase<OrderItem>(o => this.ExecuteIncreaseQuantityCommand(o), o => this.CanExecuteIncreaseQuantityCommand(o));
                this.DecreaseQuantityCommand = new CommandBase<OrderItem>(o => this.ExecuteDecreaseQuantityCommand(o), o => this.CanExecuteDecreaseQuantityCommand(o));

                this.SelectProductGroupCommand = new CommandBase<ProductGroup>(o => this.ExecuteSelectProductGroupCommand(o), o => this.CanExecuteSelectProductGroupCommand(o));
                this.ShowCancelledProductCommand = new CommandBase<object>(o => this.ExecuteShowCancelledProductCommand());

                this.DisplayName = "Create Order";

                // temporarily
                Item.CreatorId = GlobalObjects.SystemUser.Id;
                Item.CreatorUser = GlobalObjects.SystemUser;
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }

        }

        public OrderViewModel(IMessageBoxService messageBoxService, Order item, IOrderManager manager, ITableManager tableManager) : base(messageBoxService, item, manager)
        {
            // do initialization
            try
            {
                if (tableManager == null)
                {
                    throw new ArgumentNullException("tableManager");
                }
                _tableManager = tableManager;
                // populate the list of tables
                this.FreeTableList = _tableManager.GetFreeTables();
                // aslo need add current table into top of free table list
                this.FreeTableList.Insert(0, this.Item.Table);

                _productGroupManager = new ProductGroupManager();
                this.ProductGroupList = _productGroupManager.GetList();
                // add one item, in case user select all group
                this.ProductGroupList.Insert(0, new ProductGroup
                {
                    Id = -1,
                    Code = "ALL",
                    Name = "All",
                    Description = "List of all products"
                });

                _productManager = new ProductManager();
                this.AvailableProductList = _productManager.GetList();

                // initialize command
                this.AddOrderItemCommand = new CommandBase<Product>(o => this.ExecuteAddOrderItemCommand(o), o => this.CanExecuteAddOrderItemCommand(o));
                this.CancelOrderItemCommand = new CommandBase<OrderItem>(o => this.ExecuteCancelOrderItemCommand(o), o => this.CanExecuteCancelOrderItemCommand(o));
                this.UnCancelOrderItemCommand = new CommandBase<OrderItem>(o => this.ExecuteUnCancelOrderItemCommand(o), o => this.CanExecuteUnCancelOrderItemCommand(o));
                this.IncreaseQuantityCommand = new CommandBase<OrderItem>(o => this.ExecuteIncreaseQuantityCommand(o), o => this.CanExecuteIncreaseQuantityCommand(o));
                this.DecreaseQuantityCommand = new CommandBase<OrderItem>(o => this.ExecuteDecreaseQuantityCommand(o), o => this.CanExecuteDecreaseQuantityCommand(o));

                this.SelectProductGroupCommand = new CommandBase<ProductGroup>(o => this.ExecuteSelectProductGroupCommand(o), o => this.CanExecuteSelectProductGroupCommand(o));
                this.ShowCancelledProductCommand = new CommandBase<object>(o => this.ExecuteShowCancelledProductCommand());

                this.DisplayName = "Edit Order: " + this.Item.Table.Name;

                CollectionViewSource.GetDefaultView(Item.OrderItems).Filter = o => IsShowCancelledProduct
                                                || ((OrderItem)o).IsCancelled == false
                                                || (((OrderItem)o).IsCancelled == true && ((OrderItem)o).IsDirty == true);

                //if (GlobalObjects.SystemUser.StartEditOrder(Item))
                //{ }
                if (Item.LockState == false)
                {
                    // lock order, prevent other user editting order.
                    Item.LockKeeperId = GlobalObjects.SystemUser.Id;
                    Item.LockKeeper = GlobalObjects.SystemUser;

                    // below code will do inside lock function
                    //Item.LockState = true;
                    (ModelManager as IOrderManager).Lock(Item);
                }
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }
        #endregion // Constructors

        #region Public Properties

        /// <summary>
        /// A flag if enable show cancelled items of order or not.
        /// </summary>
        public bool IsShowCancelledProduct
        {
            get { return _isShowCancelledProduct; }
            set
            {
                _isShowCancelledProduct = value;
                NotifyPropertyChanged(() => IsShowCancelledProduct);
            }
        }

        /// <summary>
        /// Selected order item to synchronize with UI
        /// </summary>
        public OrderItem SelectedOrderItem
        {
            get { return _selectedOrderItem; }
            set
            {
                _selectedOrderItem = value;
                NotifyPropertyChanged(() => SelectedOrderItem);
            }
        }

        /// <summary>
        /// The list of free tables that order can be matched.
        /// </summary>
        public List<Table> FreeTableList
        {
            get { return _freeTableList; }
            set { _freeTableList = value; NotifyPropertyChanged(() => FreeTableList); }
        }

        /// <summary>
        /// The list of products in the store for adding product into order.
        /// </summary>
        public List<Product> AvailableProductList
        {
            get { return _availableProductList; }
            set { _availableProductList = value; NotifyPropertyChanged(() => AvailableProductList); }
        }

        /// <summary>
        /// The list of product groups, for filtering when user find product.
        /// </summary>
        public List<ProductGroup> ProductGroupList
        {
            get { return _productGroupList; }
            set { _productGroupList = value; NotifyPropertyChanged(() => ProductGroupList); }
        }

        #endregion // Public Properties


        #region IDataErrorInfo Members

        /*string IDataErrorInfo.Error
        {
            get { return (this.Item as IDataErrorInfo).Error; }
        }

        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                string error = null;
                error = (this.Item as IDataErrorInfo)[propertyName];

                // Dirty the commands registered with CommandManager,
                // such as our Save command, so that they are queried
                // to see if they can execute now.
                CommandManager.InvalidateRequerySuggested();

                return error;
            }
        }*/

        #endregion // IDataErrorInfo Members


        #region Command Properties

        /// <summary>
        /// Gets or sets the add order item command.
        /// </summary>
        public CommandBase<Product> AddOrderItemCommand { get; set; }

        /// <summary>
        /// Gets or sets the cancel order item command.
        /// </summary>
        public CommandBase<OrderItem> CancelOrderItemCommand { get; set; }

        /// <summary>
        /// Gets or sets the cancel order item command.
        /// </summary>
        public CommandBase<OrderItem> UnCancelOrderItemCommand { get; set; }

        /// <summary>
        /// Gets or sets the select group command
        /// </summary>
        public CommandBase<ProductGroup> SelectProductGroupCommand { get; set; }

        /// <summary>
        /// Gets or sets the command: show cancelled product in order item list
        /// </summary>
        public CommandBase<object> ShowCancelledProductCommand { get; set; }

        /// <summary>
        /// Gets or sets the command: increase quantity of item
        /// </summary>
        public ICommand IncreaseQuantityCommand { get; set; }

        /// <summary>
        /// Gets or sets the command: decrease quantity of item
        /// </summary>
        public ICommand DecreaseQuantityCommand { get; set; }

        #endregion // Command Properties


        #region  Private Method Members

        /// <summary>
        /// Execute AddOrderItemCommand
        /// </summary>
        /// <param name="item">Selected item to be processed.</param>
        private void ExecuteAddOrderItemCommand(Product product)
        {
            try
            {
                if (product == null)
                    throw new ArgumentNullException("product");

                SelectedOrderItem = this.Item.OrderItems.Where(o => o.ProductId == product.Id && o.State == OrderItemState.Ordered && o.IsCancelled == false).FirstOrDefault();
                if (SelectedOrderItem != null)
                {
                    SelectedOrderItem.Quantity++;
                }
                else
                {
                    OrderItem newOrderItem = this.Item.OrderItems.AddNew();
                    //OrderItem newOrderItem = OrderItem.NewObject();
                    //newOrderItem.MarkAsChild();
                    if (this.Item.OrderItems.Count == 0)
                        newOrderItem.Sequence = 1;
                    else
                        newOrderItem.Sequence = (byte)(this.Item.OrderItems.Max(o => o.Sequence) + 1);
                    newOrderItem.State = 0;
                    newOrderItem.OrderId = this.Item.Id;
                    newOrderItem.ProductId = product.Id;
                    newOrderItem.Quantity = 1;
                    newOrderItem.ProductInfo = _productManager.GetById(product.Id);
                    //this.Item.Recipes.Add(newOrderItem);

                    SelectedOrderItem = newOrderItem;
                }
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        /// <summary>
        /// Execute CancelOrderItemCommand
        /// </summary>
        /// <param name="item">Selected item to be processed.</param>
        private void ExecuteCancelOrderItemCommand(OrderItem item)
        {
            try
            {
                if (item == null)
                    throw new ArgumentNullException("item");

                if (item.IsNew == true)
                {
                    //this.Item.Recipes.Remove(item);
                    if (this.Item.OrderItems.Contains(item))
                    {
                        this.Item.OrderItems.Remove(item);

                        // correct sequence number of items when remove
                        for (int i = item.Sequence - 1; i < this.Item.OrderItems.Count; i++)
                        {
                            this.Item.OrderItems[i].Sequence = (byte)(i + 1);
                        }

                        /*List<OrderItem> list = (List<OrderItem>)(this.Item.OrderItems as IEditableCollection).GetDeletedList();

                        string message = "Deleted List: \n";
                        foreach (OrderItem orderItem in list)
                        {
                            message += orderItem.ProductInfo.Name + "\n";
                        }
                        this.MessageBoxService.ShowInformation(message);*/

                    }
                    else
                    {
                        this.MessageBoxService.ShowInformation("This item is not in the list.");
                    }
                }
                else
                {
                    // need CancelEdit() or not, for recover original data before mark cancelled
                    (item as IEditableObject).CancelEdit();
                    (item as IEditableObject).BeginEdit();

                    // and set cancelled flag
                    item.MarkCancelled();
                    /*// set quantity to zero, no remove item, keep old item and its sequence number in list
                    SelectedOrderItemRow.EdittedQuantity = -SelectedOrderItemRow.Quantity;
                    SelectedOrderItemRow.UpdatedQuantity = 0;*/
                }
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        /// <summary>
        /// Execute UnCancelOrderItemCommand
        /// </summary>
        /// <param name="item">Selected item to be processed.</param>
        private void ExecuteUnCancelOrderItemCommand(OrderItem item)
        {
            try
            {
                if (item == null)
                    throw new ArgumentNullException("item");

                if (item.IsNew == true)
                {
                    // do nothing, maybe this case not happen
                }
                else
                {
                    if (item.IsCancelled && item.IsDirty)
                    {
                        // need CancelEdit() or not, for recover old data
                        (item as IEditableObject).CancelEdit();
                        (item as IEditableObject).BeginEdit();

                        // and reset cancelled flag
                        item.UnMarkCancelled();
                    }
                    /*// set quantity to zero, no remove item, keep old item and its sequence number in list
                    SelectedOrderItemRow.EdittedQuantity = -SelectedOrderItemRow.Quantity;
                    SelectedOrderItemRow.UpdatedQuantity = 0;*/
                }
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        /// <summary>
        /// ExecuteIncreaseQuantityCommand
        /// </summary>
        private void ExecuteIncreaseQuantityCommand(OrderItem item)
        {
            try
            {
                if (item == null)
                    throw new ArgumentNullException("item");
                item.EdittedQuantity++;

                /*if (!item.IsDirty)
                {
                    MessageBoxService.ShowInformation("Not dirty object after edit");
                }*/
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
            /*try
            {
                SelectedOrderItemRow.EdittedQuantity++;
                SelectedOrderItemRow.UpdatedQuantity = SelectedOrderItemRow.Quantity + SelectedOrderItemRow.EdittedQuantity;
            }
            catch (Exception ex)
            {
                Logger.Error("ex.Message");
                messageBoxService.ShowError("ExecuteIncreaseQuantityCommand " + ex.Message);
            }*/
        }

        /// <summary>
        /// ExecuteIncreaseQuantityCommand
        /// </summary>
        private void ExecuteDecreaseQuantityCommand(OrderItem item)
        {
            try
            {
                if (item == null)
                    throw new ArgumentNullException("item");

                item.Quantity--;
                /*if (!item.IsDirty)
                {
                    MessageBoxService.ShowInformation("Not dirty object after edit");
                }*/
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
            /*try
            {
                SelectedOrderItemRow.EdittedQuantity--;
                if (SelectedOrderItemRow.IsNewItem == true)
                {
                    if (SelectedOrderItemRow.EdittedQuantity == 0)
                    {
                        OrderItemRows.Remove(SelectedOrderItemRow);

                        // correct sequence number of items when remove
                        for (int i = 0; i < OrderItemRows.Count; i++)
                        {
                            OrderItemRows[i].Sequence = (byte)(i + 1);
                        }
                    }
                    //SelectedOrderItemRow.EdittedQuantity = 1;
                }
                else
                {
                    if (SelectedOrderItemRow.EdittedQuantity + SelectedOrderItemRow.Quantity < 0)
                        SelectedOrderItemRow.EdittedQuantity = -SelectedOrderItemRow.Quantity;
                }

                SelectedOrderItemRow.UpdatedQuantity = SelectedOrderItemRow.Quantity + SelectedOrderItemRow.EdittedQuantity;
            }
            catch (Exception ex)
            {
                Logger.Error("ex.Message");
                messageBoxService.ShowError("ExecuteDecreaseQuantityCommand " + ex.Message);
            }*/
        }

        /// <summary>
        /// Execute SelectProductGroupCommand
        /// </summary>
        /// <param name="item">Selected item to be processed.</param>
        private void ExecuteSelectProductGroupCommand(ProductGroup item)
        {
            try
            {
                if (item == null)
                    throw new ArgumentNullException("item");

                // filter products base on seleted group
                CollectionViewSource.GetDefaultView(AvailableProductList).Filter = o => item.Id == -1 || ((Product)o).Group.Id == item.Id;
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        /// <summary>
        /// ExecuteShowCancelledProductCommand
        /// </summary>
        private void ExecuteShowCancelledProductCommand()
        {
            CollectionViewSource.GetDefaultView(Item.OrderItems).Filter = o => IsShowCancelledProduct
                                                || ((OrderItem)o).IsCancelled == false
                                                || (((OrderItem)o).IsCancelled == true && ((OrderItem)o).IsDirty == true);
        }
        /// <summary>
        /// Determines whether the AddOrderItemCommand can be executed.
        /// </summary>
        /// <param name="item">The item that this function check on.</param>
        /// <returns>
        ///     <c>true</c> if the AddOrderItemCommand can be executed; otherwise, <c>false</c>.
        /// </returns>
        private bool CanExecuteAddOrderItemCommand(Product item)
        {
            return item != null;
        }

        /// <summary>
        /// Determines whether the CancelOrderItemCommand can be executed.
        /// </summary>
        /// <param name="item">The item that this function check on.</param>
        /// <returns>
        ///     <c>true</c> if the CancelOrderItemCommand can be executed; otherwise, <c>false</c>.
        /// </returns>
        private bool CanExecuteCancelOrderItemCommand(OrderItem item)
        {
            return item != null && item.IsCancelled != true;
        }

        /// <summary>
        /// Determines whether the UnCancelOrderItemCommand can be executed.
        /// </summary>
        /// <param name="item">The item that this function check on.</param>
        /// <returns>
        ///     <c>true</c> if the UnCancelOrderItemCommand can be executed; otherwise, <c>false</c>.
        /// </returns>
        private bool CanExecuteUnCancelOrderItemCommand(OrderItem item)
        {
            // item is cancelled in database, can not undo
            return item != null && item.IsCancelled && item.IsDirty;
        }

        /// <summary>
        /// Logic to determine if ExecuteIncreaseQuantityCommand can execute
        /// </summary>
        private bool CanExecuteIncreaseQuantityCommand (OrderItem item)
        {
            // can not edit quantity when item is marked cancelled.
            return item != null && item.IsCancelled != true;
        }

        /// <summary>
        /// Logic to determine if ExecuteDecreaseQuantityCommand can execute
        /// </summary>
        private bool CanExecuteDecreaseQuantityCommand(OrderItem item)
        {
            // can not edit quantity when item is marked cancelled.
            return item != null && item.IsCancelled != true;
        }

        /// <summary>
        /// Determines whether the SelectProductGroupCommand can be executed.
        /// </summary>
        /// <param name="item">The item that this function check on.</param>
        /// <returns>
        ///     <c>true</c> if the SelectProductGroupCommand can be executed; otherwise, <c>false</c>.
        /// </returns>
        private bool CanExecuteSelectProductGroupCommand(ProductGroup item)
        {
            return item != null;
        }

        #endregion // Private Method Members


        #region Override Method Members

        /// <summary>
        /// Executes the save command.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        protected override void ExecuteSaveCommand()
        {
            // LockState was false before
            //(ModelManager as IOrderManager).Unlock(Item);
            base.ExecuteSaveCommand();
        }

        /// <summary>
        /// Executes the cancel command.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        protected override void ExecuteCancelCommand()
        {
            if (!Item.IsNew)
                (ModelManager as IOrderManager).Unlock(Item);
            base.ExecuteCancelCommand();
        }

        /// <summary>
        /// Executes the edit command.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        protected override void ExecuteEditItemCommand()
        {
            try
            {
                (this.Item as IEditableObject).BeginEdit();
                foreach(OrderItem orderItem in this.Item.OrderItems)
                {
                    (orderItem as IEditableObject).BeginEdit();
                }
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        #endregion // Override Method Members
    }
}
