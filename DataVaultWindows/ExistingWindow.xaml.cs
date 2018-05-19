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
        DataVaultInterface _dataVaultInterface;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dvInterface"></param>
        public ExistingWindow(DataVaultInterface dvInterface)
        {
            InitializeComponent();

            _dataVaultInterface = dvInterface;

            // Fill in data grid
            RefreshViewList();

            // Setup controls
            PopulateControls();
        }

        /// <summary>
        /// Populate controls
        /// </summary>
        private void PopulateControls()
        {
            // Search category combo box
            SearchCat_ComboBox.ItemsSource = DataVaultInterface.SearchOptions;
        }

        private void NewProfile_Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(_dataVaultInterface);
            mainWindow.Show();
            this.Close();
        }

        /// <summary>
        /// Double clicked a person
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Search Text box enter pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            // Enter key pressed
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                string input = Search_TextBox.Text.Trim();
                int searchOptIndex = SearchCat_ComboBox.SelectedIndex;

                RefreshViewList(input, searchOptIndex);
            }
        }

        /// <summary>
        /// Get the list without filter
        /// </summary>
        private void RefreshViewList()
        {
            // Get the whole list
            RefreshViewList(null, -1);
        }

        /// <summary>
        /// Get list from db
        /// </summary>
        /// <param name="searchInput"></param>
        /// <param name="searchOptIndex"></param>
        private void RefreshViewList(string searchInput, int searchOptIndex)
        {
            List<PersonalInfo> resultList;

            if ( !string.IsNullOrEmpty(searchInput) && searchOptIndex != -1)
            {
                // Search from database
                _dataVaultInterface.SearchBriefPersonalInfoList(
                    out resultList,
                    searchInput,
                    (DataVaultInterface.SearchOptionsEnum)searchOptIndex);
            }
            else
            {
                _dataVaultInterface.GetBriefPersonalInfoList(out resultList);
            }

            // Update control
            PersonalInfos_ListView.ItemsSource = resultList;
        }
    }
}
