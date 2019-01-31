using SimpleInjector;
using System;
using System.Threading;
using System.Windows.Forms;
using Telebot.Commands;
using Telebot.Commands.StatusCommands;
using Telebot.Contracts;
using Telebot.BusinessLogic;
using Telebot.Loggers;
using Telebot.Managers;
using Telebot.Monitors;
using Telebot.Providers;
using Telebot.Presenters;
using System.Collections.Generic;

namespace Telebot
{
    static class Program
    {
        public static Container container;
        public static CPUIDSDK pSDK;
        public static int NbDevices;
        private static volatile bool _shouldStop = false;

        [STAThread]
        static void Main()
        {
            Thread UpdateThread;
            bool res;
            int error_code = 0, extended_error_code = 0;
            string error_message;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            UpdateThread = new Thread(ThreadLoop);

            pSDK = new CPUIDSDK();
            pSDK.InitDLL();
            res = pSDK.InitSDK(ref error_code, ref extended_error_code);

            if (error_code != CPUIDSDK.CPUIDSDK_ERROR_NO_ERROR)
            {
                //	Init failed, check errorcode
                switch ((uint)error_code)
                {
                    case CPUIDSDK.CPUIDSDK_ERROR_EVALUATION:
                        {
                            switch ((uint)extended_error_code)
                            {
                                case CPUIDSDK.CPUIDSDK_EXT_ERROR_EVAL_1:
                                    error_message = "You are running a trial version of the DLL SDK. In order to make it work, please run CPU-Z at the same time.";
                                    break;

                                case CPUIDSDK.CPUIDSDK_EXT_ERROR_EVAL_2:
                                    error_message = "Evaluation version has expired.";
                                    break;

                                default:
                                    error_message = "Eval version error " + extended_error_code;
                                    break;
                            }
                        }
                        break;

                    case CPUIDSDK.CPUIDSDK_ERROR_DRIVER:
                        error_message = "Driver error " + extended_error_code;
                        break;

                    case CPUIDSDK.CPUIDSDK_ERROR_VM_RUNNING:
                        error_message = "Virtual machine detected.";
                        break;

                    case CPUIDSDK.CPUIDSDK_ERROR_LOCKED:
                        error_message = "SDK mutex locked.";
                        break;

                    default:
                        error_message = "Error code 0x%X" + error_code;
                        break;
                }

                MessageBox.Show(error_message, "CPUID SDK Error");
            }

            if (res)
            {
                NbDevices = pSDK.GetNumberOfDevices();

                UpdateThread.Start();

                buildContainer();

                var mainForm = container.GetInstance<MainForm>();

                var presenter = new MainFormPresenter(mainForm);

                Application.Run(mainForm);

                _shouldStop = true;
                UpdateThread.Join();
            }

            pSDK.UninitSDK();
        }

        private static void ThreadLoop()
        {
            while (!_shouldStop)
            {
                pSDK.RefreshInformation();
                Thread.Sleep(1000);
            }
        }

        private static void buildContainer()
        {
            container = new Container();

            container.Collection.Register<IStatusCommand>
            (
                typeof(SystemCmd),
                typeof(IPCmd),
                typeof(UptimeCmd),
                typeof(TempMonitorCmd)
            );

            container.Collection.Register<ICommand>
            (
                typeof(StatusCmd),
                typeof(AppsCmd),
                typeof(CaptureCmd),
                typeof(ScreenOnCmd),
                typeof(ScreenOffCmd),
                typeof(MonitorOnCmd),
                typeof(MonitorOffCmd),
                typeof(RebootCmd),
                typeof(ShutdownCmd),
                typeof(HelpCmd)
            );

            container.Collection.Register<ITemperatureProvider>
            (
                typeof(CPUProvider),
                typeof(GPUProvider)
            );

            container.Register<ITemperatureMonitor, SystemTempMonitor>(Lifestyle.Singleton);
            container.Register<ISettings, SettingsManager>(Lifestyle.Singleton);
            container.Register<ILogger, FileLogger>(Lifestyle.Singleton);
            container.Register<MainForm>(Lifestyle.Singleton);

            container.Register<CaptureLogic>();
            container.Register<NetworkLogic>();
            container.Register<PowerLogic>();
            container.Register<ScreenLogic>();
            container.Register<SystemLogic>();
            container.Register<WindowsLogic>();

            container.Verify();
        }
    }
}
