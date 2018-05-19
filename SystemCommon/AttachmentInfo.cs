using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SystemCommon
{
    [Serializable]
    public class AttachmentInfo : ISerializable
    {
        int _id = -1;
        bool _toBeDelete = false;
        string _type = null;
        string _path = null;
        string _filename = null;
        static List<AttachmentTypeInfo> _attachmentTypes = null;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public bool ToBeDelete
        {
            get { return _toBeDelete; }
            set { _toBeDelete = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        public string FullFilename
        {
            get { return _filename; }
            set { _filename = value; }
        }

        public string Filename
        {
            get { return System.IO.Path.GetFileNameWithoutExtension(_filename); }
        }

        public string Extension
        {
            get { return System.IO.Path.GetExtension(_filename); }
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
            string filename)
        {
            _id = id;
            _type = type;
            _path = path;
            _filename = filename;
        }

        // Deserialize
        public AttachmentInfo(SerializationInfo info, StreamingContext context)
        {
            _id = (int)info.GetValue("id", typeof(int));
            _type = (string)info.GetValue("ty", typeof(string));
            _path = (string)info.GetValue("pa", typeof(string));
            _filename = (string)info.GetValue("fi", typeof(string));
        }

        // Serialize
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("id", _id, typeof(int));
            info.AddValue("ty", _type, typeof(string));
            info.AddValue("pa", _path, typeof(string));
            info.AddValue("fi", _filename, typeof(string));
        }

        public override string ToString()
        {
            return "[" + _id +"] " +
                _type + " " + 
                _path + " " + 
                _filename + (_toBeDelete? "***ToBeDelete" : "");
        }

        public static void SetAttachmentTypes(List<AttachmentTypeInfo> types)
        {
            _attachmentTypes = types;
        }
    }
}
