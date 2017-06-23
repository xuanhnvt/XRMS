using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace XRMS.Presentation.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void pwbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).Password = ((PasswordBox)sender).Password;
            }
        }

        /*private void Window_Closed(object sender, EventArgs e)
        {
            //Application.Current.Shutdown(-1);
            if (this.DialogResult != true)
            {
                MessageBox.Show("False");
                //Application.Current.Shutdown(-1);
            }
            else
            {
                MessageBox.Show("True");
            }
        }*/
    }
}
