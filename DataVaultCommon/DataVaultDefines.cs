using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVaultCommon
{
    internal class DataVaultDefines
    {
        // Application
#if DEBUG
        public static string AppPassword = "123";
#else
        public static string AppPassword = "ddtaVll000";
#endif

        public static int MaxStringLength = 100;

        // Database
        public static string DBUsername = "None";
        public static string DBPassword = "None";
    }
}
