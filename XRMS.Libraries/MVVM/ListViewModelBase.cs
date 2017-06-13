using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using System.Linq.Expressions;

using Cinch;
using XRMS.Libraries.BaseObjects;

namespace XRMS.Libraries.MVVM
{
    /// <summary>
    /// Base class for all the view models for list views.
    /// </summary>
    /// <typeparam name="T">The model object the view model has to work on</typeparam>
    public abstract class ListViewModelBase<T> : Cinch.ViewModelBase where T : class
    {
        #region Private fields
        private IGenericManager<T> _modelManager;

        private ObservableCollection<T> items;
        private T selectedItem;
        private bool isRefreshing;
        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets message box service in order to show message to user
        /// </summary>
        public IMessageBoxService MessageBoxService { get; set; }

        /// <summary>
        /// Gets or sets UI service in order to call Popup Window
        /// </summary>
        public IUIVisualizerService UIVisualizerService { get; set; }

        /// <summary>
        /// Gets or sets the model manager service.
        /// </summary>
        public IGenericManager<T> ModelManager
        {
            get { return this._modelManager; }
            set { this._modelManager = value; }
        }

        /// <summary>
        /// Gets or sets the item list.
        /// </summary>
        public ObservableCollection<T> Items
        {
            get { return this.items; }
            set
            {
                this.items = value;

                T itemToSelect = null;

                if (items.Count > 0)
                {
                    if (SelectedItem != null)
                    {
                        itemToSelect = ModelManager.FindItem(SelectedItem, items.ToList());
                    }
                    else
                    {
                        itemToSelect = items[0];
                    }
                }

                NotifyPropertyChanged(() => Items); // note: this makes the SelectedItem property = null so it has to be set again

                SelectedItem = itemToSelect;
            }
        }

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        public virtual T SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (selectedItem != value)
                {
                    selectedItem = value;
                    NotifyPropertyChanged(() => SelectedItem);
                }
            }
        }
        #endregion

        #region Commands Delegates
        /// <summary>
        /// Gets or sets the refresh command.
        /// </summary>
        public CommandBase<T> RefreshCommand { get; set; }

        /// <summary>
        /// Gets or sets the view command.
        /// </summary>
        public CommandBase<T> ViewCommand { get; set; }

        /// <summary>
        /// Gets or sets the new command.
        /// </summary>
        public CommandBase<T> NewCommand { get; set; }

        /// <summary>
        /// Gets or sets the edit command for selected item.
        /// </summary>
        public CommandBase<T> EditItemCommand { get; set; }

        /// <summary>
        /// Gets or sets the edit list command.
        /// </summary>
        public CommandBase<T> EditListCommand { get; set; }

        /// <summary>
        /// Gets or sets the copy command.
        /// </summary>
        public CommandBase<T> CopyCommand { get; set; }

        /// <summary>
        /// Gets or sets the delete command.
        /// </summary>
        public CommandBase<T> DeleteCommand { get; set; }

        /// <summary>
        /// Gets or sets the select item command.
        /// </summary>
        public CommandBase<T> SelectItemCommand { get; set; }

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelListBase&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="messageBoxService">The message box service.</param>
        /// <param name="uiVisualizerService">The UI service.</param>
        public ListViewModelBase(IMessageBoxService messageBoxService, IUIVisualizerService uiVisualizerService, IGenericManager<T> modelManager)
        {
            if (messageBoxService == null)
            {
                throw new ArgumentNullException("messageBoxService");
            }
            this.MessageBoxService = messageBoxService;

            try
            {
                if (uiVisualizerService == null)
                {
                    throw new ArgumentNullException("uiVisualizerService");
                }
                this.UIVisualizerService = uiVisualizerService;

                if (modelManager == null)
                {
                    throw new ArgumentNullException("modelManager");
                }
                this._modelManager = modelManager;

                #region Initialize commands
                this.RefreshCommand = new CommandBase<T>(o => ExecuteRefreshCommand(), o => this.CanExecuteRefreshCommand(o));
                this.NewCommand = new CommandBase<T>(o => this.ExecuteNewCommand(), o => this.CanExecuteNewCommand(o));
                this.EditItemCommand = new CommandBase<T>(o => this.ExecuteEditItemCommand(o), o => this.CanExecuteEditItemCommand(o));
                this.DeleteCommand = new CommandBase<T>(o => this.ExecuteDeleteCommand(o), o => this.CanExecuteDeleteCommand(o));
                //this.ViewCommand = new CommandBase<T>(o => { ViewItemDetail(this.SelectedItem.Id); }, CanExecuteViewCommand);

                //this.CopyCommand = new CommandBase<T>(o => { CopyItemDetail(this.SelectedItem.Id); }, CanExecuteCopyCommand);
                //this.DbLinkCommand = new CommandBase<T>(o => { ViewLinkItems(null); }, o => { return SelectedItem != null; });

                this.SelectItemCommand = new CommandBase<T>(o => this.ExecuteSelectItemCommand(o), o => this.CanExecuteSelectItemCommand(o));
                #endregion

                // sets the default value for some status fields
                isRefreshing = false;

                //Mediator.Instance.Register(this);
                Mediator.Instance.RegisterHandler<T>("Updated" + typeof(T).Name + "Successfully", HandleReceivedMessage);
                Mediator.Instance.RegisterHandler<T>("Created" + typeof(T).Name + "Successfully", HandleReceivedMessage);

                // get list
                this.Items = new System.Collections.ObjectModel.ObservableCollection<T>(GetItems());
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }
        #endregion

        #region Command Methods
        /// <summary>
        /// Refreshes the items.
        /// </summary>
        protected virtual void ExecuteRefreshCommand()
        {
            isRefreshing = true;
            try
            {
                List<T> items = null;
                try
                {
                    T selItem = SelectedItem; // To reselect the currently selected item after refresh

                    items = this.GetItems(); // To be implemented in the derived classes

                    if (selItem != null)
                    {
                        // Tries to select the item in the new list with the same key. 
                        SelectedItem = ModelManager.FindItem(selItem, items);
                    }
                    else
                    {
                        SelectedItem = null;
                    }

                    // If the selected item was null or it was not found in the new list, selects the first item, if any
                    if ((SelectedItem == null) && (items.Count > 0))
                        SelectedItem = items[0];

                    if (SelectedItem != null)
                        this.ExecuteSelectItemCommand(SelectedItem);

                    Items = new ObservableCollection<T>(items);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
            finally
            {
                isRefreshing = false;
            }
        }

        /// <summary>
        /// Opens the item detail for new item.
        /// </summary>
        protected virtual void ExecuteNewCommand()
        {
            try
            {
                CreateNewItem();
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        /// <summary>
        /// Edit the item detail.
        /// </summary>
        /// <param name="itemId">The identifier of the record to edit</param>
        protected virtual void ExecuteEditItemCommand(T item)
        {
            try
            {
                EditItem(item);
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        /// <summary>
        /// Deletes the item.
        /// </summary>
        /// <param name="item">The item.</param>
        protected virtual void ExecuteDeleteCommand(T item)
        {
            try
            {
                DeleteItem(item);
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        /// <summary>
        /// Select the item.
        /// </summary>
        /// <param name="item">The item.</param>
        protected virtual void ExecuteSelectItemCommand(T item)
        {
            try
            {
                ProcessSelectedItem(item);
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        /// <summary>
        /// Determines whether the Refresh command can be executed.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        ///   <c>true</c> if the Refresh command can be executed; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool CanExecuteRefreshCommand(T obj)
        {
            return (isRefreshing == false);
        }

        /// <summary>
        /// Determines whether the New command can be executed.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        ///   <c>true</c> if the New command can be executed; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool CanExecuteNewCommand(T obj)
        {
            return true;
        }

        /// <summary>
        /// Determines whether the Edit Item command can be executed.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        ///   <c>true</c> if the Edit Item command can be executed; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool CanExecuteEditItemCommand(T obj)
        {
            return (obj != null);
        }

        /// <summary>
        /// Determines whether the Delete command can be executed.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        ///   <c>true</c> if the Delete command can be executed; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool CanExecuteDeleteCommand(T obj)
        {
            return (obj != null);
        }

        /// <summary>
        /// Determines whether the Select Item command can be executed.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        ///   <c>true</c> if the Select Item can be executed; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool CanExecuteSelectItemCommand(T obj)
        {
            return (obj != null);
        }

        /// <summary>
        /// Get item list from database
        /// </summary>
        protected virtual List<T> GetItems()
        {
            try
            {
                return ModelManager.GetList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Process seleted item after user change selection.
        /// </summary>
        protected virtual void ProcessSelectedItem(T selectedItem)
        {
            // do nothing, derived class should implement detail job
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// To be called when the value of the property, whose selector is passed as parameter, changes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The expressiong "bringing" the property.</param>
        public virtual void NotifyPropertyChanged<TObject>(Expression<Func<TObject>> expression)
        {
            string propertyName = ((MemberExpression)expression.Body).Member.Name;

            if (string.IsNullOrEmpty(propertyName) == false)
                NotifyPropertyChanged(propertyName);
        }

        #endregion // Public Mehthods

        #region Abstract methods, to be implemented in the inherited view models

        /// <summary>
        /// Refreshes the items.
        /// </summary>
        /// <returns>The items list.</returns>
        //protected abstract List<T> GetItems();

        /// <summary>
        /// Find item in the list.
        /// </summary>
        /// <returns>The result item</returns>
        //protected abstract T FindItem(T item, List<T> list);

        /// <summary>
        /// Do creating item.
        /// </summary>
        /// <returns></returns>
        protected abstract void CreateNewItem();

        /// <summary>
        /// Do editing selected item.
        /// </summary>
        /// <returns></returns>
        protected abstract void EditItem(T item);

        /// <summary>
        /// Do deleting selected item.
        /// </summary>
        /// <returns></returns>
        protected abstract void DeleteItem(T item);

        #endregion


        #region Overridden Methods

        protected override void OnDispose()
        {
            Mediator.Instance.UnregisterHandler<T>("Updated" + typeof(T).Name + "Successfully", HandleReceivedMessage);
            Mediator.Instance.UnregisterHandler<T>("Created" + typeof(T).Name + "Successfully", HandleReceivedMessage);
            //Mediator.Instance.Unregister(this);
            base.OnDispose();
        }

        #endregion

        #region Protectect virtual methods
        /// <summary>
        /// Handle received message from ItemViewModel
        /// </summary>
        /// <param name="item">Item go along with message</param>
        private void HandleReceivedMessage(T item)
        {
            // refresh item list
            this.ExecuteRefreshCommand();

            // re-select item
            this.SelectedItem = ModelManager.FindItem(item, this.Items.ToList());

            this.ExecuteSelectItemCommand(SelectedItem);
        }

        #endregion
    }
}
