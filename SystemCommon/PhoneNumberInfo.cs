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
        string _areaCode = String.Empty;
        string _phoneNumber = String.Empty;

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

        public string FullPhoneNumber
        {
            get { return _areaCode + _phoneNumber; }
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
            if (!string.IsNullOrEmpty(_phoneNumber) && _phoneNumber.Length >= 3)
            {
                return "(" + _areaCode + ") " + _phoneNumber.Insert(3, "-");
            }
            else if (_areaCode.Length > 0)
            {
                return "(" + _areaCode + ") " + _phoneNumber;
            }
            else
            {
                return _areaCode + _phoneNumber;
            }
        }
    }
}
