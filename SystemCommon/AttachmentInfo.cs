using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SystemCommon
{
    public enum AttachmentTypes
    {
        Unknown,
        Image,
        TextFile
    }

    [Serializable]
    public class AttachmentInfo : ISerializable
    {
        AttachmentTypes _type = AttachmentTypes.Unknown;
        string _path = null;
        string _filename = null;

        public AttachmentTypes Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        public string Filename
        {
            get { return _filename; }
            set { _filename = value; }
        }

        public AttachmentInfo()
        {
        }

        public AttachmentInfo(
            AttachmentTypes type,
            string path,
            string filename)
        {
            _type = type;
            _path = path;
            _filename = filename;
        }

        // Deserialize
        public AttachmentInfo(SerializationInfo info, StreamingContext context)
        {
            _type = (AttachmentTypes)info.GetValue("ty", typeof(AttachmentTypes));
            _path = (string)info.GetValue("pa", typeof(string));
            _filename = (string)info.GetValue("fi", typeof(string));
        }

        // Serialize
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ty", _type, typeof(AttachmentTypes));
            info.AddValue("pa", _path, typeof(string));
            info.AddValue("fi", _filename, typeof(string));
        }

        public override string ToString()
        {
            return AttachmentTypesStr(_type) + ": " + 
                _path + " " + 
                _filename;
        }

        public static string AttachmentTypesStr(AttachmentTypes type)
        {
            switch(type)
            {
                case AttachmentTypes.Unknown:  return "Unknown";
                case AttachmentTypes.TextFile: return "Text file";
                case AttachmentTypes.Image:    return "Image";
                default:                       return String.Empty;
            }
        }
    }
}
