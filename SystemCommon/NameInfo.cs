using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SystemCommon
{
    [Serializable]
    public class NameInfo : ISerializable
    {
        string _firstName = String.Empty;
        string _middleName = String.Empty;
        string _lastName = String.Empty;

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        public string MiddleName
        {
            get { return _middleName; }
            set { _middleName = value; }
        }

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public string FullName
        {
            get
            {
                if (_firstName.Equals(String.Empty)
                    && _middleName.Equals(String.Empty)
                    && _lastName.Equals(String.Empty))
                {
                    return "***Empty Name***";
                }

                return _firstName + " " + _middleName + " " + _lastName;
            }
        }

        public string FullNameWithoutMiddle
        {
            get { return _firstName + " " + _lastName; }
        }

        public NameInfo()
        {
        }

        public NameInfo(
            string first,
            string middle,
            string last)
        {
            _firstName = first;
            _middleName = middle;
            _lastName = last;
        }

        // Deserialize
        public NameInfo(SerializationInfo info, StreamingContext context)
        {
            _firstName = (string)info.GetValue("fn", typeof(string));
            _middleName = (string)info.GetValue("mn", typeof(string));
            _lastName = (string)info.GetValue("ln", typeof(string));
        }

        // Serialize
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("fn", _firstName, typeof(string));
            info.AddValue("mn", _middleName, typeof(string));
            info.AddValue("ln", _lastName, typeof(string));
        }

        public override string ToString()
        {
            return _firstName + " " + _middleName + " " + _lastName;
        }
    }
}
