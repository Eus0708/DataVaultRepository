using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SystemCommon
{
    [Serializable]
    public class GenderInfo : ISerializable
    {
        int _id = -1;
        string _gender = String.Empty;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Gender
        {
            get { return _gender; }
            set { _gender = value; }
        }

        public GenderInfo()
        {
        }

        public GenderInfo(
            int id,
            string gender)
        {
            _id = id;
            _gender = gender;
        }

        // Deserialize
        public GenderInfo(SerializationInfo info, StreamingContext context)
        {
            _id = (int)info.GetValue("id", typeof(int));
            _gender = (string)info.GetValue("ge", typeof(string));
        }

        // Serialize
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("id", _id, typeof(int));
            info.AddValue("ge", _gender, typeof(string));
        }

        public override string ToString()
        {
            return _gender;
        }
    }
}
