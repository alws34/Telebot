using Common.Contracts;
using Common.Models;
using System.Threading.Tasks;

namespace Plugins.Exit
{
    public class ExitPlugin : IModule
    {
        private IAppExit appExit;

        public ExitPlugin()
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
            appExit = data.IocContainer.GetInstance<IAppExit>();
        }
    }
}
