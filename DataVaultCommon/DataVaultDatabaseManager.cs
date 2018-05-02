using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

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

        public void Test2()
        {
            string queryString = "SELECT Address1, State FROM dbo.AddressTable;";

            SqlCommand command = new SqlCommand(queryString, _connection);
            SqlDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    Console.WriteLine(String.Format("{0}, {1}",
                        reader[0], reader[1]));
                }
            }
            finally
            {
                // Always call Close when done reading.
                reader.Close();
            }
        }

        public void LoadStates(List<string> states)
        {
            string queryString = "SELECT Text FROM dbo.StateTable;";
            
            // Already has data clean it up
            if (states.Count > 0)
            {
                states.Clear();
            }

            SqlCommand command = new SqlCommand( queryString, _connection);
            SqlDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    states.Add(reader.GetString(0));
                }
            }
            finally
            {
                // Always call Close when done reading.
                reader.Close();
            }
        }
    }
}
