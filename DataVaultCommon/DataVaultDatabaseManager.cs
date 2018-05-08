using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

using SystemCommon;

namespace DataVaultCommon
{
    public class DataVaultDatabaseManager
    {
        static string _connectionStr = @"Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|DataVaultDatabase.mdf;Integrated Security=True";
        SqlConnection _connection = null;

        public DataVaultDatabaseManager()
        {
            OpenConnection();
        }

        public void OpenConnection()
        {
            if (_connection == null)
            {
                _connection = new SqlConnection();
                _connection.ConnectionString = _connectionStr;
                _connection.Open();
            }
        }

        public void CloseConnection()
        {
            if (_connection!= null)
            {
                _connection.Close();
                _connection = null;
            }
        }

        ////////////////////////////////////////////////////////////////////
        // Read from database
        ////////////////////////////////////////////////////////////////////

        // Debug purpose
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
                    person.DateOfBirth = reader.GetDateTime(4);
                    person.Address.Address1 = SafeGetString(reader, 5);
                    person.Address.Address2 = SafeGetString(reader, 6);
                    person.Address.City = SafeGetString(reader, 7);
                    person.Address.State = SafeGetString(reader, 8);
                    person.Address.ZipCode = SafeGetString(reader, 9);
                    person.PhoneNumber.AreaCode = SafeGetString(reader, 10);
                    person.PhoneNumber.PhoneNumber = SafeGetString(reader, 11);
                    person.SSN.SSNNumber = SafeGetString(reader, 12);
                    person.DateCreated = reader.GetDateTime(13);
                    person.DateModified = reader.GetDateTime(14);

                    // Add to result list
                    personalInfos.Add(person);
                }
            }
            finally
            {
                // Always call Close when done reading.
                reader.Close();
            }

            // Not yet finished, fill in attachments as well
            foreach(PersonalInfo personalInfo in personalInfos)
            {
                ReloadAttachments(personalInfo.Attachments, personalInfo.Id);
            }
        }

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
                    person.DateOfBirth = reader.GetDateTime(4);
                    person.PhoneNumber.AreaCode = SafeGetString(reader, 5);
                    person.PhoneNumber.PhoneNumber = SafeGetString(reader, 6);
                    person.DateCreated = reader.GetDateTime(7);
                    person.DateModified = reader.GetDateTime(8);

                    // Add to result list
                    personalInfos.Add(person);
                }
            }
            finally
            {
                // Always call Close when done reading.
                reader.Close();
            }
        }

        public void ReloadPersonalInfo(PersonalInfo personalInfo, int personalInfoId)
        {
            string queryString = "LoadPersonalInfoByPersonId";

            // Check connection
            if (_connection == null)
            {
                return;
            }

            SqlCommand command = new SqlCommand(queryString, _connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", personalInfoId);

            SqlDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    personalInfo.Id = reader.GetInt32(0);
                    personalInfo.Name.FirstName = SafeGetString(reader, 1);
                    personalInfo.Name.MiddleName = SafeGetString(reader, 2);
                    personalInfo.Name.LastName = SafeGetString(reader, 3);
                    personalInfo.DateOfBirth = reader.GetDateTime(4);
                    personalInfo.Address.Address1 = SafeGetString(reader, 5);
                    personalInfo.Address.Address2 = SafeGetString(reader, 6);
                    personalInfo.Address.City = SafeGetString(reader, 7);
                    personalInfo.Address.State = SafeGetString(reader, 8);
                    personalInfo.Address.ZipCode = SafeGetString(reader, 9);
                    personalInfo.PhoneNumber.AreaCode = SafeGetString(reader, 10);
                    personalInfo.PhoneNumber.PhoneNumber = SafeGetString(reader, 11);
                    personalInfo.SSN.SSNNumber = SafeGetString(reader, 12);
                    personalInfo.DateCreated = reader.GetDateTime(13);
                    personalInfo.DateModified = reader.GetDateTime(14);
                }
            }
            finally
            {
                // Always call Close when done reading.
                reader.Close();
            }

            // Not yet finished, fill in attachments as well
            ReloadAttachments(personalInfo.Attachments, personalInfoId);
        }

        public void ReloadAttachments(List<AttachmentInfo> attachments, int personalInfoId)
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
                        SafeGetString(reader, 2),
                        SafeGetString(reader, 3))
                        );
                }
            }
            finally
            {
                // Always call Close when done reading.
                reader.Close();
            }
        }

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
            }
        }

        public string SafeGetString(SqlDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
                return reader.GetString(colIndex);
            return string.Empty;
        }

        ////////////////////////////////////////////////////////////////////
        // Write to database
        ////////////////////////////////////////////////////////////////////

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
                    AddPersonalInfo(personalInfo);
                }
                else
                {
                    UpdatePersonalInfo(personalInfo);
                }
            }

            // Update Attachments
            foreach (AttachmentInfo attachment in personalInfo.Attachments)
            {
                SaveAttachmentInfo(personalInfo.Id, attachment);
            }
        }

        public void AddPersonalInfo(PersonalInfo personalInfo)
        {
            string queryString = "InsertPersonalInfo";

            // Check connection and input
            if (_connection == null || personalInfo == null)
            {
                return;
            }

            SqlCommand command = new SqlCommand(queryString, _connection);
            command.CommandType = CommandType.StoredProcedure;
            
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
            command.Parameters.AddWithValue("@DateOfBirth", personalInfo.DateOfBirth);
            command.Parameters.AddWithValue("@DateCreated", DateTime.Now);
            command.Parameters.AddWithValue("@DateModified", DateTime.Now);

            command.ExecuteNonQuery();
        }

        public void UpdatePersonalInfo(PersonalInfo personalInfo)
        {
            string queryString = "UpdatePersonalInfo";

            // Check connection and input
            if (_connection == null || personalInfo == null)
            {
                return;
            }

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
            command.Parameters.AddWithValue("@DateOfBirth", personalInfo.DateOfBirth);
            command.Parameters.AddWithValue("@DateModified", DateTime.Now);

            command.ExecuteNonQuery();
        }

        public void DeletePersonalInfo(PersonalInfo personalInfo)
        {
            string queryString = "DeletePersonalInfoByPersonId";

            // Check connection and input
            if (_connection == null || personalInfo == null)
            {
                return;
            }

            SqlCommand command = new SqlCommand(queryString, _connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", personalInfo.Id);

            command.ExecuteNonQuery();
        }

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

        public void AddAttachment(int personalInfoId, AttachmentInfo attachment)
        {
            string queryString = "InsertAttachment";

            // Check connection and input
            if (_connection == null || attachment == null)
            {
                return;
            }

            SqlCommand command = new SqlCommand(queryString, _connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@PersonalInfoId", personalInfoId);
            command.Parameters.AddWithValue("@Type", attachment.Type);
            command.Parameters.AddWithValue("@Path", attachment.Path);
            command.Parameters.AddWithValue("@Filename", attachment.Filename);

            command.ExecuteNonQuery();
        }

        public void UpdateAttachment(int personalInfoId, AttachmentInfo attachment)
        {
            string queryString = "UpdateAttachment";

            // Check connection and input
            if (_connection == null || attachment == null)
            {
                return;
            }

            SqlCommand command = new SqlCommand(queryString, _connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", attachment.Id);
            command.Parameters.AddWithValue("@PersonalInfoId", personalInfoId);
            command.Parameters.AddWithValue("@Type", attachment.Type);
            command.Parameters.AddWithValue("@Path", attachment.Path);
            command.Parameters.AddWithValue("@Filename", attachment.Filename);

            command.ExecuteNonQuery();
        }

        public void DeleteAttachment(int personalInfoId, AttachmentInfo attachment)
        {
            string queryString = "DeleteAttachmentByAttachmentId";

            // Check connection and input
            if (_connection == null || attachment == null)
            {
                return;
            }

            SqlCommand command = new SqlCommand(queryString, _connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", attachment.Id);
            command.Parameters.AddWithValue("@PersonalInfoId", personalInfoId);

            command.ExecuteNonQuery();
        }

        public void DeleteAttachments(int personalInfoId)
        {
            string queryString = "DeleteAttachmentsByPersonalInfoId";

            // Check connection and input
            if (_connection == null)
            {
                return;
            }

            SqlCommand command = new SqlCommand(queryString, _connection);
            command.CommandType = CommandType.StoredProcedure;
            
            command.Parameters.AddWithValue("@PersonalInfoId", personalInfoId);

            command.ExecuteNonQuery();
        }
    }
}
