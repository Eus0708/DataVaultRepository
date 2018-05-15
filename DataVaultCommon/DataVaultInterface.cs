using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SystemCommon;

namespace DataVaultCommon
{
    public class DataVaultInterface
    {
        DataVaultDatabaseManager _databaseManager = null;

        public DataVaultInterface()
        {
            _databaseManager = new DataVaultDatabaseManager();
        }

        public List<PersonalInfo> GetBriefPersonalInfoList()
        {
            List<PersonalInfo> personalInfos = new List<PersonalInfo>();

            _databaseManager.PartiallyReloadPersonalInfos(personalInfos);

            return personalInfos;
        }

        public bool AddPersonalInfo(PersonalInfo personalInfo)
        {
            _databaseManager.SavePersonalInfo(personalInfo);

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
    }
}
