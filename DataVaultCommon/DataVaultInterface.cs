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

        public bool HasAccess
        {
            get { return _hasAccess; }
        }

        public DataVaultInterface()
        {
        }

        public StatusCode GetBriefPersonalInfoList(out List<PersonalInfo> personalInfos)
        {
            // Set to null first
            personalInfos = null;

            if (!HasAccess)
            {
                return StatusCode.NOT_ALLOW_TO_ACCESS;
            }


            if (_databaseManager != null)
            {
                personalInfos = new List<PersonalInfo>();
                _databaseManager.PartiallyReloadPersonalInfos(personalInfos);
                return StatusCode.NO_ERROR;
            }

            return StatusCode.UNKNOWN_ERROR;
        }

        public StatusCode AddPersonalInfo(PersonalInfo personalInfo)
        {
            if (!HasAccess)
            {
                return StatusCode.NOT_ALLOW_TO_ACCESS;
            }

            if (_databaseManager != null)
            {
                _databaseManager.SavePersonalInfo(personalInfo);
                return StatusCode.NO_ERROR;
            }

            return StatusCode.UNKNOWN_ERROR;
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
