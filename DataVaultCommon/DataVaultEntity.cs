using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

using SystemCommon;

namespace DataVaultCommon
{
    [Serializable]
    public class DataVaultEntity : ISerializable
    {
        string _hashedAppPassword = null;
        List<PersonalInfo> _personalInfos = null;

        public DataVaultEntity()
        {
            _personalInfos = new List<PersonalInfo>();
        }

        // Deserialize
        public DataVaultEntity(SerializationInfo info, StreamingContext context)
        {
            _hashedAppPassword = (string)info.GetValue("ha", typeof(string));
            _personalInfos = (List<PersonalInfo>)info.GetValue("pe", typeof(List<PersonalInfo>));
        }

        // Serialize
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ha", _hashedAppPassword, typeof(string));
            info.AddValue("pe", _personalInfos, typeof(List<PersonalInfo>));
        }

        /// <summary>
        /// This method will not create new object for the input.
        /// Make sure use new PersonalInfo for the method.
        /// </summary>
        /// <param name="info"></param>
        public void AddPersonalInfo(PersonalInfo info)
        {
            // Make sure the list is valid
            if (_personalInfos == null)
            {
                _personalInfos = new List<PersonalInfo>();
            }

            _personalInfos.Add(info);
        }
    }
}
