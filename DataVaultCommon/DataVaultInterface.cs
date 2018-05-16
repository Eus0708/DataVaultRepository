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

        /// <summary>
        /// Get the list
        /// </summary>
        /// <param name="personalInfos"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Search list with name
        /// </summary>
        /// <param name="personalInfos"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public StatusCode SearchBriefPersonalInfoListWithName(
            out List<PersonalInfo> personalInfos,
            string name)
        {
            StatusCode status = GetBriefPersonalInfoList(out personalInfos);

            if (status != StatusCode.NO_ERROR)
            {
                return status;
            }

            // Find all personal infos that contains the input
            personalInfos = personalInfos.FindAll(
                delegate (PersonalInfo p)
                {
                    if (Contains(p.Name.FirstName, name, StringComparison.OrdinalIgnoreCase)
                    || Contains(p.Name.MiddleName, name, StringComparison.OrdinalIgnoreCase)
                    || Contains(p.Name.LastName, name, StringComparison.OrdinalIgnoreCase)
                    || Contains(p.Name.FullName, name, StringComparison.OrdinalIgnoreCase)
                    || Contains(p.Name.FullNameWithoutMiddle, name, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }

                    return false;
                }
                );

            return StatusCode.NO_ERROR;
        }

        public StatusCode SearchBriefPersonalInfoListWithPhone(
            out List<PersonalInfo> personalInfos,
            string phone)
        {
            personalInfos = null;
            return StatusCode.UNKNOWN_ERROR;
        }

        public StatusCode SearchBriefPersonalInfoListWithSSN(
            out List<PersonalInfo> personalInfos,
            string ssn)
        {

            personalInfos = null;
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

        /// <summary>
        /// Check if a string contain other with caseIgnoring
        /// </summary>
        /// <param name="source"></param>
        /// <param name="toCheck"></param>
        /// <param name="comp"></param>
        /// <returns></returns>
        private bool Contains(string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }
    }
}
