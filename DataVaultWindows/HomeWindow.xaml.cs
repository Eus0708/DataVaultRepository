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

namespace DataVaultWindows
{
    /// <summary>
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {
        public HomeWindow()
        {
            InitializeComponent();
        }

        private void Main_Window_Button(object sender, RoutedEventArgs e)
        {
            
            MainWindow ss = new MainWindow();
            ss.Show();
            this.Hide();
        }

        private void Exit_Button(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
