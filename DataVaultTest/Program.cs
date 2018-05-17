using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

using SystemCommon;
using DataVaultCommon;

namespace DataVaultTest
{
    class Program
    {
        static DataVaultDatabaseManager db = new DataVaultDatabaseManager("", "");
        static string _connectionStr = @"Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|DataVaultDatabase.mdf;Integrated Security=True";

        static void Main(string[] args)
        {
            Test4();
            Seperator();
            Test23();
            Seperator();
            Test4();

            db.CloseConnection();
            Console.Read();
            //Console.WriteLine("Done DataVault Test");
        }

        // Delete a person
        static void Test23()
        {
            DataVaultInterface i = new DataVaultInterface();
            i.Login("123");
            PersonalInfo p;
            i.GetPersonalInfo(out p, 0);
            Console.WriteLine(p);
            Seperator();
            p.Name.FirstName = "TestF";
            p.Name.MiddleName = "TestM";
            p.Name.LastName = "TestL";
            p.ToBeDelete = true;
            Console.WriteLine(p);
            Seperator();
            i.ModifyPersonalInfo(p);
        }

        // Modify a person
        static void Test22()
        {
            DataVaultInterface i = new DataVaultInterface();
            i.Login("123");
            PersonalInfo p;
            i.GetPersonalInfo(out p, 0);
            Console.WriteLine(p);
            Seperator();
            p.Name.FirstName = "TestF";
            p.Name.MiddleName = "TestM";
            p.Name.LastName = "TestL";
            Console.WriteLine(p);
            Seperator();
            i.ModifyPersonalInfo(p);
        }

        // Add a person
        static void Test21()
        {
            DataVaultInterface i = new DataVaultInterface();
            i.Login("123");
            PersonalInfo p = new PersonalInfo();
            p.Name.FirstName = "TestF";
            p.Name.MiddleName = "TestM";
            p.Name.LastName = "TestL";
            i.ModifyPersonalInfo(p);
        }

        // Search for all kind
        static void Test20()
        {
            DataVaultInterface i = new DataVaultInterface();
            i.Login("123");
            List<PersonalInfo> infos;
            i.SearchBriefPersonalInfoList(out infos, "first la", DataVaultInterface.SearchOptionsEnum.Name);
            PrintList(infos);
            Seperator();
            i.SearchBriefPersonalInfoList(out infos, "23", DataVaultInterface.SearchOptionsEnum.Phone);
            PrintList(infos);
            Seperator();
            i.SearchBriefPersonalInfoList(out infos, "000", DataVaultInterface.SearchOptionsEnum.SSN);
            PrintList(infos);
        }

        static void Test19()
        {
            DataVaultInterface i = new DataVaultInterface();
            i.Login("123");
            List<PersonalInfo> infos;
            i.SearchBriefPersonalInfoListWithSSN(out infos, "000");
            PrintList(infos);
        }

        static void Test18()
        {
            DataVaultInterface i = new DataVaultInterface();
            i.Login("123");
            List<PersonalInfo> infos;
            i.SearchBriefPersonalInfoListWithPhone(out infos, "723");
            PrintList(infos);
        }

        static void Test17()
        {
            PersonalInfo p = new PersonalInfo();
            p.PhoneNumber.AreaCode = "123";
            p.PhoneNumber.PhoneNumber = "4445555";
            Console.WriteLine(p.PhoneNumber.FullPhoneNumber);
        }

        static void Seperator()
        {
            Console.WriteLine("********************* Seperator **********************");
        }

        static void Test16()
        {
            DataVaultInterface i = new DataVaultInterface();
            i.Login("123");
            List<PersonalInfo> infos;
            i.SearchBriefPersonalInfoListWithName(out infos, "first last");
            PrintList(infos);
        }

        static void Test15()
        {
            DataVaultInterface i = new DataVaultInterface();
            i.Login("123");
            List<PersonalInfo> infos;
            i.GetBriefPersonalInfoList(out infos);
            PrintList(infos);
        }

        // Interface testing begin

        // Database manager testing end

        // Delete all attachment with same id
        static void Test14()
        {
            db.DeleteAttachments(0);
        }

        // Delete one attachment
        static void Test13()
        {
            AttachmentInfo attach = new AttachmentInfo();
            attach.Id = 0;
            attach.ToBeDelete = true;
            db.SaveAttachmentInfo(0, attach);
        }

        // Update attachment
        static void Test12()
        {
            AttachmentInfo attach = new AttachmentInfo();
            attach.Id = 0;
            attach.Path = @"\Path\Images";
            attach.Filename = "attach.jpg";
            attach.Type = "Passport";
            db.SaveAttachmentInfo(0, attach);
        }

        // Add Attachment to person[1]
        static void Test11()
        {
            AttachmentInfo attach = new AttachmentInfo();
            attach.Path = @"\Path\Images";
            attach.Filename = "attach.jpg";
            attach.Type = "Passport";
            db.SaveAttachmentInfo(1, attach);
        }

        // Save a person (Delete)
        static void Test10()
        {
            Console.WriteLine("****** Delete person[4]");

            PersonalInfo person = new PersonalInfo();
            person.Id = 4;
            person.ToBeDelete = true;
            db.SavePersonalInfo(person);
        }

        // Save a person (Insert)
        static void Test9()
        {
            PersonalInfo person = new PersonalInfo();
            person.Name.FirstName = "New";
            person.Name.MiddleName = "Test";
            person.Name.LastName = "Name";
            person.Address.Address1 = "888 Sun Star";
            person.Address.Address2 = String.Empty;
            person.Address.City = "District 11";
            person.Address.State = "New York";
            person.Address.ZipCode = "11110";
            person.PhoneNumber.AreaCode = "888";
            person.PhoneNumber.PhoneNumber = "1110000";
            person.SSN.SSNNumber = "000110000";
            person.DateOfBirth = new DateTime(1990, 1, 1);
            db.SavePersonalInfo(person);
        }

        // Save a person (Update)
        static void Test8()
        {
            PersonalInfo person = new PersonalInfo();
            person.Id = 1;
            person.Name.FirstName = "OOOOO";
            person.Name.MiddleName = "MMMMM";
            person.Name.LastName = "GGGGG";
            person.Address.Address1 = "888 Moon Planet";
            person.Address.Address2 = String.Empty;
            person.Address.City = "District 5";
            person.Address.State = "New York";
            person.Address.ZipCode = "00001";
            person.PhoneNumber.AreaCode = "010";
            person.PhoneNumber.PhoneNumber = "8889999";
            person.SSN.SSNNumber = "999114444";
            person.DateOfBirth = new DateTime(1995, 4, 6);
            db.SavePersonalInfo(person);
        }

        // Procedure test
        static void Test7()
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                string queryString = "TestProcedure";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();
                int rows = command.ExecuteNonQuery();
                Console.WriteLine(rows);
                connection.Close();
            }

            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                string queryString = "TestProcedure2";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(
                            reader.GetInt32(0) + " " +
                            reader.GetString(1)
                            );
                    }
                }
                finally
                {
                    // Always call Close when done reading.
                    reader.Close();
                }
                connection.Close();
            }
        }

        // Partially fill in people info
        static void Test6()
        {
            List<PersonalInfo> people = new List<PersonalInfo>();
            db.PartiallyReloadPersonalInfos(people);
            PrintList(people);
        }

        // Fill in one person
        static void Test5()
        {
            PersonalInfo person = new PersonalInfo();
            db.ReloadPersonalInfo(person, 0);
            Console.WriteLine(person);
        }

        // Fully fill personalInfo list
        static void Test4()
        {
            List<PersonalInfo> people = new List<PersonalInfo>();
            db.Debug_FullyReloadPersonalInfos(people);
            PrintList(people);
        }

        // Attachment table test
        static void Test3()
        {
            List<AttachmentInfo> amts = new List<AttachmentInfo>();
            db.ReloadAttachments(amts, 0);
            PrintList(amts);
        }

        // States table test
        static void Test2()
        {
            List<StateInfo> states = new List<StateInfo>();
            db.ReloadStates(states);
            PrintList(states);
        }

        // Database testing begin

        static void Test1()
        {
            NameInfo name = new NameInfo("first", "middle", "last");
            AddressInfo addr = new AddressInfo("747 Sumner Ave", "Apt 1", "Syracuse", "NY", "13210");
            PhoneNumberInfo phone = new PhoneNumberInfo("123", "7891234");
            SSNNumberInfo ssn = new SSNNumberInfo("0192938475");
            string gender = "Male";
            DateTime dob = DateTime.Now;
            DateTime dateCreated = DateTime.Now;
            DateTime dateModified = DateTime.Now;

            PersonalInfo personalInfo = new PersonalInfo(-1, name, addr, phone, ssn, gender, dob, dateCreated, dateModified);
            personalInfo.AddAttachment(new AttachmentInfo(-1, "Image", @"CurrentPath\", "Image.jpg"));

            Console.WriteLine(personalInfo);
            
            Console.WriteLine(personalInfo.Name.FirstName);
            Console.WriteLine(personalInfo.Name.MiddleName);
            Console.WriteLine(personalInfo.Name.LastName);
            Console.WriteLine(personalInfo.Address.Address1);
            Console.WriteLine(personalInfo.Address.Address2);
            Console.WriteLine(personalInfo.Address.City);
            Console.WriteLine(personalInfo.Address.State);
            Console.WriteLine(personalInfo.Address.ZipCode);
            Console.WriteLine(personalInfo.PhoneNumber.AreaCode);
            Console.WriteLine(personalInfo.PhoneNumber.PhoneNumber);
            Console.WriteLine(personalInfo.SSN);
            Console.WriteLine(personalInfo.DateOfBirth);
            Console.WriteLine(personalInfo.Attachments);
        }

        static void PrintList<T>(List<T> list)
        {
            if (list == null)
            {
                return;
            }

            int i = 0;
            foreach(object obj in list)
            {
                Console.WriteLine("[" + (i++) + "]: " + obj.ToString());
            }
        }
    }
}
