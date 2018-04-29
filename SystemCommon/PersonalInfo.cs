using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SystemCommon
{
    [Serializable]
    public class PersonalInfo : ISerializable
    {
        NameInfo _name = null;
        PhoneNumberInfo _phoneNumber = null;
        AddressInfo _address = null;
        SSNNumberInfo _ssn = null;
        DateTime _dateOfBirth = new DateTime();

        public NameInfo Name
        {
            get { return _name; }
        }

        public PhoneNumberInfo PhoneNumber
        {
            get { return _phoneNumber; }
        }

        public AddressInfo Address
        {
            get { return _address; }
        }

        public SSNNumberInfo SSN
        {
            get { return _ssn; }
        }

        public DateTime DateOfBirth
        {
            get { return _dateOfBirth; }
        }

        public PersonalInfo()
        {
        }

        public PersonalInfo(
            NameInfo name,
            AddressInfo address,
            PhoneNumberInfo phone,
            SSNNumberInfo ssn,
            DateTime dob)
        {
            _name = name;
            _address = address;
            _phoneNumber = phone;
            _ssn = ssn;
            _dateOfBirth = dob;
        }

        // Deserialize
        public PersonalInfo(SerializationInfo info, StreamingContext context)
        {
            _name = (NameInfo)info.GetValue("na", typeof(NameInfo));
            _phoneNumber = (PhoneNumberInfo)info.GetValue("ph", typeof(PhoneNumberInfo));
            _address = (AddressInfo)info.GetValue("ad", typeof(AddressInfo));
            _ssn = (SSNNumberInfo)info.GetValue("ss", typeof(SSNNumberInfo));
            _dateOfBirth = (DateTime)info.GetValue("da", typeof(DateTime));
        }

        // Serialize
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("na", _name, typeof(NameInfo));
            info.AddValue("ph", _phoneNumber, typeof(PhoneNumberInfo));
            info.AddValue("ad", _address, typeof(AddressInfo));
            info.AddValue("ss", _ssn, typeof(SSNNumberInfo));
            info.AddValue("da", _dateOfBirth, typeof(DateTime));
        }

        public override string ToString()
        {
            return "Name: " + Name + "\n" +
                 "Phone: " + PhoneNumber+ "\n" +
                 "Address: " + Address + "\n" +
                 "SSN: " + SSN + "\n" +
                 "Date of Birth: " + DateOfBirth;
        }

    }
}
