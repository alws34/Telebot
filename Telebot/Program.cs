using FluentScheduler;
using System;
using System.Threading;
using System.Windows.Forms;
using Telebot.Clients;
using Telebot.Commands;
using Telebot.Commands.Factories;
using Telebot.Commands.Status;
using Telebot.Presenters;
using Telebot.ScreenCapture;
using Telebot.Temperature;
using static CPUID.CPUIDCore;
using static Telebot.Settings.SettingsFactory;

namespace Telebot
{
    static class Program
    {
        public static CommandFactory commandFactory;

        public static IScreenCapture screenCapture;
        public static ITempMon tempMonWarning;
        public static ITempMon tempMonDurated;
        public static ITempMon[] tempMons;

        private static volatile bool _shouldStop = false;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Thread RefreshThread = new Thread(RefreshThreadProc);

            MainForm mainForm = new MainForm();

            string token = TelegramSettings.GetBotToken();
            int id = TelegramSettings.GetAdminId();

            TelebotClient telebotClient = new TelebotClient(token, id);

            screenCapture = new ScreenCaptureDurated();

            tempMonWarning = new TempMonWarning
            (
                DeviceFactory.CPUDevices,
                DeviceFactory.GPUDevices
            );

            tempMonDurated = new TempMonDurated
            (
                DeviceFactory.CPUDevices,
                DeviceFactory.GPUDevices
            );

            tempMons = new ITempMon[]
            {
                tempMonWarning,
                tempMonDurated
            };

            var presenter = new MainFormPresenter(
                mainForm,
                telebotClient,
                screenCapture,
                tempMonWarning,
                tempMonDurated
            );

            SettingsBase.AddProfiles(
                presenter,
                (Settings.IProfile)tempMonWarning
            );

            buildCommandFactory();

            RefreshThread.Start();

            Application.Run(mainForm);

            // commit profiles changes
            SettingsBase.CommitChanges();

            // write changes to disk
            SettingsBase.WriteChanges();

            // stop job manager
            JobManager.Stop();

            // wait for thread to complete
            _shouldStop = true;
            RefreshThread.Join();
        }

        private static void RefreshThreadProc()
        {
            while (!_shouldStop)
            {
                pSDK.RefreshInformation();
                Thread.Sleep(1000);
            }
        }

        private static void buildCommandFactory()
        {
            commandFactory = new CommandFactory
            (
                new ICommand[]
                {
                    new StatusCmd
                    (
                        new IStatus[]
                        {
                            new SystemStatus
                            (
                                DeviceFactory.RAMDevices,
                                DeviceFactory.CPUDevices,
                                DeviceFactory.HDDDevices,
                                DeviceFactory.GPUDevices
                            ),
                            new IPAddrStatus(),
                            new UptimeStatus(),
                            new MonitorsStatus(),
                            new CaptureStatus()
                        }
                    ),
                    new AppsCmd(),
                    new BrightCmd(),
                    new CaptureCmd(),
                    new CapAppCmd(),
                    new CapTimeCmd(),
                    new ScreenCmd(),
                    new TempMonCmd(),
                    new TempTimeCmd(),
                    new PowerCmd(),
                    new ShutdownCmd(),
                    new MessageBoxCmd(),
                    new KillTaskCmd(),
                    new VolCmd(),
                    new SpecCmd(),
                    new HelpCmd()
                }
            );
        }
    }
}
