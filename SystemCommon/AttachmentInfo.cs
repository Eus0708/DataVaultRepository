using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

using System.ComponentModel;

namespace SystemCommon
{
    [Serializable]
    public class AttachmentInfo : ISerializable, INotifyPropertyChanged
    {
        int _id = -1;
        bool _toBeDelete = false;
        string _type = String.Empty;
        string _path = String.Empty;
        string _filename = String.Empty;
        string _extension = String.Empty;
        static List<AttachmentTypeInfo> _attachmentTypes = null;

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

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public bool ToBeDelete
        {
            get { return _toBeDelete; }
            set
            {
                _toBeDelete = value;
                NotifyPropertyChanged("ToBeDelete");
            }
        }

        public string Type
        {
            get { return _type; }
            set
            {
                _type = value;
                NotifyPropertyChanged("Type");
            }
        }

        public int TypeIndex
        {
            get
            {
                for (int i = 0; i < AttachmentTypes.Count; i++)
                {
                    if (Type.Equals(AttachmentTypes[i].ToString()))
                    {
                        return i;
                    }
                }
                return -1;
            }
        }

        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        public string FullFilename
        {
            get { return _filename + _extension; }
        }

        public string Extension
        {
            get { return _extension; }
            set { _extension = value; }
        }

        public string Filename
        {
            get { return _filename; }
            set
            {
                _filename = value;
                NotifyPropertyChanged("Filename");
            }
        }

        public List<AttachmentTypeInfo> AttachmentTypes
        {
            get { return _attachmentTypes; }
        }

        public AttachmentInfo()
        {
        }

        public AttachmentInfo(
            int id,
            string type,
            string path,
            string fullFilename)
        {
            _id = id;
            _type = type;
            _path = path;
            _filename = System.IO.Path.GetFileNameWithoutExtension(fullFilename);
            _extension = System.IO.Path.GetExtension(fullFilename);
        }

        public AttachmentInfo(
            int id,
            string type,
            string path,
            string filename,
            string ext)
        {
            _id = id;
            _type = type;
            _path = path;
            _filename = filename;
            _extension = ext;
        }

        // Deserialize
        public AttachmentInfo(SerializationInfo info, StreamingContext context)
        {
            _id = (int)info.GetValue("id", typeof(int));
            _type = (string)info.GetValue("ty", typeof(string));
            _path = (string)info.GetValue("pa", typeof(string));
            _filename = (string)info.GetValue("fi", typeof(string));
            _extension = (string)info.GetValue("ex", typeof(string));
        }

        // Serialize
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("id", _id, typeof(int));
            info.AddValue("ty", _type, typeof(string));
            info.AddValue("pa", _path, typeof(string));
            info.AddValue("fi", _filename, typeof(string));
            info.AddValue("ex", _extension, typeof(string));
        }

        public override string ToString()
        {
            return "[" + _id +"] " +
                _type + " " + 
                _path + " " + 
                FullFilename + (_toBeDelete? "***ToBeDelete" : "");
        }

        public static void SetAttachmentTypes(List<AttachmentTypeInfo> types)
        {
            _attachmentTypes = types;
        }
    }
}
