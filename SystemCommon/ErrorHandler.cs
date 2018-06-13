using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemCommon
{
    public enum StatusCode
    {
        NO_ERROR,
        UNKNOWN_ERROR,
        APPLICATION_ERROR,
        INVALID_PASSWORD,
        NOT_ALLOW_TO_ACCESS,
        INVALID_SEARCH_OPT,
        INVALID_DATEOFBIRTH,
        INVALID_INPUTS,
    }

    public class ErrorHandler
    {
        public static string ErrorMessage(StatusCode code)
        {
            switch (code)
            {
                case StatusCode.NO_ERROR: return "No error";
                case StatusCode.UNKNOWN_ERROR: return "Unknown error";
                case StatusCode.APPLICATION_ERROR: return "Application error\nPlease contact developers";
                case StatusCode.INVALID_PASSWORD: return "Invalid password";
                case StatusCode.NOT_ALLOW_TO_ACCESS: return "Not allow to access";
                case StatusCode.INVALID_SEARCH_OPT: return "Invalid search option";
                case StatusCode.INVALID_DATEOFBIRTH: return "Invalid date of birth";
                case StatusCode.INVALID_INPUTS: return "Invalid inputs";

                default: return "Invalid status code";
            }
        }
    }
}
