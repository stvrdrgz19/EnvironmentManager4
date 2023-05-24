using Microsoft.Toolkit.Uwp.Notifications;
using System;

namespace Utilities
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
