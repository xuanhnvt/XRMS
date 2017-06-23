using System.Windows;
using System.Globalization;

namespace XRMS.Libraries.Helpers
{
    /// <summary>
    /// This class shows messages via the MessageBox to the user.
    /// </summary>
    public class MessageService
    {
        private static MessageBoxResult MessageBoxResult { get { return MessageBoxResult.None; } }

        private static MessageBoxOptions MessageBoxOptions
        {
            get
            {
                return (CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft) ? MessageBoxOptions.RtlReading : MessageBoxOptions.None;
            }
        }

        /// <summary>
        /// Shows the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void ShowMessage(string message)
        {
            MessageBox.Show(message, "Message", MessageBoxButton.OK, MessageBoxImage.None,
                    MessageBoxResult, MessageBoxOptions);
        }

        /// <summary>
        /// Shows the message as warning.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void ShowWarning(string message)
        {
            MessageBox.Show(message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning,
                    MessageBoxResult, MessageBoxOptions);
        }

        /// <summary>
        /// Shows the message as error.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult, MessageBoxOptions);
        }

        /// <summary>
        /// Shows the specified question.
        /// </summary>
        /// <param name="message">The question.</param>
        /// <returns><c>true</c> for yes, <c>false</c> for no and <c>null</c> for cancel.</returns>
        public static bool? ShowQuestion(string message)
        {
            MessageBoxResult result;

            result = MessageBox.Show(message, "Question", MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question, MessageBoxResult.Cancel, MessageBoxOptions);

            if (result == MessageBoxResult.Yes) { return true; }
            else if (result == MessageBoxResult.No) { return false; }

            return null;
        }

        /// <summary>
        /// Shows the specified yes/no question.
        /// </summary>
        /// <param name="message">The question.</param>
        /// <returns><c>true</c> for yes and <c>false</c> for no.</returns>
        public static bool ShowYesNoQuestion(string message)
        {
            MessageBoxResult result;
            
            result = MessageBox.Show(message, "YesNoQuestion", MessageBoxButton.YesNo,
                    MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions);

            return result == MessageBoxResult.Yes;
        }
    }
}
