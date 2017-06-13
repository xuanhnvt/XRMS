using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

using Csla.Core;

using Cinch;
using XRMS.Libraries.BaseObjects;
using XRMS.Libraries.CslaBase;

namespace XRMS.Libraries.MVVM
{
    /// <summary>
    /// Base class for all the view models for item views. The involved model object must derive from BusinessBase class
    /// </summary>
    /// <typeparam name="T">The model object the view model has to work on</typeparam>
    public abstract class ItemViewModelBase<T> : Cinch.ViewModelBase where T : CslaBusinessBase<T>, new()
    {
        #region Private Fields

        private IGenericManager<T> _modelManager;

        // hold reference of model object
        private T _item;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets message box service in order to show message to user
        /// </summary>
        public IMessageBoxService MessageBoxService { get; set; }

        /// <summary>
        /// Gets or sets the item.
        /// </summary>
        /// <value>The item that hold reference of model object.</value>
        public T Item
        {
            get { return this._item; }
            set
            {
                if (this._item == value)
                    return;
                this._item = value;
                NotifyPropertyChanged(() => Item);
            }
        }

        /// <summary>
        /// Gets or sets the model manager service.
        /// </summary>
        public IGenericManager<T> ModelManager
        {
            get { return this._modelManager; }
            set { this._modelManager = value; }
        }

        #endregion

        #region Commands Delegates
        /// <summary>
        /// Gets or sets the refresh command.
        /// </summary>
        public CommandBase<T> RefreshCommand { get; set; }

        /// <summary>
        /// Gets or sets the edit command.
        /// </summary>
        public CommandBase<T> EditItemCommand { get; set; }

        /// <summary>
        /// Gets or sets the save command.
        /// </summary>
        public CommandBase<T> SaveCommand { get; set; }

        /// <summary>
        /// Gets or sets the cancel command.
        /// </summary>
        public CommandBase<T> CancelCommand { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemViewModelBase&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="messageBoxService">The message box service.</param>
        /// <param name="modelManager">Service manage the object model</param>
        public ItemViewModelBase(IMessageBoxService messageBoxService, IGenericManager<T> modelManager)
        {
            // get message box service in order to show message to user
            if (messageBoxService == null)
            {
                throw new ArgumentNullException("messageBoxService");
            }
            this.MessageBoxService = messageBoxService;

            // do initialization
            try
            {
                if (modelManager == null)
                {
                    throw new ArgumentNullException("modelManager");
                }
                this._modelManager = modelManager;
                this.Item = ModelManager.New();

                // temporarily not use refresh and edit command
                //RefreshCommand = new DelegateCommand<object>(o => RefreshItemData(), CanExecuteRefreshCommand);
                EditItemCommand = new CommandBase<T>(o => this.ExecuteEditItemCommand(), o => true);
                SaveCommand = new CommandBase<T>(o => this.ExecuteSaveCommand(), o => this.CanExecuteSaveCommand());
                CancelCommand = new CommandBase<T>(o => this.ExecuteCancelCommand(), o => true);

                // start edit item
                this.EditItemCommand.Execute(null);
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);

            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemViewModelBase&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="messageBoxService">The message box service.</param>
        /// <param name="model">Existing model object is passed for editting</param>
        /// <param name="modelManager">Service manage the object model</param>
        public ItemViewModelBase(IMessageBoxService messageBoxService, T model, IGenericManager<T> modelManager)
        {
            // get message box service in order to show message to user
            if (messageBoxService == null)
            {
                throw new ArgumentNullException("messageBoxService");
            }
            this.MessageBoxService = messageBoxService;

            // do initialization
            try
            {
                if (modelManager == null)
                {
                    throw new ArgumentNullException("modelManager");
                }
                this._modelManager = modelManager;

                if (model == null)
                {
                    throw new ArgumentNullException("model");
                }
                this.Item = ModelManager.GetByKey(model);

                // temporarily not use refresh and edit command
                //RefreshCommand = new DelegateCommand<object>(o => RefreshItemData(), CanExecuteRefreshCommand);
                EditItemCommand = new CommandBase<T>(o => this.ExecuteEditItemCommand(), o => true);
                SaveCommand = new CommandBase<T>(o => this.ExecuteSaveCommand(), o => this.CanExecuteSaveCommand());
                CancelCommand = new CommandBase<T>(o => this.ExecuteCancelCommand(), o => true);

                // start edit item
                this.EditItemCommand.Execute(null);
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// To be called when the value of the property, whose selector is passed as parameter, changes.
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="expression">The expression "bringing" the property.</param>
        public virtual void NotifyPropertyChanged<TObject>(Expression<Func<TObject>> expression)
        {
            string propertyName = ((MemberExpression)expression.Body).Member.Name;

            if (string.IsNullOrEmpty(propertyName) == false)
                NotifyPropertyChanged(propertyName);
        }

        // can not use this method, fix later
        /// <summary>
        /// A helper method that raises the PropertyChanged event for all properties.
        /// </summary>
        /*public virtual void NotifyObjectChanged()
        {
            foreach (PropertyInfo pi in GetType().GetProperties(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                NotifyPropertyChanged(pi.Name);
            }
        }*/
        #endregion // Public Mehthods

        #region Protected Virtual Methods
        /// <summary>
        /// Executes the create item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        protected virtual bool CreateItem()
        {
            bool result = false;
            try
            {
                result = ModelManager.Create(this.Item);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// Executes the update item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        protected virtual bool UpdateItem()
        {
            bool result = false;
            try
            {
                result = ModelManager.Update(this.Item);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        #endregion // Protected Virtual Methods

        #region Command Methods

        /// <summary>
        /// Executes the save command.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        protected virtual void ExecuteSaveCommand()
        {
            bool saveResult = false;
            try
            {
                if (_item.IsNew)
                {
                    saveResult = this.CreateItem();

                    if (saveResult)
                    {
                        _item.MarkOld();
                        ((IEditableObject)_item).EndEdit();
                        //_item.ApplyEdit();
                        CloseActivePopUpCommand.Execute(true);
                        this.MessageBoxService.ShowInformation("Successfully created new " + typeof(T).Name.ToLower() + ".");

                        // Use the Mediator to send a message to specific data type
                        Mediator.Instance.NotifyColleagues<T>("Created" + typeof(T).Name + "Successfully", (T) _item);
                    }
                    else
                    {
                        throw new Exception("There is problem when saving data!");
                    }
                }
                else
                {
                    if (this.MessageBoxService.ShowYesNo(
                        "Would you like to update item information?",
                        CustomDialogIcons.Question) == CustomDialogResults.Yes)
                    {
                        saveResult = this.UpdateItem();
                        if (saveResult)
                        {
                            ((IEditableObject)_item).EndEdit();
                            //_item.ApplyEdit();
                            CloseActivePopUpCommand.Execute(true);
                            this.MessageBoxService.ShowInformation("Successfully updated " + typeof(T).Name.ToLower() + " information.");

                            // Use the Mediator to send a message to specific data type
                            Mediator.Instance.NotifyColleagues<T>("Updated" + typeof(T).Name + "Successfully", (T)_item);
                        }
                        else
                        {
                            throw new Exception("There is problem when saving data!");
                        }
                    }
                    else
                    {
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        /// <summary>
        /// Determines whether the save command can be executed.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        ///   <c>true</c> if the save command can be executed; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool CanExecuteSaveCommand()
        {
            //return _item.IsValid && _item.IsDirty;
            return _item.IsSavable;
        }

        /// <summary>
        /// Executes the cancel command.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        protected virtual void ExecuteCancelCommand()
        {
            try
            {
                (_item as ISupportUndo).CancelEdit();

                (_item as ISupportUndo).BeginEdit();
                //NotifyObjectChanged();
                CloseActivePopUpCommand.Execute(false);
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        /// <summary>
        /// Executes the edit command.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        protected virtual void ExecuteEditItemCommand()
        {
            try
            {
                (_item as ISupportUndo).BeginEdit();
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }
        #endregion // Command Methods

        #region Private Methods

        #endregion

        #region Abstract Methods, to be implemented in the inherited view models

        /// <summary>
        /// Executes the read item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The item.</returns>
        //protected abstract T ExecuteReadItem(T item);

        /// <summary>
        /// Executes the create item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        //protected abstract bool CreateItem();

        /// <summary>
        /// Executes the update item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        //protected abstract bool UpdateItem();

        #endregion
    }
}
