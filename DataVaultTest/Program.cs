using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SystemCommon;
using DataVaultCommon;

namespace DataVaultTest
{
    class Program
    {
        static DataVaultDatabaseManager db = new DataVaultDatabaseManager();

        static void Main(string[] args)
        {
            Test7();

            db.CloseConnection();
            Console.Read();
            //Console.WriteLine("Done DataVault Test");
        }

        // Save a person
        static void Test7()
        {
            PersonalInfo person = new PersonalInfo();
            person.Id = 1;
            person.Name.MiddleName = "Omg";
            person.DateOfBirth = new DateTime(1995, 4, 6);
            db.SavePersonalInfo(person);

            Console.WriteLine("Executed");
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
            db.ReloadAttachments(amts, 1);
            PrintList(amts);
        }

        // States table test
        static void Test2()
        {
            List<StateInfo> states = new List<StateInfo>();
            db.ReloadStates(states);
            PrintList(states);
        }

        static void Test1()
        {
            NameInfo name = new NameInfo("first", "middle", "last");
            AddressInfo addr = new AddressInfo("747 Sumner Ave", "Apt 1", "Syracuse", "NY", "13210");
            PhoneNumberInfo phone = new PhoneNumberInfo("123", "7891234");
            SSNNumberInfo ssn = new SSNNumberInfo("0192938475");
            DateTime dob = DateTime.Now;
            DateTime dateCreated = DateTime.Now;
            DateTime dateModified = DateTime.Now;

            PersonalInfo personalInfo = new PersonalInfo(-1, name, addr, phone, ssn, dob, dateCreated, dateModified);
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
            int i = 0;
            foreach(object obj in list)
            {
                Console.WriteLine("[" + (i++) + "]: " + obj.ToString());
            }
        }
    }
}
