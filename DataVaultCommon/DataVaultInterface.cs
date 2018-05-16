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
        bool _hasAccess = false;

        public DataVaultInterface()
        {
        }

        public List<PersonalInfo> GetBriefPersonalInfoList()
        {
            List<PersonalInfo> personalInfos = new List<PersonalInfo>();

            if (_databaseManager != null)
            {
                _databaseManager.PartiallyReloadPersonalInfos(personalInfos);
            }

            return personalInfos;
        }

        public bool AddPersonalInfo(PersonalInfo personalInfo)
        {
            if (_databaseManager != null)
            {
                _databaseManager.SavePersonalInfo(personalInfo);
            }

            return true;
        }

        public bool RemovePersonalInfo()
        {
            return true;
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="appPwd"></param>
        /// <returns></returns>
        public StatusCode Login(string appPwd)
        {
            StatusCode status = VerifyAppPassword(appPwd);
            
            // Password is matched
            if (status == StatusCode.NO_ERROR)
            {
                _databaseManager = new DataVaultDatabaseManager(
                        DataVaultDefines.DBUsername,
                        DataVaultDefines.DBPassword);

                _hasAccess = true;

                return StatusCode.NO_ERROR;
            }

            return status;
        }

        /// <summary>
        /// Used to check password
        /// </summary>
        /// <param name="appPwd"></param>
        /// <returns></returns>
        public StatusCode VerifyAppPassword(string appPwd)
        {
            // Password is matched
            if (String.Equals(appPwd, DataVaultDefines.AppPassword))
            {
                return StatusCode.NO_ERROR;
            }

            return StatusCode.INCORRECT_PASSWORD;
        }
    }
}
