using System;
using System.Windows;
using System.Windows.Input;

using Cinch;
using XRMS.Presentation.ViewModels;

namespace XRMS.Presentation.Popups
{
    /// <summary>
    /// Interaction logic for KeyPadPopup.xaml
    /// </summary>
    [PopupNameToViewLookupKeyMetadata("KeyPadPopup", typeof(KeyPadPopup))]
    public partial class KeyPadPopup : Window
    {
        public KeyPadPopup()
        {
            InitializeComponent();
        }

        public decimal ReturnValue { get; set; }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txbValue.Text == "")
                    this.ReturnValue = 0;
                else
                    this.ReturnValue = Convert.ToDecimal(txbValue.Text);

                ((KeyPadViewModel)this.DataContext).ReturnedValue = this.ReturnValue;

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message + "\nYou entered wrong decimal number");
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    if (txbValue.Text == "")
                        this.ReturnValue = 0;
                    else
                        this.ReturnValue = Convert.ToDecimal(txbValue.Text);

                    ((KeyPadViewModel)this.DataContext).ReturnedValue = this.ReturnValue;
                    this.DialogResult = true;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message + "\nYou entered wrong decimal number");
            }
        }

        private void txbValue_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Decimal:
                    /*if (txbValue.Text == "0" || txbValue.Text == "")
                        txbValue.Text = ".";
                    else
                        txbValue.Text = txbValue.Text + ".";*/
                    e.Handled = false;
                    break;
                case Key.D0:
                case Key.NumPad0:
                    if (txbValue.Text != "")
                    {
                        if (Convert.ToDecimal(txbValue.Text) == 0)
                        {
                            e.Handled = true;
                            break;
                        }
                    }
                    e.Handled = false;
                    break;
                case Key.D1:
                case Key.D2:
                case Key.D3:
                case Key.D4:
                case Key.D5:
                case Key.D6:
                case Key.D7:
                case Key.D8:
                case Key.D9:
                case Key.NumPad1:
                case Key.NumPad2:
                case Key.NumPad3:
                case Key.NumPad4:
                case Key.NumPad5:
                case Key.NumPad6:
                case Key.NumPad7:
                case Key.NumPad8:
                case Key.NumPad9:

                    /*if (txbValue.Text != "")
                    {
                        try
                        {
                            if (Convert.ToDecimal(txbValue.Text) == 0)
                            {
                                txbValue.Text = "";
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("You entered wrong decimal number");
                        }
                    }*/
                    e.Handled = false;
                    break;
                case Key.Enter:
                    e.Handled = false;
                    break;
                default:
                    MessageBox.Show("Please enter number digit!!!");
                    e.Handled = true;
                    break;
            }

            /*if (txbValue.Text != "")
            {
                if (Convert.ToInt64(txbValue.Text) != 0)
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            }*/
        }

        private void btnNumberBack_Click(object sender, RoutedEventArgs e)
        {
            string s = txbValue.Text;

            if (s.Length > 1)
            {
                s = s.Substring(0, s.Length - 1);
            }
            else
            {
                s = "0";
            }

            txbValue.Text = s;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txbValue.Text = "0";
        }

        private void btnNumber0_Click(object sender, RoutedEventArgs e)
        {
            if (txbValue.Text == "0" || txbValue.Text == "")
                txbValue.Text = "0";
            else
                txbValue.Text = txbValue.Text + "0";
        }

        private void btnNumber1_Click(object sender, RoutedEventArgs e)
        {
            if (txbValue.Text == "0" || txbValue.Text == "")
                txbValue.Text = "1";
            else
                txbValue.Text = txbValue.Text + "1";
        }

        private void btnNumber2_Click(object sender, RoutedEventArgs e)
        {
            if (txbValue.Text == "0" || txbValue.Text == "")
                txbValue.Text = "2";
            else
                txbValue.Text = txbValue.Text + "2";
        }

        private void btnNumber3_Click(object sender, RoutedEventArgs e)
        {
            if (txbValue.Text == "0" || txbValue.Text == "")
                txbValue.Text = "3";
            else
                txbValue.Text = txbValue.Text + "3";
        }

        private void btnNumber4_Click(object sender, RoutedEventArgs e)
        {
            if (txbValue.Text == "0" || txbValue.Text == "")
                txbValue.Text = "4";
            else
                txbValue.Text = txbValue.Text + "4";
        }

        private void btnNumber5_Click(object sender, RoutedEventArgs e)
        {
            if (txbValue.Text == "0" || txbValue.Text == "")
                txbValue.Text = "5";
            else
                txbValue.Text = txbValue.Text + "5";
        }

        private void btnNumber6_Click(object sender, RoutedEventArgs e)
        {
            if (txbValue.Text == "0" || txbValue.Text == "")
                txbValue.Text = "6";
            else
                txbValue.Text = txbValue.Text + "6";
        }

        private void btnNumber7_Click(object sender, RoutedEventArgs e)
        {
            if (txbValue.Text == "0" || txbValue.Text == "")
                txbValue.Text = "7";
            else
                txbValue.Text = txbValue.Text + "7";
        }

        private void btnNumber8_Click(object sender, RoutedEventArgs e)
        {
            if (txbValue.Text == "0" || txbValue.Text == "")
                txbValue.Text = "8";
            else
                txbValue.Text = txbValue.Text + "8";
        }

        private void btnNumber9_Click(object sender, RoutedEventArgs e)
        {
            if (txbValue.Text == "0" || txbValue.Text == "")
                txbValue.Text = "9";
            else
                txbValue.Text = txbValue.Text + "9";
        }

        private void btnNumber000_Click(object sender, RoutedEventArgs e)
        {
            if (txbValue.Text == "0" || txbValue.Text == "")
                txbValue.Text = "0";
            else
                txbValue.Text = txbValue.Text + "000";
        }

        private void btnDot_Click(object sender, RoutedEventArgs e)
        {
            if (txbValue.Text == "0" || txbValue.Text == "")
                txbValue.Text = ".";
            else
                txbValue.Text = txbValue.Text + ".";
        }
    }
}
