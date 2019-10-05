using CPUID.Builder;
using FluentScheduler;
using System;
using System.Threading;
using System.Windows.Forms;
using Telebot.Clients;
using Telebot.Commands;
using Telebot.Commands.Factories;
using Telebot.Commands.Status;
using Telebot.Commands.Status.Builder;
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

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            JobManager.AddJob(() => {
                pSDK.RefreshInformation();
            }, (s) => s.ToRunNow().AndEvery(1).Seconds());

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

            Application.Run(mainForm);

            SettingsBase.CommitChanges();

            SettingsBase.WriteChanges();

            JobManager.StopAndBlock();
        }

        private static void buildCommandFactory()
        {
            var devices = new DeviceBuilder()
                .AddItems(DeviceFactory.RAMDevices)
                .AddItems(DeviceFactory.CPUDevices)
                .AddItems(DeviceFactory.HDDDevices)
                .AddItems(DeviceFactory.GPUDevices)
                .Build();

            var statuses = new StatusBuilder()
                .AddItem(new SystemStatus(devices))
                .AddItem(new IPAddrStatus())
                .AddItem(new UptimeStatus())
                .AddItem(new MonitorsStatus())
                .AddItem(new CaptureStatus())
                .Build();

            commandFactory = new CommandFactory
            (
                new ICommand[]
                {
                    new StatusCmd(statuses),
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
