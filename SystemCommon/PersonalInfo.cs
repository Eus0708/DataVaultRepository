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
        int _id = 0;
        NameInfo _name = null;
        PhoneNumberInfo _phoneNumber = null;
        AddressInfo _address = null;
        SSNNumberInfo _ssn = null;
        DateTime _dateOfBirth = new DateTime();
        List<AttachmentInfo> _attachments = null;

        public int Id
        {
            get { return _id; }
        }

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

        public List<AttachmentInfo> Attachments
        {
            get { return _attachments; }
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

        public PersonalInfo(
            NameInfo name,
            AddressInfo address,
            PhoneNumberInfo phone,
            SSNNumberInfo ssn,
            DateTime dob,
            List<AttachmentInfo> attachments)
        {
            _name = name;
            _address = address;
            _phoneNumber = phone;
            _ssn = ssn;
            _dateOfBirth = dob;
            _attachments = attachments;
        }

        // Deserialize
        public PersonalInfo(SerializationInfo info, StreamingContext context)
        {
            _id = (int)info.GetValue("id", typeof(int));
            _name = (NameInfo)info.GetValue("na", typeof(NameInfo));
            _phoneNumber = (PhoneNumberInfo)info.GetValue("ph", typeof(PhoneNumberInfo));
            _address = (AddressInfo)info.GetValue("ad", typeof(AddressInfo));
            _ssn = (SSNNumberInfo)info.GetValue("ss", typeof(SSNNumberInfo));
            _dateOfBirth = (DateTime)info.GetValue("da", typeof(DateTime));
            _attachments = TryGetValue<List<AttachmentInfo>>(info, "at");
        }

        T TryGetValue<T>(SerializationInfo info, string name)
        {
            try
            {
                return (T)info.GetValue(name, typeof(T));
            }
            catch
            {
                return default(T);
            }
        }

        // Serialize
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("id", _id, typeof(int));
            info.AddValue("na", _name, typeof(NameInfo));
            info.AddValue("ph", _phoneNumber, typeof(PhoneNumberInfo));
            info.AddValue("ad", _address, typeof(AddressInfo));
            info.AddValue("ss", _ssn, typeof(SSNNumberInfo));
            info.AddValue("da", _dateOfBirth, typeof(DateTime));
            if (_attachments != null && _attachments.Count > 0)
            {
                info.AddValue("at", _attachments, typeof(List<AttachmentInfo>));
            }
        }

        public override string ToString()
        {
            return "Id: " + Id + "\n" +
                 "Name: " + Name + "\n" +
                 "Phone: " + PhoneNumber+ "\n" +
                 "Address: " + Address + "\n" +
                 "SSN: " + SSN + "\n" +
                 "Date of Birth: " + DateOfBirth + "\n" +
                 "Attachments: " + Attachments;
        }
        
        public void AddAttachment(AttachmentInfo attachment)
        {
            // If a client does not have any attachments,
            // the list is null
            if (_attachments == null)
            {
                _attachments = new List<AttachmentInfo>();
            }

            _attachments.Add(new AttachmentInfo(
                attachment.Type,
                attachment.Path,
                attachment.Filename
                ));
        }
    }
}
