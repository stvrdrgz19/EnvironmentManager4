using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentManager4
{
    public class ErrorHandling
    {
        public static void LogException(Exception e, bool dbUpdate = false, string extraMessage = null)
        {
            string logFile = Utilities.GetFile("Log.txt");
            DateTime logTime = DateTime.Now;
            if (!File.Exists(logFile))
            {
                using (StreamWriter sw = File.CreateText(logFile))
                {
                    sw.WriteLine(String.Format("-({0})-------------------------------------------------", logTime));
                    sw.WriteLine(String.Format("Environment Manager v{0}", Utilities.GetAppVersion()));
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
            else
            {
                using (StreamWriter sw = File.AppendText(logFile))
                {
                    sw.WriteLine(String.Format("-({0})-------------------------------------------------", logTime));
                    sw.WriteLine(String.Format("Environment Manager v{0}", Utilities.GetAppVersion()));
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

        public static void DisplayExceptionMessage(Exception e, bool dbUpdate = false, string extraMessage = null)
        {
            ExceptionForm form = new ExceptionForm();
            ExceptionForm.exception = e;
            ExceptionForm.extraMessage = extraMessage;
            form.ShowDialog();
        }

        public static void LogDatabaseUpdateFailure()
        {
            string logFile = Utilities.GetFile("Log.txt");
            DateTime logTime = DateTime.Now;
            if (!File.Exists(logFile))
            {
                using (StreamWriter sw = File.CreateText(logFile))
                {
                    sw.WriteLine(String.Format("-({0})-------------------------------------------------", logTime));
                    sw.WriteLine(String.Format("Environment Manager v{0}", Utilities.GetAppVersion()));
                    sw.WriteLine(GetLogContents());
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(logFile))
                {
                    sw.WriteLine(String.Format("-({0})-------------------------------------------------", logTime));
                    sw.WriteLine(String.Format("Environment Manager v{0}", Utilities.GetAppVersion()));
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
            foreach (string file in Directory.GetFiles(Utilities.GetCurrentDirectory()))
            {
                if (file.Contains("pass_log_"))
                    File.Delete(file);
                if (file.Contains("fail_log_"))
                    File.Delete(file);
            }
        }

        public static bool IsThereAFailLog()
        {
            bool tf = false;
            foreach (string file in Directory.GetFiles(Utilities.GetCurrentDirectory()))
                if (file.Contains("fail_log_"))
                    tf = true;
            return tf;
        }

        private static string GetLogContents()
        {
            string logContents = "Could not find fail log.";
            foreach (string file in Directory.GetFiles(Utilities.GetCurrentDirectory()))
                if (file.Contains("fail_log_"))
                    logContents = File.ReadAllText(file);
            return logContents;
        }
    }
}
