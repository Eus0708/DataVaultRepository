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
    
        /// <summary>
        /// Constructor
        /// </summary>
        public LoginWindow()
        {
            InitializeComponent();

            // Give textbox focus
            Password_TextBox.Focus();

            // Create the new data vault interface
            _dataVaultInterface = new DataVaultInterface();
        }

        /// <summary>
        /// Login button clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }

        /// <summary>
        /// Show meesage box
        /// </summary>
        /// <param name="message"></param>
        private MessageBoxResult ShowMessageBox(StatusCode status)
        {
            return MessageBox.Show(this, ErrorHandler.ErrorMessage(status), "Data Vault");
        }

        /// <summary>
        /// Show message box
        /// </summary>
        /// <param name="message"></param>
        private MessageBoxResult ShowMessageBox(string message)
        {
            return MessageBox.Show(this, message, "Data Vault");
        }

        /// <summary>
        /// Keyboard key up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PasswordTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            // Enter key pressed
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                Login();
            }
        }

        /// <summary>
        /// Log in
        /// </summary>
        private void Login()
        {
            string input = Password_TextBox.Password;

            if (null != _dataVaultInterface)
            {
                // Verify password
                StatusCode status = _dataVaultInterface.Login(input);

                // Check return value
                if (status == StatusCode.NO_ERROR)
                {
                    // Pass the interface to next window
                    ExistingWindow existingWindow = new ExistingWindow(_dataVaultInterface);
                    existingWindow.Show();
                    this.Close();
                }
                else
                {
                    ShowMessageBox(status);
                }
            }
        }
    }
}
