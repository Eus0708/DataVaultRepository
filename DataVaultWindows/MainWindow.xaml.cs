using System;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

using DataVaultCommon;
using SystemCommon;

namespace DataVaultWindows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataVaultInterface _dataVaultInterface = null;
        PersonalInfo _personalInfo = null;
        int _personalInfoId = -1;
        List<StateInfo> _states = null;
        List<GenderInfo> _genders = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dvInterface"></param>
        /// <param name="personalInfoId"></param>
        public MainWindow(DataVaultInterface dvInterface, int personalInfoId = -1)
        {
            InitializeComponent();

            _dataVaultInterface = dvInterface;
            _personalInfoId = personalInfoId;

            // Get peronsal info from database
            RetrieveInfoFromDb();

            // Populate controls
            PopulateControls();

            // Setup windown events
            this.Closed += WindowClosed;
        }

        /// <summary>
        /// Go back to existing window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowClosed(object sender, EventArgs e)
        {
            ExistingWindow existingWindow = new ExistingWindow(_dataVaultInterface);
            existingWindow.Show();
        }

        /// <summary>
        /// Populate member data with the data from db
        /// </summary>
        private void RetrieveInfoFromDb()
        {
            if (_dataVaultInterface != null)
            {
                // Get info from db
                _dataVaultInterface.GetPersonalInfo(out _personalInfo, _personalInfoId);

                // Get states form db
                _dataVaultInterface.GetStates(out _states);

                // Get genders from db
                _dataVaultInterface.GetGenders(out _genders);

                // Get attachment types from db
                List<AttachmentTypeInfo> attachmentTypes;
                _dataVaultInterface.GetAttachmentTypes(out attachmentTypes);
                AttachmentInfo.SetAttachmentTypes(attachmentTypes);
            }
        }

        /// <summary>
        /// Populate controls
        /// </summary>
        private void PopulateControls()
        {
            if (_personalInfo != null)
            {
                // Combo box
                int stateIndex = _states.FindIndex(
                    delegate (StateInfo state)
                    {
                        if (state.State.Equals(_personalInfo.Address.State))
                        {
                            return true;
                        }
                        return false;
                    });
                States_ComboBox.ItemsSource = _states;
                States_ComboBox.SelectedIndex = stateIndex;
                int genderIndex = _genders.FindIndex(
                    delegate (GenderInfo gender)
                    {
                        if (gender.Gender.Equals(_personalInfo.Gender))
                        {
                            return true;
                        }
                        return false;
                    });
                Genders_ComboBox.ItemsSource = _genders;
                Genders_ComboBox.SelectedIndex = genderIndex;

                // Text box
                FirstName_TextBox.Text = _personalInfo.Name.FirstName;
                MiddleName_TextBox.Text = _personalInfo.Name.MiddleName;
                LastName_TextBox.Text = _personalInfo.Name.LastName;
                AreaCode_TextBox.Text = _personalInfo.PhoneNumber.AreaCode;
                PhoneNumber_TextBox.Text = _personalInfo.PhoneNumber.PhoneNumber;
                SSN_TextBox.Text = _personalInfo.SSN.SSNNumber;
                StreetAdd1_TextBox.Text = _personalInfo.Address.Address1;
                StreetAdd2_TextBox.Text = _personalInfo.Address.Address2;
                City_TextBox.Text = _personalInfo.Address.City;
                Zipcode_TextBox.Text = _personalInfo.Address.ZipCode;

                // List box
                Attachments_ListView.ItemsSource = _personalInfo.Attachments;
            }
        }

        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            string controlName = tb.Name;
            string hint = ControlHints.GetHints(controlName);

            if (tb.Text.Equals(hint))
                tb.Text = "";

            tb.Foreground = Brushes.Black;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            string controlName = tb.Name;

            if (tb.Text == "")
            {
                tb.Text = ControlHints.GetHints(controlName);
                tb.Foreground = Brushes.LightGray;
            }
            else
            {
                tb.Foreground = Brushes.Black;
            }
        }

        /// <summary>
        /// Attached an item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView_Drop(object sender, DragEventArgs e)
        {
            string[] droppedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

            foreach (string file in droppedFiles)
            {
                string filename = GetFileName(file);
                ShowMessageBox("You dropped " + filename);

                // Create a new object and added to the list
                AttachmentInfo attachment = new AttachmentInfo();
                attachment.Filename = filename;
                _personalInfo.AddAttachment(attachment);
            }
        }

        /// <summary>
        /// Setup effect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                e.Effects = DragDropEffects.All;
            }
        }

        /// <summary>
        /// Get file name
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string GetFileName(string path)
        {
            return System.IO.Path.GetFileName(path);
        }

        private void ItemDoubleClicked(object sender, MouseButtonEventArgs e)
        {

        }

        /// <summary>
        /// Save button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Button_Clicked(object sender, RoutedEventArgs e)
        {
            // Fill the personal info object
            RetrieveDataFromControls();

            // Save to database
            if (_personalInfo != null && _dataVaultInterface != null)
            {
                StatusCode status = _dataVaultInterface.ModifyPersonalInfo(_personalInfo);

                // Show status
                if (status == StatusCode.NO_ERROR)
                {
                    ShowMessageBox("Saved!");
                }
                else
                {
                    ShowMessageBox(status);
                }
            }
        }

        /// <summary>
        /// Get modified info
        /// </summary>
        private void RetrieveDataFromControls()
        {
            if (_personalInfo != null)
            {
                _personalInfo.Name.FirstName = FirstName_TextBox.Text.Trim();
                _personalInfo.Name.MiddleName = MiddleName_TextBox.Text.Trim();
                _personalInfo.Name.LastName = LastName_TextBox.Text.Trim();
                _personalInfo.PhoneNumber.AreaCode = AreaCode_TextBox.Text.Trim();
                _personalInfo.PhoneNumber.PhoneNumber = PhoneNumber_TextBox.Text.Trim();
                _personalInfo.Address.Address1 = StreetAdd1_TextBox.Text.Trim();
                _personalInfo.Address.Address2 = StreetAdd2_TextBox.Text.Trim();
                _personalInfo.Address.City = City_TextBox.Text.Trim();
                _personalInfo.Address.State = GetComboBoxString(States_ComboBox);
                _personalInfo.Address.ZipCode = Zipcode_TextBox.Text.Trim();
                _personalInfo.Gender = GetComboBoxString(Genders_ComboBox);
                _personalInfo.SSN.SSNNumber = SSN_TextBox.Text.Trim();
            }
        }

        /// <summary>
        /// Get string from combo box
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        private string GetComboBoxString(ComboBox control)
        {
            if (control.SelectedValue != null)
            {
                return control.SelectedValue.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Show meesage box
        /// </summary>
        /// <param name="message"></param>
        private void ShowMessageBox(StatusCode status)
        {
            MessageBox.Show(this, ErrorHandler.ErrorMessage(status), "Data Vault");
        }

        /// <summary>
        /// Show message box
        /// </summary>
        /// <param name="message"></param>
        private void ShowMessageBox(string message)
        {
            MessageBox.Show(this, message, "Data Vault");
        }
    }
}
