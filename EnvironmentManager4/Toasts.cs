using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentManager4
{
    public class Toasts
    {
        public static void Toast(string title, string message, int expireMinutes = 5)
        {
            // Toasts are informational only; some machines reject them (PlatformNotSupportedException), so never let them crash the app
            try
            {
                ToastContentBuilder tst = new ToastContentBuilder();
                tst.AddText(title);
                tst.AddText(message);
                tst.Show(toast =>
                {
                    toast.ExpirationTime = DateTime.Now.AddMinutes(expireMinutes);
                });
            }
            catch (Exception e)
            {
                ErrorHandling.LogException(e, false, String.Format("Unable to show toast: {0} - {1}", title, message));
            }
        }
    }
}
