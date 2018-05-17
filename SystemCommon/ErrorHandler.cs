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
        INVALID_PASSWORD,
        NOT_ALLOW_TO_ACCESS,
        INVALID_SEARCH_OPT,
    }

    public class ErrorHandler
    {
        public static string ErrorMessage(StatusCode code)
        {
            switch (code)
            {
                case StatusCode.NO_ERROR: return "No error";
                case StatusCode.UNKNOWN_ERROR: return "Unknown error";
                case StatusCode.INVALID_PASSWORD: return "Invalid password";
                case StatusCode.NOT_ALLOW_TO_ACCESS: return "Not allow to access";
                case StatusCode.INVALID_SEARCH_OPT: return "Invalid search option";

                default: return "Invalid status code";
            }
        }
    }
}
