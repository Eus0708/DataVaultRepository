using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVaultCommon
{
    public class DataVaultInterface
    {
        DataVaultEntity _entity = null;

        public DataVaultInterface()
        {
            _entity = new DataVaultEntity();
        }

        public bool AddPersonalInfo()
        {
            return true;
        }

        public bool RemovePersonalInfo()
        {
            return true;
        }

        public bool Login()
        {
            return true;
        }

        public bool VerifyAppPassword()
        {
            return true;
        }

        bool DecryptEntity()
        {
            return true;
        }

        bool EncryptEntity()
        {
            return true;
        }
    }
}
