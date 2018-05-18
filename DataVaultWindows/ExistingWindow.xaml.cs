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

            // Setup controls
            SetupControls();
        }

        private void SetupControls()
        {
            // Search category combo box
            SearchCat_ComboBox.ItemsSource = DataVaultInterface.SearchOptions;
        }

        private void SetupInfoListView()
        {
            // Get the list
            _dataVaultInterface.GetBriefPersonalInfoList(out _personalInfos);

            // Set list to the control
            PersonalInfos_ListView.ItemsSource = _personalInfos;
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
            int index = PersonalInfos_ListView.SelectedIndex;

            if (index != -1)
            {
                // Get the corresponding item
                PersonalInfo personalInfo = PersonalInfos_ListView.Items.GetItemAt(index) as PersonalInfo;

                // databaseId
                int databaseId = personalInfo.Id;

                // Bring up main window
                MainWindow mainWindow = new MainWindow(_dataVaultInterface, databaseId);
                mainWindow.Show();
                this.Close();
            }
        }

        // Search Text box enter pressed
        private void SearchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            // Enter key pressed
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                List<PersonalInfo> searchResult;
                string input = Search_TextBox.Text;
                int searchOptIndex = SearchCat_ComboBox.SelectedIndex;

                // Search from database
                _dataVaultInterface.SearchBriefPersonalInfoList(
                    out searchResult,
                    input,
                    (DataVaultInterface.SearchOptionsEnum) searchOptIndex);

                // Update control
                PersonalInfos_ListView.ItemsSource = searchResult;
            }
        }
    }
}
