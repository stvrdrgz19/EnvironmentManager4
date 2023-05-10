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
            ToastContentBuilder tst = new ToastContentBuilder();
            tst.AddText(title);
            tst.AddText(message);
            tst.Show(toast =>
            {
                toast.ExpirationTime = DateTime.Now.AddMinutes(expireMinutes);
            });
        }
    }
}
