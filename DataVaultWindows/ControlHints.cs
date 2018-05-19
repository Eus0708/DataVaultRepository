using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVaultWindows
{
    internal class ControlHints
    {
        // Hints
        public static string GetHints(string controlName)
        {
            switch (controlName)
            {
                case "Password_TextBox":
                    return "Please Enter Your Password...";
                case "FirstName_TextBox":
                    return "First Name";
                case "MiddleName_TextBox":
                    return "Middle Name";
                default:
                    return string.Empty;
            }
        }
    }
}
