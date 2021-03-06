﻿using System;
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

        // Search options
        public enum SearchOptionsEnum
        {
            Name = 0,
            Phone = 1,
            SSN = 2,
        }

        public static string[] SearchOptions = {
            "Name",
            "Phone",
            "SSN"
        };

        // Properties
        public bool HasAccess
        {
            get { return _hasAccess; }
        }

        // Contructors
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

        /// <summary>
        /// Search list with phone
        /// </summary>
        /// <param name="personalInfos"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public StatusCode SearchBriefPersonalInfoListWithPhone(
            out List<PersonalInfo> personalInfos,
            string phone)
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
                    if (Contains(p.PhoneNumber.FullPhoneNumber, phone, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }

                    return false;
                }
                );

            return StatusCode.NO_ERROR;
        }

        /// <summary>
        /// Search list with ssn
        /// </summary>
        /// <param name="personalInfos"></param>
        /// <param name="ssn"></param>
        /// <returns></returns>
        public StatusCode SearchBriefPersonalInfoListWithSSN(
            out List<PersonalInfo> personalInfos,
            string ssn)
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
                    if (Contains(p.SSN.SSNNumber, ssn, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }

                    return false;
                }
                );

            return StatusCode.NO_ERROR;
        }

        /// <summary>
        /// Search list
        /// </summary>
        /// <param name="personalInfos"></param>
        /// <param name="str"></param>
        /// <param name="searchOpt"></param>
        /// <returns></returns>
        public StatusCode SearchBriefPersonalInfoList(
            out List<PersonalInfo> personalInfos,
            string str,
            SearchOptionsEnum searchOpt)
        {
            switch(searchOpt)
            {
                case SearchOptionsEnum.Name:
                    return SearchBriefPersonalInfoListWithName(out personalInfos, str);
                case SearchOptionsEnum.Phone:
                    return SearchBriefPersonalInfoListWithPhone(out personalInfos, str);
                case SearchOptionsEnum.SSN:
                    return SearchBriefPersonalInfoListWithSSN(out personalInfos, str);
                default:
                    personalInfos = null;
                    return StatusCode.INVALID_SEARCH_OPT;
            }
        }

        /// <summary>
        /// Get personal info
        /// </summary>
        /// <param name="personalInfo"></param>
        /// <param name="personalInfoId"></param>
        /// <returns></returns>
        public StatusCode GetPersonalInfo(out PersonalInfo personalInfo, int personalInfoId)
        {
            personalInfo = new PersonalInfo();
            if (personalInfoId == -1)
            {
                return StatusCode.NO_ERROR;
            }

            if (!HasAccess)
            {
                return StatusCode.NOT_ALLOW_TO_ACCESS;
            }
            
            if (_databaseManager != null)
            {
                _databaseManager.ReloadPersonalInfo(personalInfo, personalInfoId);
                return StatusCode.NO_ERROR;
            }

            return StatusCode.UNKNOWN_ERROR;
        }

        /// <summary>
        /// Get attachment info
        /// </summary>
        /// <param name="attachmentInfo"></param>
        /// <param name="attachmentId"></param>
        /// <returns></returns>
        public StatusCode GetAttachmentInfo(out AttachmentInfo attachmentInfo, int attachmentId)
        {
            attachmentInfo = new AttachmentInfo();
            if (attachmentId == -1)
            {
                return StatusCode.NO_ERROR;
            }

            if (!HasAccess)
            {
                return StatusCode.NOT_ALLOW_TO_ACCESS;
            }

            if (_databaseManager != null)
            {
                _databaseManager.ReloadAttachment(attachmentInfo, attachmentId);
                return StatusCode.NO_ERROR;
            }

            return StatusCode.UNKNOWN_ERROR;
        }

        /// <summary>
        /// Get attachment data
        /// </summary>
        /// <param name="attachmentData"></param>
        /// <param name="attachmentId"></param>
        /// <returns></returns>
        public StatusCode GetAttachmentData(out byte[] attachmentData, int attachmentId)
        {
            attachmentData = null;
            if (attachmentId == -1)
            {
                return StatusCode.NO_ERROR;
            }

            if (!HasAccess)
            {
                return StatusCode.NOT_ALLOW_TO_ACCESS;
            }

            if (_databaseManager != null)
            {
                _databaseManager.LoadAttachmentData(out attachmentData, attachmentId);
                return StatusCode.NO_ERROR;
            }

            return StatusCode.UNKNOWN_ERROR;
        }

        /// <summary>
        /// Add, Update and Delete a personal info
        /// </summary>
        /// <param name="personalInfo"></param>
        /// <returns></returns>
        public StatusCode ModifyPersonalInfo(PersonalInfo personalInfo)
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

        /// <summary>
        /// Get states list from database
        /// </summary>
        /// <param name="states"></param>
        /// <returns></returns>
        public StatusCode GetStates(out List<StateInfo> states)
        {
            states = null;
            if (!HasAccess)
            {
                return StatusCode.NOT_ALLOW_TO_ACCESS;
            }

            if (_databaseManager != null)
            {
                states = new List<StateInfo>();
                _databaseManager.ReloadStates(states);
                return StatusCode.NO_ERROR;
            }

            return StatusCode.UNKNOWN_ERROR;
        }

        /// <summary>
        /// Get genders list from database
        /// </summary>
        /// <param name="genders"></param>
        /// <returns></returns>
        public StatusCode GetGenders(out List<GenderInfo> genders)
        {
            genders = null;
            if (!HasAccess)
            {
                return StatusCode.NOT_ALLOW_TO_ACCESS;
            }

            if (_databaseManager != null)
            {
                genders = new List<GenderInfo>();
                _databaseManager.ReloadGenders(genders);
                return StatusCode.NO_ERROR;
            }

            return StatusCode.UNKNOWN_ERROR;
        }
        
        /// <summary>
        /// Get attachment types
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public StatusCode GetAttachmentTypes(out List<AttachmentTypeInfo> types)
        {
            types = null;
            if (!HasAccess)
            {
                return StatusCode.NOT_ALLOW_TO_ACCESS;
            }

            if (_databaseManager != null)
            {
                types = new List<AttachmentTypeInfo>();
                _databaseManager.ReloadAttachmentTypes(types);
                return StatusCode.NO_ERROR;
            }

            return StatusCode.UNKNOWN_ERROR;
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

            return StatusCode.INVALID_PASSWORD;
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
