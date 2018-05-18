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
    /// Interaction logic for ExistingWindow.xaml
    /// </summary>
    public partial class ExistingWindow : Window
    {
        List<PersonalInfo> _personalInfos;
        DataVaultInterface _dataVaultInterface;

        public ExistingWindow(DataVaultInterface dvInterface)
        {
            InitializeComponent();

            _dataVaultInterface = dvInterface;

            // Fill in data grid
            SetupInfoListView();
        }

        private void SetupInfoListView()
        {
            // Get the list
            _dataVaultInterface.GetBriefPersonalInfoList(out _personalInfos);

            // Set list to the control
            InfoListView.ItemsSource = _personalInfos;
        }

        private void Home_Button_Click(object sender, RoutedEventArgs e)
        {
            HomeWindow homeWindow = new HomeWindow();
            homeWindow.Show();
            this.Close();
        }

        private void ItemDoubleClicked(object sender, MouseButtonEventArgs e)
        {
            // Selected index
            int index = InfoListView.SelectedIndex;

            if (index != -1)
            {
                // Get the corresponding item
                PersonalInfo personalInfo = InfoListView.Items.GetItemAt(index) as PersonalInfo;

                // databaseId
                int databaseId = personalInfo.Id;

                // Bring up main window
                MainWindow mainWindow = new MainWindow(_dataVaultInterface, databaseId);
                mainWindow.Show();
                this.Close();
            }
        }
    }
}
