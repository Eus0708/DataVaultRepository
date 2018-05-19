using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SystemCommon
{
    [Serializable]
    public class SSNNumberInfo : ISerializable
    {
        string _ssnNumber = null;

        public string SSNNumber
        {
            get { return _ssnNumber; }
            set { _ssnNumber = value; }
        }

        public SSNNumberInfo()
        {
        }

        public SSNNumberInfo(string ssn)
        {
            _ssnNumber = ssn;
        }

        // Deserialize
        public SSNNumberInfo(SerializationInfo info, StreamingContext context)
        {
            _ssnNumber = (string)info.GetValue("ss", typeof(string));
        }

        // Serialize
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ss", _ssnNumber, typeof(string));
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(_ssnNumber) && _ssnNumber.Length >= 5)
            {
                return _ssnNumber.Insert(5, "-").Insert(3, "-");
            }
            else
            {
                return _ssnNumber;
            }
        }
    }
}
