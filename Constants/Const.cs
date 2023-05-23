using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constants
{
    public class Const
    {
        //Misc
        public const string EnvironmentManagerLogFile = "Log.txt";

        //GP Management
        public const string CouldNotConnect = "Could not connect to network.";

        //Database Management
        public const string DatabaseUpdatePassLog = "pass_log_";
        public const string DatabaseUpdateFailLog = "fail_log_";
        public const string CouldNotFindFailLog = "Could not find fail log.";
        public const string DescriptionFullLine = "===============================================================================";
        public const string DescriptionFileNotPresent = "=================== SELECTED DATABASE HAS NO DESCRIPTION ==================";

        //Exception Handling
        public const string ExceptionDivider = "-------------------------------------------------";
    }
}
