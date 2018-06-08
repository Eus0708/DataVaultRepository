using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        GridViewColumnHeader _listViewSortCol;
        SortAdorner _listViewSortAdorner;

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

            // Sorting
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(PersonalInfos_ListView.ItemsSource);
            view.SortDescriptions.Add(new SortDescription("FullName", ListSortDirection.Ascending));
        }

        /// <summary>
        /// Column header clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();
            if (_listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(_listViewSortCol).Remove(_listViewSortAdorner);
                PersonalInfos_ListView.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (_listViewSortCol == column && _listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            _listViewSortCol = column;
            _listViewSortAdorner = new SortAdorner(_listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(_listViewSortCol).Add(_listViewSortAdorner);
            PersonalInfos_ListView.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
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
                // Refresh list with search
                RefreshListWithSearch();
            }
        }

        /// <summary>
        /// Search button clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search_Button_Click(object sender, RoutedEventArgs e)
        {
            // Refresh list with search
            RefreshListWithSearch();
        }

        /// <summary>
        /// Refresh list with search
        /// </summary>
        private void RefreshListWithSearch()
        {
            string input = Search_TextBox.Text.Trim();
            int searchOptIndex = SearchCat_ComboBox.SelectedIndex;

            RefreshViewList(input, searchOptIndex);
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

        /// <summary>
        /// Remove the selected row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveProfile_Button_Click(object sender, RoutedEventArgs e)
        {
            PersonalInfo personalInfo = PersonalInfos_ListView.SelectedItem as PersonalInfo;

            if (personalInfo != null)
            {
                // Ask the user for confirmation
                MessageBoxResult result = ShowMessageBox("Delete \"" + personalInfo.FullName + "\" ?", MessageBoxButton.YesNo);

                // Remove personal info from the db
                if (result == MessageBoxResult.Yes)
                {
                    personalInfo.ToBeDelete = true;
                    _dataVaultInterface.ModifyPersonalInfo(personalInfo);

                    // Refresh the list
                    RefreshViewList();
                }
            }
        }

        /// <summary>
        /// Show message box
        /// </summary>
        /// <param name="message"></param>
        private MessageBoxResult ShowMessageBox(string message, MessageBoxButton button)
        {
            return MessageBox.Show(message, "Data Vault", button);
        }
    }
}
