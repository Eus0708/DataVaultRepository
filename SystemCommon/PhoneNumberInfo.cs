using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SystemCommon
{
    [Serializable]
    public class PhoneNumberInfo : ISerializable
    {
        string _areaCode = null;
        string _phoneNumber = null;

        public string AreaCode
        {
            get { return _areaCode; }
            set { _areaCode = value; }
        }

        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { _phoneNumber = value; }
        }

        public PhoneNumberInfo()
        {
        }

        public PhoneNumberInfo(
            string areaCode,
            string phoneNumber)
        {
            _areaCode = areaCode;
            _phoneNumber = phoneNumber;
        }

        // Deserialize
        public PhoneNumberInfo(SerializationInfo info, StreamingContext context)
        {
            _areaCode = (string)info.GetValue("ar", typeof(string));
            _phoneNumber = (string)info.GetValue("ph", typeof(string));
        }

        // Serialize
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ar", _areaCode, typeof(string));
            info.AddValue("ph", _phoneNumber, typeof(string));
        }

        public override string ToString()
        {
            return "(" + _areaCode + ")" + _phoneNumber;
        }
    }
}
