using BotSdk.Contracts;
using BotSdk.Models;
using SimpleInjector;
using System.Threading.Tasks;

namespace Plugins.Restart
{
    public class RestartPlugin : IModule
    {
        private IAppRestart appRestart;

        public RestartPlugin()
        {
            Pattern = "/restart";
            Description = "Restart Telebot.";
        }

        public override async void Execute(Request req)
        {
            var result = new Response(
                "Telebot is restarting...",
                req.MessageId
            );

            await ResultHandler(result);

            await Task.Delay(2000).ContinueWith((t) =>
            {
                appRestart.Restart();
            });
        }

        public override void Initialize(ModuleData data)
        {
            base.Initialize(data);
            var container = (Container)data.IoCProvider.GetService(typeof(Container));
            appRestart = container.GetInstance<IAppRestart>();
        }
    }
}
