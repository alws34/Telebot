using System;
using System.Threading;
using System.Windows.Forms;
using Telebot.Loggers;
using Telebot.Presenters;
using Telebot.Settings;

namespace Telebot
{
    static class Program
    {
        public static ILogger logger;
        public static ISettings appSettings;
        public static CPUIDSDK pSDK;

        private static volatile bool _shouldStop = false;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var RefreshThread = new Thread(RefreshThreadProc);

            pSDK = new CPUIDSDK();
            pSDK.InitDLL();
            bool sdkLoaded = pSDK.InitSDK_Quick();

            if (!sdkLoaded)
            {
                throw new Exception("Couldn't load cpuidsdk module.");
            }

            RefreshThread.Start();

            logger = new FileLogger();
            appSettings = new SettingsManager();
            MainForm mainForm = new MainForm();

            var presenter = new MainFormPresenter(mainForm);

            Application.Run(mainForm);

            _shouldStop = true;
            RefreshThread.Join();

            pSDK.UninitSDK();

            appSettings.CommitChanges();
        }

        private static void RefreshThreadProc()
        {
            while (!_shouldStop)
            {
                pSDK.RefreshInformation();
                Thread.Sleep(1000);
            }
        }
    }
}
