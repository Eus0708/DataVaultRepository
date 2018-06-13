using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SystemCommon
{
    [Serializable]
    public class AttachmentTypeInfo : ISerializable
    {
        int _id = -1;
        string _attachmentType = String.Empty;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string AttachmentType
        {
            get { return _attachmentType; }
            set { _attachmentType = value; }
        }

        public AttachmentTypeInfo()
        {
        }

        public AttachmentTypeInfo(
            int id,
            string gender)
        {
            _id = id;
            _attachmentType = gender;
        }

        // Deserialize
        public AttachmentTypeInfo(SerializationInfo info, StreamingContext context)
        {
            _id = (int)info.GetValue("id", typeof(int));
            _attachmentType = (string)info.GetValue("aty", typeof(string));
        }

        // Serialize
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("id", _id, typeof(int));
            info.AddValue("aty", _attachmentType, typeof(string));
        }

        public override string ToString()
        {
            return _attachmentType;
        }
    }
}
