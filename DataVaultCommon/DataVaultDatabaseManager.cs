using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            string queryString = "SELECT Person.Id, " +
                "Name.FirstName, Name.MiddleName, Name.LastName, " +
                "Person.DateOfBirth, " +
                "Address.Address1, Address.Address2, Address.City, State.Text, Address.ZipCode, " +
                "Phone.AreaCode, Phone.PhoneNumber, " +
                "Person.SSN, " +
                "Person.DateCreated, Person.DateModified " +
                "FROM dbo.PersonalInfoTable as Person " +
                "LEFT JOIN dbo.AddressTable as Address " +
                "ON Person.AddressId = Address.Id " +
                "LEFT JOIN dbo.NameTable as Name " +
                "ON Person.NameId = Name.Id " +
                "LEFT JOIN dbo.PhoneTable as Phone " +
                "ON Person.PhoneId = Phone.Id " +
                "LEFT JOIN dbo.StateTable as State " +
                "ON Address.StateId = State.Id;";

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
            string queryString = "SELECT Person.Id, " +
                "Name.FirstName, Name.MiddleName, Name.LastName, " +
                "Person.DateOfBirth, " +
                "Phone.AreaCode, Phone.PhoneNumber, " +
                "Person.DateCreated, Person.DateModified " +
                "FROM dbo.PersonalInfoTable as Person " +
                "LEFT JOIN dbo.NameTable as Name " +
                "ON Person.NameId = Name.Id " +
                "LEFT JOIN dbo.PhoneTable as Phone " +
                "ON Person.PhoneId = Phone.Id;";

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
            string queryString = "SELECT Person.Id, " +
                "Name.FirstName, Name.MiddleName, Name.LastName, " +
                "Person.DateOfBirth, " +
                "Address.Address1, Address.Address2, Address.City, State.Text, Address.ZipCode, " +
                "Phone.AreaCode, Phone.PhoneNumber, " +
                "Person.SSN, " +
                "Person.DateCreated, Person.DateModified " +
                "FROM dbo.PersonalInfoTable as Person " +
                "LEFT JOIN dbo.AddressTable as Address " +
                "ON Person.AddressId = Address.Id " +
                "LEFT JOIN dbo.NameTable as Name " +
                "ON Person.NameId = Name.Id " +
                "LEFT JOIN dbo.PhoneTable as Phone " +
                "ON Person.PhoneId = Phone.Id " +
                "LEFT JOIN dbo.StateTable as State " +
                "ON Address.StateId = State.Id " +
                "WHERE Person.Id = @Id; ";

            // Check connection
            if (_connection == null)
            {
                return;
            }

            SqlCommand command = new SqlCommand(queryString, _connection);
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
            string queryString = "SELECT Amt.Id, Text, Path, Filename " +
                "FROM dbo.AttachmentTable as Amt " +
                "LEFT JOIN dbo.AttachmentTypeTable as AmtType " +
                "ON Amt.TypeId = AmtType.Id " +
                "WHERE PersonalInfoId = @Id; ";

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
            string queryString = "SELECT Id, Text FROM dbo.StateTable;";

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
            if (personalInfo.Id == -1)
            {
                AddPersonalInfo(personalInfo);
            }
            else
            {
                ModifyPersonalInfo(personalInfo);
            }
        }

        void AddPersonalInfo(PersonalInfo personalInfo)
        {

        }

        void ModifyPersonalInfo(PersonalInfo personalInfo)
        {
            //string queryString = "UPDATE dbo.NameTable SET " +
            //    //"DateOfBirth = @Dob " +
            //    "MiddleName = @Middle " +
            //    "WHERE Id = @Id;";

            string queryString = "EXECUTE UpdatePersonalInfo 'Omfg', 0; ";

            // Check connection and input
            if (_connection == null || personalInfo == null)
            {
                return;
            }

            using (SqlCommand command = new SqlCommand(queryString, _connection))
            {
                //command.Parameters.AddWithValue("@Id", personalInfo.Id);
                //command.Parameters.AddWithValue("@Middle", personalInfo.Name.MiddleName);
                //command.Parameters.AddWithValue("@Dob", personalInfo.DateOfBirth);

                int rows = command.ExecuteNonQuery();
                Console.WriteLine(rows);
            }
        }
    }
}
