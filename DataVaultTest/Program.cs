using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SystemCommon;

namespace DataVaultTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Test1();

            Console.WriteLine("Done DataVault Test");
        }

        static void Test1()
        {
            NameInfo name = new NameInfo("first", "middle", "last");
            AddressInfo addr = new AddressInfo("747 Sumner Ave", "Apt 1", "Syracuse", "NY", "13210");
            PhoneNumberInfo phone = new PhoneNumberInfo("123", "7891234");
            SSNNumberInfo ssn = new SSNNumberInfo("0192938475");
            DateTime dob = DateTime.Now;

            PersonalInfo personalInfo = new PersonalInfo(name, addr, phone, ssn, dob);
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
        }
    }
}
