using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace EnvironmentManager4
{
    public class WaterBot
    {
        public static WaterBotForm s_waterBot;
        public static int lastHour = DateTime.Now.Hour;

        public static void StartWaterBot()
        {
            //System.Timers.Timer aTimer = new System.Timers.Timer(60000);
            ////aTimer.AutoReset = false;
            //aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            //aTimer.Start();
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            SettingsModel settings = SettingsUtilities.GetSettings();
            if (lastHour < DateTime.Now.Hour || (lastHour == 23 && DateTime.Now.Hour == 0))
                if (DateTime.Now.Minute == 0 || DateTime.Now.Minute == 30)
                    if (settings.Other.EnableWaterBot)
                    {
                        //ToastNotificationManagerCompat.History.Clear();
                        ToastContentBuilder wb = new ToastContentBuilder();
                        wb.AddText("WaterBot");
                        wb.AddText("Time to drink some water.");
                        wb.Show();
                        //s_waterBot = new WaterBotForm();
                        //s_waterBot.ShowDialog();
                    }
        }
    }
}
