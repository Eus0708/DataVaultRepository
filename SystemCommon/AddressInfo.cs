using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SystemCommon
{
    [Serializable]
    public class AddressInfo : ISerializable
    {
        string _address1 = null;
        string _address2 = null;
        string _zipCode = null;
        string _city = null;
        string _state = null;

        public string Address1
        {
            get { return _address1; }
            set { _address1 = value; }
        }

        public string Address2
        {
            get { return _address2; }
            set { _address2 = value; }
        }

        public string ZipCode
        {
            get { return _zipCode; }
            set { _zipCode = value; }
        }

        public string City
        {
            get { return _city; }
            set { _city = value; }
        }

        public string State
        {
            get { return _state; }
            set { _state = value; }
        }

        public AddressInfo()
        {
        }

        public AddressInfo(
            string address1,
            string address2,
            string city,
            string state,
            string zipCode)
        {
            _address1 = address1;
            _address2 = address2;
            _city = city;
            _state = state;
            _zipCode = zipCode;
        }

        // Deserialize
        public AddressInfo(SerializationInfo info, StreamingContext context)
        {
            _address1 = (string)info.GetValue("a1", typeof(string));
            _address2 = (string)info.GetValue("a2", typeof(string));
            _city = (string)info.GetValue("ci", typeof(string));
            _state = (string)info.GetValue("st", typeof(string));
            _zipCode = (string)info.GetValue("zi", typeof(string));
        }

        // Serialize
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("a1", _address1, typeof(string));
            info.AddValue("a2", _address2, typeof(string));
            info.AddValue("ci", _city, typeof(string));
            info.AddValue("st", _state, typeof(string));
            info.AddValue("zi", _zipCode, typeof(string));
        }

        public override string ToString()
        {
            return _address1 + " " + _address2 + " " + _city + " " + _state + " " + _zipCode;
        }
    }
}
