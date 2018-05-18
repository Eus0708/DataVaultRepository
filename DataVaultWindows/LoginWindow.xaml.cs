using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using DataVaultCommon;
using SystemCommon;

namespace DataVaultWindows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {

        DataVaultInterface _dataVaultInterface;
    
        public LoginWindow()
        {
            InitializeComponent();

            // Create the new data vault interface
            _dataVaultInterface = new DataVaultInterface();
        }

        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            string input = Password_TextBox.Text;

            if (_dataVaultInterface.Login(input) == StatusCode.NO_ERROR)
            {
                if (null != _dataVaultInterface && _dataVaultInterface.HasAccess)
                {
                    // Pass the interface to next window
                    ExistingWindow existingWindow = new ExistingWindow(_dataVaultInterface);
                    existingWindow.Show();
                    this.Close();
                }
            }
        }

        private void Password_TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "Please Enter Your Password...")
            {
                tb.Text = "";
            }

            tb.Foreground = Brushes.Black;
        }

        private void Password_TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "")
            {
                tb.Text = "Please Enter Your Password...";
                tb.Foreground = Brushes.LightGray;
            }
            else
            {
                tb.Foreground = Brushes.Black;
            }
        }
    }
}
