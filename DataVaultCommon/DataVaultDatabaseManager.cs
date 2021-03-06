﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Collections.ObjectModel;

using SystemCommon;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DataVaultTest")]

namespace DataVaultCommon
{
    internal class DataVaultDatabaseManager : IDisposable
    {
        static string _connectionStr = @"Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|DataVaultDatabase.mdf;Integrated Security=True";
        SqlConnection _connection = new SqlConnection(_connectionStr);
        string _dbUsername = null;
        string _dbPassword = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public DataVaultDatabaseManager(
            string username,
            string password)
        {
            _dbUsername = username;
            _dbPassword = password;

            OpenConnection();
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            // Close db connection
            CloseConnection();
        }

        /// <summary>
        /// Open connection
        /// </summary>
        public void OpenConnection()
        {
            if (_connection != null && _connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
        }

        /// <summary>
        /// Close connection
        /// </summary>
        public void CloseConnection()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        ////////////////////////////////////////////////////////////////////
        // Read from database
        ////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Get all data from database
        /// Debug use only
        /// </summary>
        /// <param name="personalInfos"></param>
        public void Debug_FullyReloadPersonalInfos(List<PersonalInfo> personalInfos)
        {
            string queryString = "FullyLoadPersonalInfos";

            // Already has data clean it up
            if (personalInfos.Count > 0)
            {
                personalInfos.Clear();
            }

            // Check connection
            if (_connection == null)
            {
                return;
            }

            OpenConnection();

            SqlCommand command = new SqlCommand(queryString, _connection);
            command.CommandType = CommandType.StoredProcedure;

            SqlDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    PersonalInfo person = new PersonalInfo();

                    person.Id = reader.GetInt32(0);
                    person.Name.FirstName = SafeGetString(reader, 1);
                    person.Name.MiddleName = SafeGetString(reader, 2);
                    person.Name.LastName = SafeGetString(reader, 3);
                    person.DateOfBirth = SafeGetDateTime(reader, 4);
                    person.Address.Address1 = SafeGetString(reader, 5);
                    person.Address.Address2 = SafeGetString(reader, 6);
                    person.Address.City = SafeGetString(reader, 7);
                    person.Address.State = SafeGetString(reader, 8);
                    person.Address.ZipCode = SafeGetString(reader, 9);
                    person.PhoneNumber.AreaCode = SafeGetString(reader, 10);
                    person.PhoneNumber.PhoneNumber = SafeGetString(reader, 11);
                    person.SSN.SSNNumber = SafeGetString(reader, 12);
                    person.DateCreated = SafeGetDateTime(reader, 13);
                    person.DateModified = SafeGetDateTime(reader, 14);
                    person.Gender = SafeGetString(reader, 15);

                    // Add to result list
                    personalInfos.Add(person);
                }
            }
            finally
            {
                // Always call Close when done reading.
                reader.Close();

                CloseConnection();
            }

            // Not yet finished, fill in attachments as well
            foreach(PersonalInfo personalInfo in personalInfos)
            {
                ReloadAttachments(personalInfo.Attachments, personalInfo.Id);
            }
        }

        /// <summary>
        /// Load main personal info list
        /// </summary>
        /// <param name="personalInfos"></param>
        public void PartiallyReloadPersonalInfos(List<PersonalInfo> personalInfos)
        {
            string queryString = "PartiallyLoadPersonalInfos";

            // Already has data clean it up
            if (personalInfos.Count > 0)
            {
                personalInfos.Clear();
            }

            // Check connection
            if (_connection == null)
            {
                return;
            }

            OpenConnection();

            SqlCommand command = new SqlCommand(queryString, _connection);
            command.CommandType = CommandType.StoredProcedure;

            SqlDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    PersonalInfo person = new PersonalInfo();

                    person.Id = reader.GetInt32(0);
                    person.Name.FirstName = SafeGetString(reader, 1);
                    person.Name.MiddleName = SafeGetString(reader, 2);
                    person.Name.LastName = SafeGetString(reader, 3);
                    person.DateOfBirth = SafeGetDateTime(reader, 4);
                    person.PhoneNumber.AreaCode = SafeGetString(reader, 5);
                    person.PhoneNumber.PhoneNumber = SafeGetString(reader, 6);
                    person.DateCreated = SafeGetDateTime(reader, 7);
                    person.DateModified = SafeGetDateTime(reader, 8);
                    person.SSN.SSNNumber = SafeGetString(reader, 9);
                    person.Gender = SafeGetString(reader, 10);

                    // Add to result list
                    personalInfos.Add(person);
                }
            }
            finally
            {
                // Always call Close when done reading.
                reader.Close();

                CloseConnection();
            }
        }

        /// <summary>
        /// Get info for a person
        /// </summary>
        /// <param name="personalInfo"></param>
        /// <param name="personalInfoId"></param>
        public void ReloadPersonalInfo(PersonalInfo personalInfo, int personalInfoId)
        {
            string queryString = "LoadPersonalInfoByPersonId";

            // Check connection
            if (_connection == null)
            {
                return;
            }

            OpenConnection();

            SqlCommand command = new SqlCommand(queryString, _connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", personalInfoId);

            SqlDataReader reader = command.ExecuteReader();
            try
            {
                if (reader.Read())
                {
                    personalInfo.Id = reader.GetInt32(0);
                    personalInfo.Name.FirstName = SafeGetString(reader, 1);
                    personalInfo.Name.MiddleName = SafeGetString(reader, 2);
                    personalInfo.Name.LastName = SafeGetString(reader, 3);
                    personalInfo.DateOfBirth = SafeGetDateTime(reader, 4);
                    personalInfo.Address.Address1 = SafeGetString(reader, 5);
                    personalInfo.Address.Address2 = SafeGetString(reader, 6);
                    personalInfo.Address.City = SafeGetString(reader, 7);
                    personalInfo.Address.State = SafeGetString(reader, 8);
                    personalInfo.Address.ZipCode = SafeGetString(reader, 9);
                    personalInfo.PhoneNumber.AreaCode = SafeGetString(reader, 10);
                    personalInfo.PhoneNumber.PhoneNumber = SafeGetString(reader, 11);
                    personalInfo.SSN.SSNNumber = SafeGetString(reader, 12);
                    personalInfo.DateCreated = SafeGetDateTime(reader, 13);
                    personalInfo.DateModified = SafeGetDateTime(reader, 14);
                    personalInfo.Gender = SafeGetString(reader, 15);
                }
            }
            finally
            {
                // Always call Close when done reading.
                reader.Close();

                CloseConnection();
            }

            // Not yet finished, fill in attachments as well
            ReloadAttachments(personalInfo.Attachments, personalInfoId);
        }

        /// <summary>
        /// Load all attachment from a person
        /// </summary>
        /// <param name="attachments"></param>
        /// <param name="personalInfoId"></param>
        public void ReloadAttachments(ObservableCollection<AttachmentInfo> attachments, int personalInfoId)
        {
            string queryString = "LoadAttachmentsByPersonId";

            // Already has data clean it up
            if (attachments.Count > 0)
            {
                attachments.Clear();
            }

            // Check connection
            if (_connection == null)
            {
                return;
            }

            OpenConnection();

            SqlCommand command = new SqlCommand(queryString, _connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", personalInfoId);

            SqlDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    attachments.Add(
                        new AttachmentInfo(
                        reader.GetInt32(0),
                        SafeGetString(reader, 1),
                        string.Empty,
                        SafeGetString(reader, 2))
                        );
                }
            }
            finally
            {
                // Always call Close when done reading.
                reader.Close();

                CloseConnection();
            }
        }

        /// <summary>
        /// Load attachment info
        /// </summary>
        /// <param name="attachment"></param>
        /// <param name="attachmentId"></param>
        public void ReloadAttachment(AttachmentInfo attachment, int attachmentId)
        {
            string queryString = "LoadAttachmentByAttachmentId";

            // Check connection and input
            if (_connection == null || attachment == null)
            {
                return;
            }

            OpenConnection();

            SqlCommand command = new SqlCommand(queryString, _connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", attachmentId);

            SqlDataReader reader = command.ExecuteReader();
            try
            {
                if (reader.Read())
                {
                    attachment.Id = reader.GetInt32(0);
                    attachment.Type = SafeGetString(reader, 1);
                    
                    string fullFilename = SafeGetString(reader, 2);
                    attachment.Filename = Path.GetFileNameWithoutExtension(fullFilename);
                    attachment.Extension = Path.GetExtension(fullFilename);
                }
            }
            finally
            {
                // Always call Close when done reading.
                reader.Close();

                CloseConnection();
            }
        }

        /// <summary>
        /// Load attachment data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="attachmentId"></param>
        public void LoadAttachmentData(out byte[] data, int attachmentId)
        {
            string queryString = "LoadAttachmentDataByAttachmentId";

            data = null;

            // Check connection and input
            if (_connection == null)
            {
                return;
            }

            OpenConnection();

            SqlCommand command = new SqlCommand(queryString, _connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", attachmentId);

            SqlDataReader reader = command.ExecuteReader();
            try
            {
                if (reader.Read())
                {
                    data = reader.GetValue(0) as byte[];
                }
            }
            finally
            {
                // Always call Close when done reading.
                reader.Close();

                CloseConnection();
            }
        }

        /// <summary>
        /// Load all states
        /// </summary>
        /// <param name="states"></param>
        public void ReloadStates(List<StateInfo> states)
        {
            string queryString = "LoadStates";

            // Already has data clean it up
            if (states.Count > 0)
            {
                states.Clear();
            }

            if (_connection == null)
            {
                return;
            }

            OpenConnection();

            SqlCommand command = new SqlCommand( queryString, _connection);
            command.CommandType = CommandType.StoredProcedure;

            SqlDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    states.Add(
                        new StateInfo(
                        reader.GetInt32(0),
                        SafeGetString(reader, 1))
                        );
                }
            }
            finally
            {
                // Always call Close when done reading.
                reader.Close();

                CloseConnection();
            }
        }

        /// <summary>
        /// Load all genders
        /// </summary>
        /// <param name="genders"></param>
        public void ReloadGenders(List<GenderInfo> genders)
        {
            string queryString = "LoadGenders";

            // Already has data clean it up
            if (genders.Count > 0)
            {
                genders.Clear();
            }

            if (_connection == null)
            {
                return;
            }

            OpenConnection();

            SqlCommand command = new SqlCommand(queryString, _connection);
            command.CommandType = CommandType.StoredProcedure;

            SqlDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    genders.Add(
                        new GenderInfo(
                        reader.GetInt32(0),
                        SafeGetString(reader, 1))
                        );
                }
            }
            finally
            {
                // Always call Close when done reading.
                reader.Close();

                CloseConnection();
            }
        }

        /// <summary>
        /// Reload attachment types
        /// </summary>
        /// <param name="genders"></param>
        public void ReloadAttachmentTypes(List<AttachmentTypeInfo> types)
        {
            string queryString = "LoadAttachmentTypes";

            // Already has data clean it up
            if (types.Count > 0)
            {
                types.Clear();
            }

            if (_connection == null)
            {
                return;
            }

            OpenConnection();

            SqlCommand command = new SqlCommand(queryString, _connection);
            command.CommandType = CommandType.StoredProcedure;

            SqlDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    types.Add(
                        new AttachmentTypeInfo(
                        reader.GetInt32(0),
                        SafeGetString(reader, 1))
                        );
                }
            }
            finally
            {
                // Always call Close when done reading.
                reader.Close();

                CloseConnection();
            }
        }



        /// <summary>
        /// Get string from db data type safely
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="colIndex"></param>
        /// <returns></returns>
        public string SafeGetString(SqlDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
                return reader.GetString(colIndex);
            return string.Empty;
        }

        /// <summary>
        /// Get dateTime from db data type safely
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="colIndex"></param>
        /// <returns></returns>
        public DateTime? SafeGetDateTime(SqlDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
                return reader.GetDateTime(colIndex);
            return null;
        }

        ////////////////////////////////////////////////////////////////////
        // Write to database
        ////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Save a personal info for a person
        /// </summary>
        /// <param name="personalInfo"></param>
        public void SavePersonalInfo(PersonalInfo personalInfo)
        {
            // Update personal info table
            if (personalInfo.ToBeDelete)
            {
                DeletePersonalInfo(personalInfo);
            }
            else
            {
                if (personalInfo.Id == -1)
                {
                    int id = AddPersonalInfo(personalInfo);

                    // Update the id
                    personalInfo.Id = id;
                }
                else
                {
                    UpdatePersonalInfo(personalInfo);
                }
            }

            // Update Attachments
            if (personalInfo.Id != -1)
            {
                foreach (AttachmentInfo attachment in personalInfo.Attachments)
                {
                    SaveAttachmentInfo(personalInfo.Id, attachment);
                }
            }
        }

        /// <summary>
        /// Insert a person
        /// </summary>
        /// <param name="personalInfo"></param>
        /// <returns></returns>
        public int AddPersonalInfo(PersonalInfo personalInfo)
        {
            string queryString = "InsertPersonalInfo";

            // Check connection and input
            if (_connection == null || personalInfo == null)
            {
                return -1;
            }

            OpenConnection();

            SqlCommand command = new SqlCommand(queryString, _connection);
            command.CommandType = CommandType.StoredProcedure;
            
            // Input vars
            command.Parameters.AddWithValue("@FirstName", personalInfo.Name.FirstName);
            command.Parameters.AddWithValue("@MiddleName", personalInfo.Name.MiddleName);
            command.Parameters.AddWithValue("@LastName", personalInfo.Name.LastName);
            command.Parameters.AddWithValue("@Address1", personalInfo.Address.Address1);
            command.Parameters.AddWithValue("@Address2", personalInfo.Address.Address2);
            command.Parameters.AddWithValue("@City", personalInfo.Address.City);
            command.Parameters.AddWithValue("@State", personalInfo.Address.State);
            command.Parameters.AddWithValue("@ZipCode", personalInfo.Address.ZipCode);
            command.Parameters.AddWithValue("@AreaCode", personalInfo.PhoneNumber.AreaCode);
            command.Parameters.AddWithValue("@PhoneNumber", personalInfo.PhoneNumber.PhoneNumber);
            command.Parameters.AddWithValue("@SSN", personalInfo.SSN.SSNNumber);
            command.Parameters.AddWithValue("@Gender", personalInfo.Gender);
            command.Parameters.AddWithValue("@DateOfBirth", personalInfo.DateOfBirth);
            command.Parameters.AddWithValue("@DateCreated", DateTime.Now);
            command.Parameters.AddWithValue("@DateModified", DateTime.Now);

            // Output vars
            command.Parameters.Add("@PersonalId", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

            // Exec
            command.ExecuteNonQuery();

            // Get output
            int personalId = (int)command.Parameters["@PersonalId"].Value;

            CloseConnection();

            return personalId;
        }

        /// <summary>
        /// Update a pserson
        /// </summary>
        /// <param name="personalInfo"></param>
        public void UpdatePersonalInfo(PersonalInfo personalInfo)
        {
            string queryString = "UpdatePersonalInfo";

            // Check connection and input
            if (_connection == null || personalInfo == null)
            {
                return;
            }

            OpenConnection();

            SqlCommand command = new SqlCommand(queryString, _connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", personalInfo.Id);
            command.Parameters.AddWithValue("@FirstName", personalInfo.Name.FirstName);
            command.Parameters.AddWithValue("@MiddleName", personalInfo.Name.MiddleName);
            command.Parameters.AddWithValue("@LastName", personalInfo.Name.LastName);
            command.Parameters.AddWithValue("@Address1", personalInfo.Address.Address1);
            command.Parameters.AddWithValue("@Address2", personalInfo.Address.Address2);
            command.Parameters.AddWithValue("@City", personalInfo.Address.City);
            command.Parameters.AddWithValue("@State", personalInfo.Address.State);
            command.Parameters.AddWithValue("@ZipCode", personalInfo.Address.ZipCode);
            command.Parameters.AddWithValue("@AreaCode", personalInfo.PhoneNumber.AreaCode);
            command.Parameters.AddWithValue("@PhoneNumber", personalInfo.PhoneNumber.PhoneNumber);
            command.Parameters.AddWithValue("@SSN", personalInfo.SSN.SSNNumber);
            command.Parameters.AddWithValue("@Gender", personalInfo.Gender);
            command.Parameters.AddWithValue("@DateOfBirth", personalInfo.DateOfBirth);
            command.Parameters.AddWithValue("@DateModified", DateTime.Now);

            command.ExecuteNonQuery();

            CloseConnection();
        }

        /// <summary>
        /// Delete a person
        /// </summary>
        /// <param name="personalInfo"></param>
        public void DeletePersonalInfo(PersonalInfo personalInfo)
        {
            string queryString = "DeletePersonalInfoByPersonId";

            // Check connection and input
            if (_connection == null || personalInfo == null)
            {
                return;
            }

            OpenConnection();

            SqlCommand command = new SqlCommand(queryString, _connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", personalInfo.Id);

            command.ExecuteNonQuery();

            CloseConnection();
        }

        /// <summary>
        /// Save an attachment
        /// </summary>
        /// <param name="personalInfoId"></param>
        /// <param name="attachment"></param>
        public void SaveAttachmentInfo(int personalInfoId, AttachmentInfo attachment)
        {
            // Update attachment table
            if (attachment.ToBeDelete)
            {
                DeleteAttachment(personalInfoId, attachment);
            }
            else
            {
                if (attachment.Id == -1)
                {
                    AddAttachment(personalInfoId, attachment);
                }
                else
                {
                    UpdateAttachment(personalInfoId, attachment);
                }
            }
        }

        /// <summary>
        /// Add an attachment
        /// </summary>
        /// <param name="personalInfoId"></param>
        /// <param name="attachment"></param>
        public void AddAttachment(int personalInfoId, AttachmentInfo attachment)
        {
            string queryString = "InsertAttachment";

            // Check connection and input
            if (_connection == null || attachment == null)
            {
                return;
            }

            OpenConnection();

            SqlCommand command = new SqlCommand(queryString, _connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@PersonalInfoId", personalInfoId);
            command.Parameters.AddWithValue("@Type", attachment.Type);
            command.Parameters.AddWithValue("@Filename", attachment.FullFilename);

            byte[] data = GetImageData(attachment.Path);
            command.Parameters.AddWithValue("@Data", data);

            command.ExecuteNonQuery();

            CloseConnection();
        }

        /// <summary>
        /// Update an attachment
        /// </summary>
        /// <param name="personalInfoId"></param>
        /// <param name="attachment"></param>
        public void UpdateAttachment(int personalInfoId, AttachmentInfo attachment)
        {
            string queryString = "UpdateAttachment";

            // Check connection and input
            if (_connection == null || attachment == null)
            {
                return;
            }

            OpenConnection();

            SqlCommand command = new SqlCommand(queryString, _connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", attachment.Id);
            command.Parameters.AddWithValue("@PersonalInfoId", personalInfoId);
            command.Parameters.AddWithValue("@Type", attachment.Type);
            command.Parameters.AddWithValue("@Filename", attachment.FullFilename);

            command.ExecuteNonQuery();

            CloseConnection();
        }

        /// <summary>
        /// Delete an attachment
        /// </summary>
        /// <param name="personalInfoId"></param>
        /// <param name="attachment"></param>
        public void DeleteAttachment(int personalInfoId, AttachmentInfo attachment)
        {
            string queryString = "DeleteAttachmentByAttachmentId";

            // Check connection and input
            if (_connection == null || attachment == null)
            {
                return;
            }

            OpenConnection();

            SqlCommand command = new SqlCommand(queryString, _connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", attachment.Id);
            command.Parameters.AddWithValue("@PersonalInfoId", personalInfoId);

            command.ExecuteNonQuery();

            CloseConnection();
        }

        /// <summary>
        /// Delete all attachments
        /// </summary>
        /// <param name="personalInfoId"></param>
        public void DeleteAttachments(int personalInfoId)
        {
            string queryString = "DeleteAttachmentsByPersonalInfoId";

            // Check connection and input
            if (_connection == null)
            {
                return;
            }

            OpenConnection();

            SqlCommand command = new SqlCommand(queryString, _connection);
            command.CommandType = CommandType.StoredProcedure;
            
            command.Parameters.AddWithValue("@PersonalInfoId", personalInfoId);

            command.ExecuteNonQuery();

            CloseConnection();
        }

        /// <summary>
        /// Giving the file path return the byte[]
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private byte[] GetImageData(string path)
        {
            Image image = new Bitmap(path);
            MemoryStream memStream = new MemoryStream();
            image.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            return memStream.ToArray();
        }
    }
}
