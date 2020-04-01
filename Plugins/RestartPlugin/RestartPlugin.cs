﻿using Common.Models;
using System.Threading.Tasks;
using Common.Contracts;

namespace Plugins.Restart
{

    public class RestartPlugin : IPlugin
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

        public override void Initialize(PluginData data)
        {
            base.Initialize(data);
            appRestart = data.IocContainer.GetInstance<IAppRestart>();
        }
    }
}
