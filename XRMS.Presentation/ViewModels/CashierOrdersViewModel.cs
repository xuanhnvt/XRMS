using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Input;
using System.Collections.ObjectModel;

using System.ComponentModel.Composition;
using MEFedMVVM.ViewModelLocator;

//using DeepEqual;
//using DeepEqual.Syntax;

using Cinch;
using XRMS.Business;
using XRMS.Business.Models;
using XRMS.Business.Services;
using XRMS.Libraries.BaseClasses;
using XRMS.Libraries.Helpers;
using XRMS.Libraries.MVVM;


namespace XRMS.Presentation.ViewModels
{
    // supporting class for view
    public class Discount
    {
        public string Display { get; set; }
        public byte Value { get; set; }

        public Discount(string display, byte value)
        {
            Display = display;
            Value = value;
        }
    }

    public class ReportGroupBy
    {
        public string Display { get; set; }
        public string PropertyName { get; set; }

        public ReportGroupBy(string display, string value)
        {
            Display = display;
            PropertyName = value;
        }
    }

    /// This is a workspace type ViewModel, which is created by some code in the <c>MainWindowViewModel</c>
    /// and the DataTemplate in the <c>MainWindow.xaml</c>
    /// 
    /// This ViewModel is also able to operate in design mode by using this ViewModel at runtime or by using
    /// data provided by a Design time ViewModel that inherits from this one.
    /// 
    /// This ViewModel also demonstrates EventToCommand where the XAML fires an Command in this ViewModel
    /// based on some RoutedEvent in the XAML.
    [ExportViewModel("CashierOrdersViewModel")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class CashierOrdersViewModel : ListViewModelBase<Order>
    {
        #region Private Data Members
        private List<ReportGroupBy> _groupByList = new List<ReportGroupBy>();
        private ReportGroupBy _selectedGroupBy;
        private List<ReportOrderItemEdition> _reportEditionOrderItemList = new List<ReportOrderItemEdition>();

        private bool _isShowCancelledProduct;
        private ITableManager _tableManager;
        private ObservableCollection<Table> _tableList;

        private IReportOrderItemEditionManager _reportOrderEdition;

        #endregion // Private Data Members

        #region Constructors
        [ImportingConstructor]
        public CashierOrdersViewModel(IMessageBoxService messageBoxService, IUIVisualizerService uiVisualizerService,
                                        IOrderManager orderManager, ITableManager tableManager, IReportOrderItemEditionManager reportOrderEdition) : base(messageBoxService, uiVisualizerService, orderManager)
        {
            // do initialization
            try
            {
                if (tableManager == null)
                {
                    throw new ArgumentNullException("tableManager");
                }
                _tableManager = tableManager;

                this.DisplayName = "Orders Management";
                TableList = new ObservableCollection<Table>(_tableManager.GetList().OrderBy(o => o.State));

                if (reportOrderEdition == null)
                {
                    throw new ArgumentNullException("reportOrderEdition");
                }
                _reportOrderEdition = reportOrderEdition;

                if (SelectedItem != null)
                {
                    (ModelManager as IOrderManager).FetchOrderItems(SelectedItem);
                    CollectionViewSource.GetDefaultView(SelectedItem.OrderItems).Filter = o => IsShowCancelledProduct
                                                 || ((OrderItem)o).IsCancelled == false;

                    /*ReportEditionOrderItemList = _reportOrderEdition.GetEdititonReportOfOrder(SelectedItem);
                    CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ReportEditionOrderItemList);
                    PropertyGroupDescription groupDescription = new PropertyGroupDescription(SelectedGroupBy.PropertyName);
                    view.GroupDescriptions.Clear();
                    view.GroupDescriptions.Add(groupDescription);*/
                }

                // initialize command
                this.RefreshTablesCommand = new CommandBase<Table>(o => this.ExecuteRefreshTablesCommand(o));
                this.ShowCancelledProductCommand = new CommandBase<object>(o => this.ExecuteShowCancelledProductCommand());
                this.SelectGroupByCommand = new CommandBase<object>(o => this.ExecuteSelectGroupByCommand());

                this.CheckoutCommand = new CommandBase<Order>(o => this.ExecuteCheckoutCommand(o), o => this.CanExecuteCheckoutCommand(o));
                this.CancelCommand = new CommandBase<Order>(o => this.ExecuteCancelCommand(o), o => this.CanExecuteCancelCommand(o));
                this.BillCommand = new CommandBase<Order>(o => this.ExecuteBillCommand(o), o => this.CanExecuteBillCommand(o));
                this.PrintCommand = new CommandBase<Order>(o => this.ExecutePrintCommand(o), o => this.CanExecutePrintCommand(o));

                this.SelectDiscountCommand = new CommandBase<Order>(o => this.ExecuteSelectDiscountCommand(o), o => this.CanExecuteSelectDiscountCommand(o));
                this.ServiceChargeCommand = new CommandBase<Order>(o => this.ExecuteServiceChargeCommand(o), o => this.CanExecuteServiceChargeCommand(o));
                this.EnableVatCommand = new CommandBase<Order>(o => this.ExecuteEnableVatCommand(o), o => this.CanExecuteEnableVatCommand(o));
                
                // available discount list
                DiscountList = new List<Discount>();
                DiscountList.Add(new Discount("0%", 0));
                DiscountList.Add(new Discount("5%", 5));
                DiscountList.Add(new Discount("10%", 10));
                DiscountList.Add(new Discount("15%", 15));
                DiscountList.Add(new Discount("20%", 20));
                DiscountList.Add(new Discount("25%", 25));
                DiscountList.Add(new Discount("30%", 30));

                GroupByList.Add(new ReportGroupBy("Thu tu goi", "EditionInfo"));
                GroupByList.Add(new ReportGroupBy("Mon an", "ProductName"));
                SelectedGroupBy = GroupByList[0];

                ReportEditionOrderItemList = _reportOrderEdition.GetEdititonReportOfOrder(SelectedItem);
                    CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ReportEditionOrderItemList);
                    PropertyGroupDescription groupDescription = new PropertyGroupDescription(SelectedGroupBy.PropertyName);
                    view.GroupDescriptions.Clear();
                    view.GroupDescriptions.Add(groupDescription);

                //Mediator.Instance.RegisterHandler<T>("Updated" + typeof(T).Name + "Successfully", HandleReceivedMessage);
                //Mediator.Instance.RegisterHandler<T>("Created" + typeof(T).Name + "Successfully", HandleReceivedMessage);

                Mediator.Instance.Register(this);
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }
        #endregion // Constructors


        #region Public Properties

        /// <summary>
        /// A table list.
        /// </summary>
        public ObservableCollection<Table> TableList
        {
            get { return _tableList; }
            set { _tableList = value; NotifyPropertyChanged(() => TableList); }
        }

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
        /// A available discount list.
        /// </summary>
        public List<Discount> DiscountList { get; set; }

        /// <summary>
        /// The group selection list for display item report
        /// </summary>
        public List<ReportGroupBy> GroupByList
        {
            get { return _groupByList; }
            set
            {
                if (_groupByList != value)
                {
                    _groupByList = value;
                    NotifyPropertyChanged(() => GroupByList);
                }
            }
        }

        /// <summary>
        /// The group selection for display item report
        /// </summary>
        public ReportGroupBy SelectedGroupBy
        {
            get { return _selectedGroupBy; }
            set
            {
                if (_selectedGroupBy != value)
                {
                    _selectedGroupBy = value;
                    NotifyPropertyChanged(() => SelectedGroupBy);
                }
            }
        }



        /// <summary>
        /// A order item edition list.
        /// </summary>
        public List<ReportOrderItemEdition> ReportEditionOrderItemList
        {
            get { return _reportEditionOrderItemList; }
            set
            {
                _reportEditionOrderItemList = value;
                NotifyPropertyChanged(() => ReportEditionOrderItemList);
            }
        }

        #endregion // Public Properties


        #region Command Properties

        /// <summary>
        /// Gets or sets the select material command.
        /// </summary>
        public CommandBase<Table> RefreshTablesCommand { get; set; }

        /// <summary>
        /// Gets or sets the command: show cancelled product in order item list
        /// </summary>
        public CommandBase<object> ShowCancelledProductCommand { get; set; }

        /// <summary>
        /// Gets or sets the command: check out selected order
        /// </summary>
        public CommandBase<Order> CheckoutCommand { get; set; }

        /// <summary>
        /// Gets or sets the command: bill selected order
        /// </summary>
        public CommandBase<Order> BillCommand { get; set; }

        /// <summary>
        /// Gets or sets the command: print selected order
        /// </summary>
        public CommandBase<Order> PrintCommand { get; set; }

        /// <summary>
        /// Gets or sets the command: cancel selected order
        /// </summary>
        public CommandBase<Order> CancelCommand { get; set; }

        /// <summary>
        /// Returns the command that, when invoked, calculate discount price for customer
        /// </summary>
        public CommandBase<Order> SelectDiscountCommand { get; set; }

        /// <summary>
        /// Gets or sets the command: set service charge for selected order
        /// </summary>
        public CommandBase<Order> ServiceChargeCommand { get; set; }

        /// <summary>
        /// Returns the command that, when invoked, calculate discount price for customer
        /// </summary>
        public CommandBase<Order> EnableVatCommand { get; set; }

        /// <summary>
        /// Returns the command that, when invoked, calculate discount price for customer
        /// </summary>
        public CommandBase<object> SelectGroupByCommand { get; set; }

        #endregion // Command Properties

        #region Override Methods
        /// <summary>
        /// Determines whether the Edit Item command can be executed.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        ///   <c>true</c> if the Edit Item command can be executed; otherwise, <c>false</c>.
        /// </returns>
        protected override bool CanExecuteEditItemCommand(Order obj)
        {
            //return (obj != null && obj.LockState != true);
            return obj != null && (obj.LockState != true || obj.LockKeeper.Id == GlobalObjects.SystemUser.Id) && obj.State < OrderState.Billed;
        }

        /// <summary>
        /// Get item list from database
        /// </summary>
        protected override List<Order> GetItems()
        {
            try
            {
                return ModelManager.GetList().OrderByDescending<Order, OrderState>(o => o.State).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Create new item and add into database
        /// </summa
        protected override void CreateNewItem()
        {
            try
            {
                OrderViewModel viewModel = new OrderViewModel(this.MessageBoxService, (IOrderManager)ModelManager, _tableManager);

                // open dialog and return result when it is closed
                bool? result = this.UIVisualizerService.ShowDialog("OrderPopup", viewModel);

                // code to check result
                if (result == true)
                {
                    // refresh item list
                    RefreshTablesCommand.Execute(null);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Edit an existing item and update in database
        /// </summary>
        protected override void EditItem(Order item)
        {
            try
            {
                OrderViewModel viewModel = new OrderViewModel(this.MessageBoxService, item, (IOrderManager)ModelManager, _tableManager);

                // open dialog and return result when it is closed
                bool? result = this.UIVisualizerService.ShowDialog("OrderPopup", viewModel);

                // code to check result
                if (result == true)
                {
                    // refresh item list
                    RefreshTablesCommand.Execute(null);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Delete item from database
        /// </summary>
        protected override void DeleteItem(Order item)
        {
            try
            {
                // need warning for confirmation
                // ...
                if (this.MessageBoxService.ShowYesNo(
                "Would you like to remove order on table \"" + item.Table.Name + "\" from list?",
                CustomDialogIcons.Question) == CustomDialogResults.Yes)
                {
                    bool result = ModelManager.Delete(item);
                    if (result)
                        this.MessageBoxService.ShowInformation("Successfully deleted order");

                    // refresh item list
                    this.ExecuteRefreshCommand();
                    // refresh table list
                    RefreshTablesCommand.Execute(null);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Process seleted item after user change selection.
        /// </summary>
        protected override void ProcessSelectedItem(Order selectedItem)
        {
            try
            {
                
                //MessageBoxService.ShowInformation("Selected Order: " + selectedItem.Code);
                (ModelManager as IOrderManager).FetchOrderItems(selectedItem);
                if (this.SelectedItem != null)
                {
                    CollectionViewSource.GetDefaultView(this.SelectedItem.OrderItems).Filter = o => IsShowCancelledProduct
                                                    || ((OrderItem)o).IsCancelled == false;

                    ReportEditionOrderItemList = _reportOrderEdition.GetEdititonReportOfOrder(SelectedItem);
                    CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ReportEditionOrderItemList);
                    PropertyGroupDescription groupDescription = new PropertyGroupDescription(SelectedGroupBy.PropertyName);
                    view.GroupDescriptions.Clear();
                    view.GroupDescriptions.Add(groupDescription);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnDispose()
        {
            //Mediator.Instance.UnregisterHandler<T>("Updated" + typeof(T).Name + "Successfully", HandleReceivedMessage);
            //Mediator.Instance.UnregisterHandler<T>("Created" + typeof(T).Name + "Successfully", HandleReceivedMessage);
            Mediator.Instance.Unregister(this);
            base.OnDispose();
        }
        #endregion // Override Methods


        #region Private Method Members
        /// <summary>
        /// ExecuteShowCancelledProductCommand
        /// </summary>
        private void ExecuteShowCancelledProductCommand()
        {
            try
            {
                if (this.SelectedItem != null)
                {
                    CollectionViewSource.GetDefaultView(SelectedItem.OrderItems).Filter = o => IsShowCancelledProduct
                                                || ((OrderItem)o).IsCancelled == false;
                }
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        /// <summary>
        /// Execute RefreshTablesCommand
        /// </summary>
        /// <param name="item">Selected item to be processed.</param>
        private void ExecuteRefreshTablesCommand(Table item)
        {
            try
            {
                TableList = new ObservableCollection<Table>(_tableManager.GetList().OrderBy(o => o.State));
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        /// <summary>
        /// Execute CheckoutCommand
        /// </summary>
        /// <param name="item">Selected item to be processed.</param>
        private void ExecuteCheckoutCommand(Order item)
        {
            try
            {
                (ModelManager as IOrderManager).CheckOutOrder(GlobalObjects.SystemUser, item);
                SelectedItem = this.Items[0];

                // refresh item list
                this.ExecuteRefreshCommand();
                // refresh table list
                RefreshTablesCommand.Execute(null);
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        /// <summary>
        /// Determines whether the CheckoutCommand can be executed.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        ///   <c>true</c> if the CheckoutCommand can be executed; otherwise, <c>false</c>.
        /// </returns>
        private bool CanExecuteCheckoutCommand(Order obj)
        {
            return obj != null && obj.State == OrderState.Billed;
        }

        /// <summary>
        /// Execute BillCommand
        /// </summary>
        /// <param name="item">Selected item to be processed.</param>
        private void ExecuteBillCommand(Order item)
        {
            try
            {
                if (this.MessageBoxService.ShowYesNo(
                        "Would you like to bill order \"" + item.Code + "\"?",
                        CustomDialogIcons.Question) == CustomDialogResults.Yes)
                {
                    KeyPadViewModel viewModel = new KeyPadViewModel("Cash");

                    bool? result = this.UIVisualizerService.ShowDialog("KeyPadPopup", viewModel);

                    if (result.HasValue && result.Value)
                    {
                        item.Cash = Convert.ToInt64(viewModel.ReturnedValue);

                        if (item.Change < 0)
                        {
                            MessageBoxService.ShowInformation(
                                "Payment for order " + item.Code + " is not enough !!! Please pay greater amount! Thank you.");
                        }
                        else
                        {
                            (ModelManager as IOrderManager).BillOrder(GlobalObjects.SystemUser, item);
                            //SelectedItem = this.Items[0];
                        }

                        // refresh item list
                        this.ExecuteRefreshCommand();
                        // refresh table list
                        RefreshTablesCommand.Execute(null);
                    }
                }
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        /// <summary>
        /// Determines whether the BillCommand can be executed.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        ///   <c>true</c> if the BillCommand can be executed; otherwise, <c>false</c>.
        /// </returns>
        private bool CanExecuteBillCommand(Order obj)
        {
            return obj != null && obj.PrintCount != 0 && obj.State == OrderState.Printed;
        }

        /// <summary>
        /// Execute PrintCommand
        /// </summary>
        /// <param name="item">Selected item to be processed.</param>
        private void ExecutePrintCommand(Order item)
        {
            try
            {
                (ModelManager as IOrderManager).UpdatePrintCount(GlobalObjects.SystemUser, item);
                //SelectedItem = this.Items[0];

                // refresh item list
                this.ExecuteRefreshCommand();
                // refresh table list
                RefreshTablesCommand.Execute(null);
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        /// <summary>
        /// Determines whether the PrintCommand can be executed.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        ///   <c>true</c> if the PrintCommand can be executed; otherwise, <c>false</c>.
        /// </returns>
        private bool CanExecutePrintCommand(Order obj)
        {
            return obj != null && obj.State < OrderState.Billed;
        }

        /// <summary>
        /// Execute ServiceChargeCommand
        /// </summary>
        /// <param name="item">Selected item to be processed.</param>
        private void ExecuteServiceChargeCommand(Order item)
        {
            try
            {
                KeyPadViewModel viewModel = new KeyPadViewModel("Service Charge");
                bool? result = UIVisualizerService.ShowDialog("KeyPadPopup", viewModel);

                if (result.HasValue && result.Value)
                {
                    item.ServiceCharge = Convert.ToInt64(viewModel.ReturnedValue);
                    (ModelManager as IOrderManager).UpdateServiceCharge(item);

                    // refresh item list
                    this.ExecuteRefreshCommand();
                    // refresh table list
                    RefreshTablesCommand.Execute(null);
                }
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        /// <summary>
        /// Determines whether the ServiceChargeCommand can be executed.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        ///   <c>true</c> if the ServiceChargeCommand can be executed; otherwise, <c>false</c>.
        /// </returns>
        private bool CanExecuteServiceChargeCommand(Order obj)
        {
            return obj != null && obj.State < OrderState.Billed;
        }

        /// <summary>
        /// Execute SelectDiscountCommand
        /// </summary>
        /// <param name="item">Selected item to be processed.</param>
        private void ExecuteSelectDiscountCommand(Order obj)
        {
            try
            {
                if (SelectedItem == null)
                    throw new ArgumentNullException("No have Order argument.");
                (ModelManager as IOrderManager).UpdateDiscountPercent(SelectedItem);
                //MessageBoxService.ShowInformation(SelectedItem.Code + " Discount changed." + SelectedItem.DiscountPercent.ToString());

                // re-fetch data into selected item
                //(ModelManager as IOrderManager).FetchOrderItems(SelectedItem);

                // refresh item list
                //this.ExecuteRefreshCommand();
                // refresh table list
                //RefreshTablesCommand.Execute(null);
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        /// <summary>
        /// Determines whether the SelectDiscountCommand can be executed.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        ///   <c>true</c> if the SelectDiscountCommand can be executed; otherwise, <c>false</c>.
        /// </returns>
        private bool CanExecuteSelectDiscountCommand(Order obj)
        {
            return (SelectedItem != null && SelectedItem.State < OrderState.Billed);
        }

        /// <summary>
        /// Execute ExecuteEnableVatCommand
        /// </summary>
        /// <param name="item">Selected item to be processed.</param>
        private void ExecuteEnableVatCommand(Order obj)
        {
            try
            {
                if (SelectedItem == null)
                    throw new ArgumentNullException("No have Order argument.");
                (ModelManager as IOrderManager).UpdateVatEnable(SelectedItem);
                //MessageBoxService.ShowInformation(SelectedItem.Code + " Discount changed." + SelectedItem.DiscountPercent.ToString());

                // re-fetch data into selected item
                //(ModelManager as IOrderManager).FetchOrderItems(SelectedItem);

                // refresh item list
                //this.ExecuteRefreshCommand();
                // refresh table list
                //RefreshTablesCommand.Execute(null);
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        /// <summary>
        /// Determines whether the ExecuteEnableVatCommand can be executed.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        ///   <c>true</c> if the ExecuteEnableVatCommand can be executed; otherwise, <c>false</c>.
        /// </returns>
        private bool CanExecuteEnableVatCommand(Order obj)
        {
            return (SelectedItem != null && SelectedItem.State < OrderState.Billed);
        }
        /// <summary>
        /// Execute CancelCommand
        /// </summary>
        /// <param name="item">Selected item to be processed.</param>
        private void ExecuteCancelCommand(Order item)
        {
            try
            {
                (ModelManager as IOrderManager).CancelOrder(GlobalObjects.SystemUser, item);
                SelectedItem = this.Items[0];

                // refresh item list
                this.ExecuteRefreshCommand();
                // refresh table list
                RefreshTablesCommand.Execute(null);
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        /// <summary>
        /// Determines whether the CancelCommand can be executed.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        ///   <c>true</c> if the CancelCommand can be executed; otherwise, <c>false</c>.
        /// </returns>
        private bool CanExecuteCancelCommand(Order obj)
        {
            return obj != null && obj.State < OrderState.Printed;
        }
        
        /// <summary>
        /// Execute CancelCommand
        /// </summary>
        private void ExecuteSelectGroupByCommand()
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ReportEditionOrderItemList);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription(SelectedGroupBy.PropertyName);
            view.GroupDescriptions.Clear();
            view.GroupDescriptions.Add(groupDescription);
        }

        /// <summary>
        /// Mediator callback from MainWindowViewModel
        /// </summary>
        [MediatorMessageSink("MainWindowRequestRefreshingData")]
        private void MainWindowRequestRefreshingDataSink(Boolean dummy)
        {
            // refresh item list
            this.ExecuteRefreshCommand();
            // refresh table list
            RefreshTablesCommand.Execute(null);
        }
        #endregion // Private Method Members
    }
}