using XRMS.Libraries.MVVM;

namespace XRMS.Presentation.ViewModels
{
    public class KeyPadViewModel : PropertyChangedNotifierBase
    {
        string _inputName;
        decimal _returnedValue;

        public KeyPadViewModel(string name)
        {
            InputName = "Enter " + name;
        }

        /// <summary>
        /// A input name.
        /// </summary>
        public string InputName
        {
            get { return _inputName; }
            set
            {
                _inputName = value;
                NotifyPropertyChanged(() => InputName);
            }
        }

        /// <summary>
        /// Returned value
        /// </summary>
        public decimal ReturnedValue
        {
            get { return _returnedValue; }
            set
            {
                _returnedValue = value;
                NotifyPropertyChanged(() => ReturnedValue);
            }
        }
    }
}
