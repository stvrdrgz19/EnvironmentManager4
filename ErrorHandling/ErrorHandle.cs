using Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace ErrorHandling
{
    public class ErrorHandle
    {
        public static void LogException(Exception e, bool dbUpdate = false, string extraMessage = null)
        {
            string logFile = Utils.GetFile("Log.txt");
            DateTime logTime = DateTime.Now;
            using (FileStream stream = File.Open(logFile, FileMode.OpenOrCreate))
            {
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    sw.WriteLine(String.Format("-({0}){1}", logTime, Const.ExceptionDivider));
                    sw.WriteLine(String.Format("Environment Manager v{0}", Utils.GetAppVersion()));
                    sw.WriteLine(String.Format("Exception Message: {0}", e.Message));
                    sw.WriteLine(String.Format("Exception Type: {0}", e.GetType().ToString()));
                    sw.WriteLine(String.Format("Exception Source: {0}", e.Source));
                    sw.WriteLine(String.Format("Exception Target Site: {0}", e.TargetSite));
                    sw.WriteLine("");
                    if (!String.IsNullOrEmpty(extraMessage))
                    {
                        sw.WriteLine(extraMessage);
                        sw.WriteLine("");
                    }
                    sw.WriteLine("STACK TRACE");
                    sw.WriteLine(e.StackTrace);
                    sw.WriteLine("");
                }
            }
        }

        public static void DisplayExceptionMessage(Exception e, bool dbUpdate = false, string extraMessage = null, string action = null, string variables = null)
        {
            ExceptionForm form = new ExceptionForm();
            ExceptionForm.exception = e;
            ExceptionForm.extraMessage = extraMessage;
            ExceptionForm.action = action;
            ExceptionForm.variables = variables;
            form.ShowDialog();
        }

        public static void LogDatabaseUpdateFailure()
        {
            string logFile = Utils.GetFile(Const.EnvironmentManagerLogFile);
            DateTime logTime = DateTime.Now;

            using (FileStream stream = File.Open(logFile, FileMode.OpenOrCreate))
            {
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    sw.WriteLine(String.Format("-({0})-------------------------------------------------", logTime));
                    sw.WriteLine(String.Format("Environment Manager v{0}", Utils.GetAppVersion()));
                    sw.WriteLine(GetLogContents());
                }
            }
        }

        public static void DisplayDatabaseUpdateFailure()
        {
            ExceptionForm form = new ExceptionForm();
            ExceptionForm.extraMessage = GetLogContents();
            ExceptionForm.dbUpdateFail = true;
            form.ShowDialog();
        }

        public static void DeleteLogFiles()
        {
            foreach (string file in Directory.GetFiles(Utils.GetCurrentDirectory()))
            {
                if (file.Contains(Const.DatabaseUpdatePassLog) || file.Contains(Const.DatabaseUpdateFailLog))
                    File.Delete(file);
            }
        }

        public static bool IsThereAFailLog()
        {
            return Directory.GetFiles(Utils.GetCurrentDirectory()).Any(s => s.Contains(Const.DatabaseUpdateFailLog));
        }

        private static string GetLogContents()
        {
            string logContents = Const.CouldNotFindFailLog;
            foreach (string file in Directory.GetFiles(Utils.GetCurrentDirectory()))
                if (file.Contains(Const.DatabaseUpdateFailLog))
                    logContents = File.ReadAllText(file);
            return logContents;
        }
    }
}
