using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
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
    /// Interaction logic for AttachmentWindow.xaml
    /// </summary>
    public partial class AttachmentWindow : Window, INotifyPropertyChanged
    {
        DataVaultInterface _dataVaultInterface = null;
        AttachmentInfo _attachmentInfo = null;
        byte[] _attachmentData = null;
        int _childWindowId = -1;

        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public AttachmentInfo attachment
        {
            get { return _attachmentInfo; }
        }

        public int ChildWindowId
        {
            get { return _childWindowId; }
        }

        public string Filename
        {
            get { return _attachmentInfo.Filename; }
            set
            {
                _attachmentInfo.Filename = value;
                NotifyPropertyChanged("Filename");
            }
        }

        public string AttachmentType
        {
            get { return _attachmentInfo.Type; }
            set
            {
                _attachmentInfo.Type = value;
                NotifyPropertyChanged("AttachmentType");
            }
        }

        public bool ToBeDelete
        {
            get { return _attachmentInfo.ToBeDelete; }
            set
            {
                _attachmentInfo.ToBeDelete = value;
                NotifyPropertyChanged("ToBeDelete");
            }
        }

        public List<AttachmentTypeInfo> AttachmentTypes
        {
            get { return _attachmentInfo.AttachmentTypes; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="attachment"></param>
        public AttachmentWindow(AttachmentInfo attachment)
        {
            InitializeComponent();
            MyGrid.DataContext = this;

            _attachmentInfo = attachment;

            // Retrieve attachment data 
            RetrieveAttachmentData();

            // Populate controls
            PopulateControls();

            // Windows event
            this.Closing += WindowClosing;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dvInterface"></param>
        /// <param name="attachmentId"></param>
        public AttachmentWindow(DataVaultInterface dvInterface, AttachmentInfo attachment, int childWindowId)
        {
            InitializeComponent();
            MyGrid.DataContext = this;

            _dataVaultInterface = dvInterface;
            _attachmentInfo = attachment;
            _childWindowId = childWindowId;

            // Retrieve attachment data 
            RetrieveAttachmentData();

            // Populate controls
            PopulateControls();

            // Windows event
            this.Closing += WindowClosing;
        }

        /// <summary>
        /// Window closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowClosing(object sender, CancelEventArgs e)
        {
            StatusCode status = RetrieveDataFromControls();
            if (status == StatusCode.NO_ERROR)
            {
                e.Cancel = false;
            }
            else
            {
                ShowMessageBox(status);
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Retrieve attachment data
        /// </summary>
        private void RetrieveAttachmentData()
        {
            if (_attachmentInfo.Id == -1)
            {
                // Get attachment data from local path
                if (!_attachmentInfo.Path.Equals(String.Empty))
                {
                    System.Drawing.Image image = new Bitmap(_attachmentInfo.Path);
                    MemoryStream memStream = new MemoryStream();
                    image.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    _attachmentData = memStream.ToArray();
                }
            }
            else
            {
                if (_dataVaultInterface != null)
                {
                    // Get attachment data from db
                    _dataVaultInterface.GetAttachmentData(out _attachmentData, _attachmentInfo.Id);
                }
            }
        }

        /// <summary>
        /// Populate controls
        /// </summary>
        private void PopulateControls()
        {
            // Title
            this.Title = _attachmentInfo.FullFilename;

            // TextBox
            Extension_TextBox.Text = _attachmentInfo.Extension;

            // Image
            if (_attachmentData != null && _attachmentData.Length > 0)
            {
                System.IO.MemoryStream memStream = new System.IO.MemoryStream(_attachmentData);
                memStream.Seek(0, System.IO.SeekOrigin.Begin);

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memStream;
                bitmapImage.EndInit();

                Attachment_Image.Source = bitmapImage;
            }

            // Combo box
            Category_ComboBox.ItemsSource = AttachmentTypes;
        }

        /// <summary>
        /// Get data from controls
        /// </summary>
        /// <returns></returns>
        private StatusCode RetrieveDataFromControls()
        {
            try
            {
                if (_attachmentInfo != null)
                {
                    _attachmentInfo.Filename = Name_TextBox.Text.Trim();
                    _attachmentInfo.Type = Category_ComboBox.Text;

                    return StatusCode.NO_ERROR;
                }

                return StatusCode.APPLICATION_ERROR;
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message);
                return StatusCode.APPLICATION_ERROR;
            }
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
        /// Close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Button_Clicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
