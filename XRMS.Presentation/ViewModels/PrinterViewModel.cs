using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Drawing.Printing;
using System.Collections.ObjectModel;

using Cinch;
using XRMS.Libraries.BaseClasses;
using XRMS.Libraries.Helpers;
using XRMS.Libraries.MVVM;

namespace XRMS.Presentation.ViewModels
{
    public class PrinterViewModel : Cinch.ViewModelBase
    {
        #region Private Data Members
        private ObservableCollection<string> _printerList = new ObservableCollection<string>();
        private string _selectedPrinter;

        IMessageBoxService messageBoxService = null;
        #endregion // Private Data Members

        #region Constructors
        public PrinterViewModel(IMessageBoxService messageBoxService)
        {
            // do initialization

            this.messageBoxService = messageBoxService;
            try
            {
                this.DisplayName = "Printer Selection";
                this.PopulatePrinterList();

                this.OkCommand = new CommandBase<object>(o => this.ExecuteOkCommand());
                this.CancelCommand = new CommandBase<object>(o => this.ExecuteCancelCommand());
            }
            catch (Exception ex)
            {
                this.messageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }
        #endregion // Constructors

        #region Public Properties

        /// <summary>
        /// A printer list.
        /// </summary>
        public ObservableCollection<string> PrinterList
        {
            get { return _printerList; }
            set { _printerList = value; NotifyPropertyChanged("PrinterList"); }
        }

        /// <summary>
        /// The currently-selected printer.
        /// </summary>
        public string SelectedPrinter
        {
            get { return _selectedPrinter; }
            set { _selectedPrinter = value; NotifyPropertyChanged("SelectedPrinter"); }
        }
        #endregion // Public Properties

        #region Command Properties

        /// <summary>
        /// Returns the command that, when invoked, get selected printer
        /// </summary>
        public CommandBase<object> OkCommand { get; set; }

        /// <summary>
        /// Returns the command that, when invoked, cancel printer selection
        /// </summary>
        public CommandBase<object> CancelCommand { get; set; }

        #endregion // Command Properties

        #region Private Method Members

        private void PopulatePrinterList()
        {
            try
            {
                PrinterSettings ps = new PrinterSettings();
                foreach (string item in PrinterSettings.InstalledPrinters)
                {
                    PrinterList.Add(item);
                }
            }
            catch (Exception ex)
            {
                this.messageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        /// <summary>
        /// ExecuteOkCommand
        /// </summary>
        private void ExecuteOkCommand()
        {
            try
            {
                CloseActivePopUpCommand.Execute(true);
            }
            catch (Exception ex)
            {
                messageBoxService.ShowError("ExecuteOkCommand: " + ex.Message);
            }
        }

        /// <summary>
        /// ExecuteCancelCommand
        /// </summary>
        private void ExecuteCancelCommand()
        {
            try
            {
                SelectedPrinter = null;
                CloseActivePopUpCommand.Execute(false);
            }
            catch (Exception ex)
            {
                this.messageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }
        #endregion // Private Method Members
    }
}
