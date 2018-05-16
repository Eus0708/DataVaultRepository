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
        int _id = -1;
        bool _toBeDelete = false;
        NameInfo _name = null;
        PhoneNumberInfo _phoneNumber = null;
        AddressInfo _address = null;
        SSNNumberInfo _ssn = null;
        string _gender = null;
        DateTime _dateOfBirth = new DateTime();
        DateTime _dateCreated = new DateTime();
        DateTime _dateModified = new DateTime();
        List<AttachmentInfo> _attachments = null;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public bool ToBeDelete
        {
            get { return _toBeDelete; }
            set { _toBeDelete = value; }
        }

        public NameInfo Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public PhoneNumberInfo PhoneNumber
        {
            get { return _phoneNumber; }
            set { _phoneNumber = value; }
        }

        public AddressInfo Address
        {
            get { return _address; }
            set { _address = value; }
        }

        public SSNNumberInfo SSN
        {
            get { return _ssn; }
            set { _ssn = value; }
        }

        public string Gender
        {
            get { return _gender; }
            set { _gender = value; }
        }

        public DateTime DateOfBirth
        {
            get { return _dateOfBirth; }
            set { _dateOfBirth = value; }
        }

        public DateTime DateCreated
        {
            get { return _dateCreated; }
            set { _dateCreated = value; }
        }

        public DateTime DateModified
        {
            get { return _dateModified; }
            set { _dateModified = value; }
        }

        public List<AttachmentInfo> Attachments
        {
            get { return _attachments; }
            set { _attachments = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public PersonalInfo()
        {
            _name = new NameInfo();
            _address = new AddressInfo();
            _phoneNumber = new PhoneNumberInfo();
            _ssn = new SSNNumberInfo();
            _attachments = new List<AttachmentInfo>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="address"></param>
        /// <param name="phone"></param>
        /// <param name="ssn"></param>
        /// <param name="gender"></param>
        /// <param name="dob"></param>
        /// <param name="created"></param>
        /// <param name="modified"></param>
        public PersonalInfo(
            int id,
            NameInfo name,
            AddressInfo address,
            PhoneNumberInfo phone,
            SSNNumberInfo ssn,
            string gender,
            DateTime dob,
            DateTime created,
            DateTime modified)
        {
            _id = id;
            _name = name;
            _address = address;
            _phoneNumber = phone;
            _ssn = ssn;
            _gender = gender;
            _dateOfBirth = dob;
            _dateCreated = created;
            _dateModified = modified;
            _attachments = new List<AttachmentInfo>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="address"></param>
        /// <param name="phone"></param>
        /// <param name="ssn"></param>
        /// <param name="gender"></param>
        /// <param name="dob"></param>
        /// <param name="created"></param>
        /// <param name="modified"></param>
        /// <param name="attachments"></param>
        public PersonalInfo(
            int id,
            NameInfo name,
            AddressInfo address,
            PhoneNumberInfo phone,
            SSNNumberInfo ssn,
            string gender,
            DateTime dob,
            DateTime created,
            DateTime modified,
            List<AttachmentInfo> attachments)
        {
            _id = id;
            _name = name;
            _address = address;
            _phoneNumber = phone;
            _ssn = ssn;
            _gender = gender;
            _dateOfBirth = dob;
            _dateCreated = created;
            _dateModified = modified;
            _attachments = attachments;
        }

        /// <summary>
        /// Constructor (Deserialize)
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public PersonalInfo(SerializationInfo info, StreamingContext context)
        {
            _id = (int)info.GetValue("id", typeof(int));
            _name = (NameInfo)info.GetValue("na", typeof(NameInfo));
            _phoneNumber = (PhoneNumberInfo)info.GetValue("ph", typeof(PhoneNumberInfo));
            _address = (AddressInfo)info.GetValue("ad", typeof(AddressInfo));
            _ssn = (SSNNumberInfo)info.GetValue("ss", typeof(SSNNumberInfo));
            _gender = (string)info.GetValue("ge", typeof(string));
            _dateOfBirth = (DateTime)info.GetValue("da", typeof(DateTime));
            _dateCreated = (DateTime)info.GetValue("dc", typeof(DateTime));
            _dateModified = (DateTime)info.GetValue("dm", typeof(DateTime));
            _attachments = TryGetValue<List<AttachmentInfo>>(info, "at");
        }

        /// <summary>
        /// Safely get value from serialization info
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <param name="name"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Serialize
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("id", _id, typeof(int));
            info.AddValue("na", _name, typeof(NameInfo));
            info.AddValue("ph", _phoneNumber, typeof(PhoneNumberInfo));
            info.AddValue("ad", _address, typeof(AddressInfo));
            info.AddValue("ss", _ssn, typeof(SSNNumberInfo));
            info.AddValue("ge", _gender, typeof(string));
            info.AddValue("da", _dateOfBirth, typeof(DateTime));
            info.AddValue("dc", _dateCreated, typeof(DateTime));
            info.AddValue("dm", _dateModified, typeof(DateTime));
            if (_attachments != null && _attachments.Count > 0)
            {
                info.AddValue("at", _attachments, typeof(List<AttachmentInfo>));
            }
        }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Id: " + Id + "\n" +
                 "Name: " + Name + "\n" +
                 "Phone: " + PhoneNumber+ "\n" +
                 "Address: " + Address + "\n" +
                 "SSN: " + SSN + "\n" +
                 "Gender: " + Gender + "\n" +
                 "Date of Birth: " + DateOfBirth + "\n" +
                 "Date Created: " + DateCreated + "\n" +
                 "Date Modified: " + DateModified + "\n" +
                 "Attachments: \n" + PrintList(Attachments);
        }

        /// <summary>
        /// Print list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        string PrintList<T>(List<T> list)
        {
            StringBuilder strBuilder = new StringBuilder();
            int i = 0;
            foreach (object obj in list)
            {
                strBuilder.AppendLine("\t[" + (i++) + "]: " + obj.ToString());
            }
            return strBuilder.ToString();
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
                -1,
                attachment.Type,
                attachment.Path,
                attachment.Filename
                ));
        }
    }
}
