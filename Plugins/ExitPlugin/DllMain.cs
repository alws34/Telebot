using BotSdk.Contracts;
using BotSdk.Models;
using SimpleInjector;
using System.Threading.Tasks;

namespace Plugins.Exit
{
    public class DllMain : IModule
    {
        private IAppExit appExit;

        public DllMain()
        {
            Pattern = "/exit";
            Description = "Shutdown Telebot.";
        }

        public override async void Execute(Request req)
        {
            var result = new Response("Telebot is closing...", req.MessageId);

            await ResultHandler(result);

            await Task.Delay(2000).ContinueWith((t) =>
            {
                appExit.Exit();
            });
        }

        public override void Initialize(ModuleData data)
        {
            base.Initialize(data);
            var container = (Container)data.IoCProvider.GetService(typeof(Container));
            appExit = container.GetInstance<IAppExit>();
        }
    }
}
