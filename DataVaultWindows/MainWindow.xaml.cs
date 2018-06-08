﻿using System;
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
using System.ComponentModel;

namespace DataVaultWindows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataVaultInterface _dataVaultInterface = null;
        PersonalInfo _personalInfo = null;
        AttachmentWindow[] _childWindows = null;
        int _personalInfoId = -1;
        List<StateInfo> _states = null;
        List<GenderInfo> _genders = null;
        bool _isSaved = true;

        public bool IsSaved
        {
            get { return _isSaved; }
            set
            {
                if ( value != _isSaved)
                {
                    _isSaved = value;

                    // Update save button
                    Save_Button.IsEnabled = !_isSaved;
                }
            }
        }

        public bool IsNotSaved
        {
            get { return !_isSaved; }
        }

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

            // Setup controls events
            SetupControlEvents();

            // Setup windown events
            this.Closing += WindowClosing;
            this.Closed += WindowClosed;
        }

        /// <summary>
        /// Close all chile windows
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowClosing(object sender, CancelEventArgs e)
        {
            if (!IsSaved)
            {
                // Ask to save
                MessageBoxResult result = ShowMessageBox("Do you want to save your changes?", MessageBoxButton.YesNoCancel);

                if (result == MessageBoxResult.Yes)
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
                            e.Cancel = false;

                            // Close all child windows
                            CloseAllChildWindows();

                            return;
                        }
                        else
                        {
                            e.Cancel = true;

                            ShowMessageBox(status);

                            return;
                        }
                    }
                }
                else if (result == MessageBoxResult.No)
                {
                    e.Cancel = false;

                    // Close all child windows
                    CloseAllChildWindows();

                    return;
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;

                    return;
                }
            }

            e.Cancel = false;

            // Close all child windows
            CloseAllChildWindows();

            return;
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

                // Create empty child window list. Should be same size as the attachment list
                int attachmentsCount = _personalInfo.Attachments.Count;
                _childWindows = new AttachmentWindow[attachmentsCount];
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

        /// <summary>
        /// Setup events for controls
        /// </summary>
        private void SetupControlEvents()
        {
            // Text box
            FirstName_TextBox.TextChanged += Control_ValueChanged;
            MiddleName_TextBox.TextChanged += Control_ValueChanged;
            LastName_TextBox.TextChanged += Control_ValueChanged;
            AreaCode_TextBox.TextChanged += Control_ValueChanged;
            PhoneNumber_TextBox.TextChanged += Control_ValueChanged;
            DOB_TextBox.TextChanged += Control_ValueChanged;
            StreetAdd1_TextBox.TextChanged += Control_ValueChanged;
            StreetAdd2_TextBox.TextChanged += Control_ValueChanged;
            City_TextBox.TextChanged += Control_ValueChanged;
            Zipcode_TextBox.TextChanged += Control_ValueChanged;
            SSN_TextBox.TextChanged += Control_ValueChanged;

            // Combo box
            Genders_ComboBox.SelectionChanged += Control_ValueChanged;
            States_ComboBox.SelectionChanged += Control_ValueChanged;
        }

        /// <summary>
        /// Some checkbox in list view is checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewCheckboxChecked(object sender, RoutedEventArgs e)
        {
            // Something is changed
            IsSaved = false;
        }

        /// <summary>
        /// Some checkbox in list view is unchecked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewCheckboxUnChecked(object sender, RoutedEventArgs e)
        {
            // Something is changed
            IsSaved = false;
        }

        /// <summary>
        /// Close all child windows
        /// </summary>
        private void CloseAllChildWindows()
        {
            // Close all child windows
            for (int i = 0; i < _childWindows.Length; i++)
            {
                if (_childWindows[i] != null)
                {
                    _childWindows[i].Close();
                    _childWindows[i] = null;
                }
            }
        }

        /// <summary>
        /// Exit button clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            if (droppedFiles.Length > 0)
            {
                // Something is changed
                IsSaved = false;
            }

            foreach (string path in droppedFiles)
            {
                string filename = GetFileName(path);
                ShowMessageBox("You dropped " + filename);

                // Create a new object and added to the list
                AttachmentInfo attachment = new AttachmentInfo();
                attachment.Path = path;
                attachment.FullFilename = filename;
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

        /// <summary>
        /// Attachment clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemDoubleClicked(object sender, MouseButtonEventArgs e)
        {
            // Selected index
            int index = Attachments_ListView.SelectedIndex;

            if (index != -1)
            {
                // Get the corresponding item
                AttachmentInfo attachmentInfo = Attachments_ListView.Items.GetItemAt(index) as AttachmentInfo;

                // databaseId
                int databaseId = attachmentInfo.Id;

                if (databaseId != -1)
                {
                    // Bring up attachment window
                    if (_childWindows[index] == null)
                    {
                        _childWindows[index] = new AttachmentWindow(_dataVaultInterface, databaseId, index);
                        _childWindows[index].Closed += OnChildWindowsClosed;
                        _childWindows[index].Show();
                    }
                    else
                    {
                        // Bring up the window
                        _childWindows[index].Focus();
                    }
                }
                else
                {
                    ShowMessageBox("Please SAVE before opening the image.");
                }
            }
        }

        /// <summary>
        /// Attachment window closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChildWindowsClosed(object sender, EventArgs e)
        {
            AttachmentWindow attachmentWindow = sender as AttachmentWindow;

            int index = attachmentWindow.ChildWindowId;

            if ( index >= 0 && index < _childWindows.Length )
            {
                // Already closed set it to null
                _childWindows[index] = null;
            }
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
                    // Refresh all controls
                    RefreshAllControls();

                    // Set the boolean
                    IsSaved = true;

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
        /// Refresh all controls
        /// </summary>
        private void RefreshAllControls()
        {
            // Get peronsal info from database
            RetrieveInfoFromDb();

            // Populate controls
            PopulateControls();
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
        /// Text box string changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Control_ValueChanged(object sender, TextChangedEventArgs e)
        {
            // Something is changed
            IsSaved = false;
        }

        /// <summary>
        /// Combo box selection changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Control_ValueChanged(object sender, SelectionChangedEventArgs e)
        {
            // Something is changed
            IsSaved = false;
        }

        /// <summary>
        /// Combo box value changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewComboboxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Something is changed
            IsSaved = false;
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
        /// Show message box
        /// </summary>
        /// <param name="message"></param>
        private MessageBoxResult ShowMessageBox(string message, MessageBoxButton button)
        {
            return MessageBox.Show(message, "Data Vault", button);
        }
    }
}
