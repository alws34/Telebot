using CPUID.Builder;
using FluentScheduler;
using System;
using System.Threading;
using System.Windows.Forms;
using Telebot.Clients;
using Telebot.Commands;
using Telebot.Commands.Factories;
using Telebot.Commands.Status;
using Telebot.Contracts;
using Telebot.Presenters;
using Telebot.ScreenCapture;
using Telebot.Temperature;
using static CPUID.CPUIDCore;
using static Telebot.Settings.SettingsFactory;

namespace Telebot
{
    static class Program
    {
        public static IFactory<ICommand> commandFactory;
        public static IFactory<ITempMon> tempMonFactory;
        public static IFactory<IScreenCapture> screenCapFactory;

        private static volatile bool _shouldStop = false;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Thread RefreshThread = new Thread(() =>
            {
                while (!_shouldStop)
                {
                    pSDK.RefreshInformation();
                    Thread.Sleep(1000);
                }
            });

            tempMonFactory = new TempMonFactory();
            screenCapFactory = new ScreenCapFactory();

            buildCommandFactory();

            var tempMons = tempMonFactory.GetAllEntities();
            var screenCaps = screenCapFactory.GetAllEntities();

            MainForm mainForm = new MainForm();

            string token = TelegramSettings.GetBotToken();
            int id = TelegramSettings.GetAdminId();

            TelebotClient telebotClient = new TelebotClient(token, id);

            var presenter = new MainFormPresenter(
                mainForm,
                telebotClient,
                screenCaps,
                tempMons
            );

            RefreshThread.Start();

            Application.Run(mainForm);

            // commit profiles changes
            SettingsBase.CommitChanges();

            // write changes to disk
            SettingsBase.WriteChanges();

            // stop job manager and remove all jobs
            JobManager.Stop();
            JobManager.RemoveAllJobs();

            // wait for thread to complete
            _shouldStop = true;
            RefreshThread.Join();
        }

        private static void buildCommandFactory()
        {
            var builder = new DeviceBuilder()
                .AddItems(DeviceFactory.RAMDevices)
                .AddItems(DeviceFactory.CPUDevices)
                .AddItems(DeviceFactory.HDDDevices)
                .AddItems(DeviceFactory.GPUDevices)
                .Build();

            commandFactory = new CommandFactory
            (
                new ICommand[]
                {
                    new StatusCmd
                    (
                        new IStatus[]
                        {
                            new SystemStatus(builder),
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
