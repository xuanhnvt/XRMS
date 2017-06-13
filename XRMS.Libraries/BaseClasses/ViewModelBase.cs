using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;

using XRMS.Libraries.DataTypes;
using XRMS.Libraries.Services;

namespace XRMS.Libraries.BaseClasses
{
    /// <summary>
    /// Base class for ViewModels that need to notify their property change event
    /// It implements the INotifyPropertyChanged interface
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
    {

        private IIOCProvider iocProvider = null;
        private static ILogger logger;
        private static Boolean isInitialised = false;
        private static Action<IUIVisualizerService> setupVisualizer = null;

        /// <summary>
        /// Service resolver for view models.  Allows derived types to add/remove
        /// services from mapping.
        /// </summary>
        public static readonly ServiceProvider ServiceProvider = new ServiceProvider();

        #region Constructors

        /// <summary>
        /// Constructs a new ViewModelBase and wires up all the Window based Lifetime
        /// commands such as activatedCommand/deactivatedCommand/loadedCommand/closeCommand
        /// </summary>
        public ViewModelBase() : this(new UnityProvider())
        {
           	
        }

        public ViewModelBase(IIOCProvider iocProvider)
        {

            if (iocProvider == null)
                throw new InvalidOperationException(
                    String.Format(
                        "ViewModelBase constructor requires a IIOCProvider instance in order to work"));

            this.iocProvider = iocProvider;

            if (!ViewModelBase.isInitialised)
            {
                iocProvider.SetupContainer();
                FetchCoreServiceTypes();
            }
        }

        #endregion // Constructors

        /// <summary>
        /// Delegate that is called when the services are injected
        /// </summary>
        public static Action<IUIVisualizerService> SetupVisualizer
        {
            get { return setupVisualizer; }
            set { setupVisualizer = value; }
        }

        #region DisplayName

        /// <summary>
        /// Returns the user-friendly name of this object.
        /// Child classes can set this property to a new value,
        /// or override it to determine the value on-demand.
        /// </summary>
        public virtual string DisplayName { get; protected set; }

        #endregion // DisplayName

        #region Debugging Aides

        /// <summary>
        /// Warns the developer if this object does not have
        /// a public property with the specified name. This 
        /// method does not exist in a Release build.
        /// </summary>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real,  
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;

                if (this.ThrowOnInvalidPropertyName)
                    throw new Exception(msg);
                else
                    Debug.Fail(msg);
            }
        }

        /// <summary>
        /// Returns whether an exception is thrown, or if a Debug.Fail() is used
        /// when an invalid property name is passed to the VerifyPropertyName method.
        /// The default value is false, but subclasses used by unit tests might 
        /// override this property's getter to return true.
        /// </summary>
        protected virtual bool ThrowOnInvalidPropertyName { get; private set; }

        #endregion // Debugging Aides

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// To be called when the value of the property, whose name is passed as parameter, changes.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            this.VerifyPropertyName(propertyName);

            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// To be called when the value of the property, whose selector is passed as parameter, changes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The expressiong "bringing" the property.</param>
        protected virtual void NotifyPropertyChanged<T>(Expression<Func<T>> expression)
        {
            string propertyName = ((MemberExpression)expression.Body).Member.Name;

            this.VerifyPropertyName(propertyName);

            if (string.IsNullOrEmpty(propertyName) == false)
                NotifyPropertyChanged(propertyName);
        }

        #endregion // INotifyPropertyChanged Members

        #region IDisposable Members

        /// <summary>
        /// Invoked when this object is being removed from the application
        /// and will be subject to garbage collection.
        /// </summary>
        public void Dispose()
        {
            this.OnDispose();
        }

        /// <summary>
        /// Child classes can override this method to perform 
        /// clean-up logic, such as removing event handlers.
        /// </summary>
        protected virtual void OnDispose()
        {

        }

        #if DEBUG
        /// <summary>
        /// Useful for ensuring that ViewModel objects are properly garbage collected.
        /// </summary>
        ~ViewModelBase()
        {
            string msg = string.Format("{0} ({1}) ({2}) Finalized", this.GetType().Name, this.DisplayName, this.GetHashCode());
            System.Diagnostics.Debug.WriteLine(msg);
        }
        #endif

        #endregion // IDisposable Members

        #region Event

        /// <summary>
        /// This event should be raised to close the view.  Any view tied to this
        /// ViewModel should register a handler on this event and close itself when
        /// this event is raised.  If the view is not bound to the lifetime of the
        /// ViewModel then this event can be ignored.
        /// </summary>
        public event EventHandler<CloseRequestEventArgs> CloseRequest;

        /// <summary>
        /// This event should be raised to activate the UI.  Any view tied to this
        /// ViewModel should register a handler on this event and close itself when
        /// this event is raised.  If the view is not bound to the lifetime of the
        /// ViewModel then this event can be ignored.
        /// </summary>
        public event EventHandler<EventArgs> ActivateRequest;

        /// <summary>
        /// This raises the CloseRequest event to close the UI.
        /// </summary>
        public virtual void OnCloseRequest()
        {
            EventHandler<CloseRequestEventArgs> handlers = CloseRequest;

            // Invoke the event handlers
            if (handlers != null)
            {
                try
                {
                    handlers(this, new CloseRequestEventArgs(null));
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(ex.Message);
                }
            }
        }

        /// <summary>
        /// This raises the CloseRequest event to close the UI.
        /// </summary>
        public virtual void OnCloseRequest(bool? dialogResult)
        {
            EventHandler<CloseRequestEventArgs> handlers = CloseRequest;

            // Invoke the event handlers
            if (handlers != null)
            {
                try
                {
                    handlers(this, new CloseRequestEventArgs(dialogResult));
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(ex.Message);
                }
            }
        }

        /// <summary>
        /// This raises the ActivateRequest event to activate the UI.
        /// </summary>
        public virtual void OnActivateRequest()
        {
            EventHandler<EventArgs> handlers = ActivateRequest;

            // Invoke the event handlers
            if (handlers != null)
            {
                try
                {
                    handlers(this, EventArgs.Empty);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(ex.Message);
                }
            }
        }

        #endregion  // Event

        /// <summary>
        /// This method registers services with the service provider.
        /// </summary>
        private void FetchCoreServiceTypes()
        {
            try
            {
                ViewModelBase.isInitialised = false;

                //ILogger : Allows MessageBoxs to be shown 
                logger = (ILogger)this.iocProvider.GetTypeFromContainer<ILogger>();

                ServiceProvider.Add(typeof(ILogger), logger);

                //IMessageBoxService : Allows MessageBoxs to be shown 
                IMessageBoxService messageBoxService =
                    (IMessageBoxService)this.iocProvider.GetTypeFromContainer<IMessageBoxService>();

                ServiceProvider.Add(typeof(IMessageBoxService), messageBoxService);

                //IOpenFileService : Allows Opening of files 
                IOpenFileService openFileService =
                    (IOpenFileService)this.iocProvider.GetTypeFromContainer<IOpenFileService>();
                ServiceProvider.Add(typeof(IOpenFileService), openFileService);

                //ISaveFileService : Allows Saving of files 
                ISaveFileService saveFileService =
                    (ISaveFileService)this.iocProvider.GetTypeFromContainer<ISaveFileService>();
                ServiceProvider.Add(typeof(ISaveFileService), saveFileService);

                //IUIVisualizerService : Allows popup management
                IUIVisualizerService uiVisualizerService =
                   (IUIVisualizerService)this.iocProvider.GetTypeFromContainer<IUIVisualizerService>();
                ServiceProvider.Add(typeof(IUIVisualizerService), uiVisualizerService);

                //call the callback delegate to setup IUIVisualizerService managed
                //windows
                if (SetupVisualizer != null)
                    SetupVisualizer(uiVisualizerService);

                ViewModelBase.isInitialised = true;

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        /// <summary>
        /// This resolves a service type and returns the implementation.
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <returns>Implementation</returns>
        protected T Resolve<T>()
        {
            return ServiceProvider.Resolve<T>();
        }
    }
}
