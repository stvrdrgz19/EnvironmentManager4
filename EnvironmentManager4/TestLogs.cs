using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentManager4
{
    public class TestLogs
    {
        public static void LogMessage(string message, string logFile = null)
        {
            if (logFile == null)
                logFile = Utilities.GetFile("TestLog.txt");

            DateTime logTime = DateTime.Now;

            using (StreamWriter sw = File.AppendText(logFile))
            {
                sw.WriteLine(String.Format("({0}) - {1}", logTime, message));
            }
        }
    }
}
