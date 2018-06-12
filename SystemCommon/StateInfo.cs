using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SystemCommon
{
    [Serializable]
    public class StateInfo : ISerializable
    {
        int _id = -1;
        string _state = String.Empty;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string State
        {
            get { return _state; }
            set { _state = value; }
        }

        public StateInfo()
        {
        }

        public StateInfo(
            int id,
            string state)
        {
            _id = id;
            _state = state;
        }

        // Deserialize
        public StateInfo(SerializationInfo info, StreamingContext context)
        {
            _id = (int)info.GetValue("id", typeof(int));
            _state = (string)info.GetValue("st", typeof(string));
        }

        // Serialize
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("id", _id, typeof(int));
            info.AddValue("st", _state, typeof(string));
        }

        public override string ToString()
        {
            return _state;
        }
    }
}
