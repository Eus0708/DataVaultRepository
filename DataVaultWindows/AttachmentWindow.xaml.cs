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
    /// Interaction logic for AttachmentWindow.xaml
    /// </summary>
    public partial class AttachmentWindow : Window
    {
        DataVaultInterface _dataVaultInterface = null;
        AttachmentInfo _attachmentInfo = null;
        int _attachmentId = -1;
        byte[] _attachmentData = null;
        int _childWindowId = -1;

        public int ChildWindowId
        {
            get { return _childWindowId; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dvInterface"></param>
        /// <param name="attachmentId"></param>
        public AttachmentWindow(DataVaultInterface dvInterface, int attachmentId, int childWindowId)
        {
            InitializeComponent();

            _dataVaultInterface = dvInterface;
            _attachmentId = attachmentId;
            _childWindowId = childWindowId;

            // Get peronsal info from database
            RetrieveInfoFromDb();

            // Populate controls
            PopulateControls();
        }

        /// <summary>
        /// Get data from db
        /// </summary>
        private void RetrieveInfoFromDb()
        {
            if (_dataVaultInterface != null)
            {
                // Get info from db
                _dataVaultInterface.GetAttachmentInfo(out _attachmentInfo, _attachmentId);

                // Get attachment data
                _dataVaultInterface.GetAttachmentData(out _attachmentData, _attachmentId);
            }
        }

        /// <summary>
        /// Populate controls
        /// </summary>
        private void PopulateControls()
        {
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
        }
    }
}
