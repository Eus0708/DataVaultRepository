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

        public MainWindow(DataVaultInterface dvInterface, int personalInfoId)
        {
            InitializeComponent();

            _dataVaultInterface = dvInterface;
            _personalInfoId = personalInfoId;

            // Get peronsal info from database
            RetrieveInfoFromDb();

            // Set up controls
            SetupControls();
        }

        private void RetrieveInfoFromDb()
        {
            if (_dataVaultInterface != null)
            {
                if (_personalInfoId != -1)
                {
                    // Get info from db
                    _dataVaultInterface.GetPersonalInfo(out _personalInfo, _personalInfoId);
                }

                // Get states form db
                _dataVaultInterface.GetStates(out _states);

                // Get genders from db
                _dataVaultInterface.GetGenders(out _genders);
            }
        }

        private void SetupControls()
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

        private void TextBox_DragLeave(object sender, DragEventArgs e)
        {
            //e.Effects = DragDropEffects.All;
            
        }

        private void Home_Button_Click(object sender, RoutedEventArgs e)
        {
            
            HomeWindow ss = new HomeWindow();
            ss.Show();
            this.Hide();
        }

        private void ListBox_Drop(object sender, DragEventArgs e)
        {
            string[] droppedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

            foreach (string file in droppedFiles)
            {
                string filename = getFilelName(file);
                MessageBox.Show("You dropped " + filename);
                DragDrop_ListBox.Items.Add(filename);
                
            }
        }

        private void ListBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                e.Effects = DragDropEffects.All;
            }
        }

        private string getFilelName(string path)
        {
            return System.IO.Path.GetFileNameWithoutExtension(path);
        }

        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }
    }
}
