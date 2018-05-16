using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemCommon
{
    public enum StatusCode
    {
        NO_ERROR = -1,
        UNKNOWN_ERROR = 0,
        INCORRECT_PASSWORD = 1,
        NOT_ALLOW_TO_ACCESS = 2,
    }

    public class ErrorHandler
    {
        public static string ErrorMessage(StatusCode code)
        {
            switch (code)
            {
                case StatusCode.NO_ERROR: return "No error";
                case StatusCode.UNKNOWN_ERROR: return "Unknown error";
                case StatusCode.INCORRECT_PASSWORD: return "Incorred password";
                case StatusCode.NOT_ALLOW_TO_ACCESS: return "Not allow to access";


                default: return "Invalid code";
            }
        }
    }
}
