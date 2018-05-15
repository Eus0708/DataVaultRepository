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
        
    }

    public class ErrorHandler
    {
        public static string ErrorMessage(StatusCode code)
        {
            switch (code)
            {
                case StatusCode.NO_ERROR: return "No error";
                case StatusCode.UNKNOWN_ERROR: return "Unknown error";


                default: return "Invalid code";
            }
        }
    }
}
