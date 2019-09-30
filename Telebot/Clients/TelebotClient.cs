﻿using System;
using System.IO;
using System.Text.RegularExpressions;
using Telebot.Models;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace Telebot.Clients
{
    public class TelebotClient : TelegramBotClient, ITelebotClient
    {
        public int AdminID { get; private set; }

        public TelebotClient(string bot_token, int admin_id) : base(bot_token)
        {
            this.AdminID = admin_id;

            OnMessage += BotMessageHandler;
        }

        public event EventHandler<RequestArrivalArgs> RequestArrival;

        private void RaiseRequestArrival(RequestArrivalArgs e)
        {
            RequestArrival?.Invoke(this, e);
        }

        private void BotMessageHandler(object sender, MessageEventArgs e)
        {
            async void executeCallback(CommandResult result)
            {
                switch (result.SendType)
                {
                    case SendType.Text:
                        await SendTextMessageAsync(e.Message.Chat.Id, result.Text.TrimEnd(),
                            parseMode: ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                        break;
                    case SendType.Photo:
                        var photo = new InputOnlineFile(result.Stream, "capture.jpg");
                        await SendDocumentAsync(e.Message.Chat.Id, photo,
                            parseMode: ParseMode.Markdown, replyToMessageId: e.Message.MessageId,
                            caption: "From *Telebot*", thumb: photo as InputMedia);
                        result.Stream.Close();
                        result.Stream.Dispose();
                        break;
                    case SendType.Document:
                        var document = new InputOnlineFile(result.Stream, (result.Stream as FileStream).Name);
                        await SendDocumentAsync(e.Message.Chat.Id, document,
                            parseMode: ParseMode.Markdown, replyToMessageId: e.Message.MessageId,
                            caption: "From *Telebot*", thumb: document as InputMedia);
                        result.Stream.Close();
                        result.Stream.Dispose();
                        break;
                }
            }

            if (e.Message.From.Id != AdminID)
            {
                var cmdResult = new CommandResult
                {
                    SendType = SendType.Text,
                    Text = "Unauthorized."
                };
                executeCallback(cmdResult);
                return;
            }

            string cmdPattern = e.Message.Text;

            if (string.IsNullOrEmpty(cmdPattern))
            {
                var cmdResult = new CommandResult
                {
                    SendType = SendType.Text,
                    Text = "Command pattern is null or empty."
                };
                executeCallback(cmdResult);
                return;
            }

            string info = $"Received {cmdPattern} from {e.Message.From.Username}.";

            var objlvitem = new ObjListViewItem
            {
                DateTime = DateTime.Now.ToString(),
                Text = info
            };

            var arrival = new RequestArrivalArgs
            {
                Item = objlvitem
            };

            RaiseRequestArrival(arrival);

            var command = Program.commandFactory.Dispatch(cmdPattern);

            if (command != null)
            {
                var groups = Regex.Match(cmdPattern, command.Pattern).Groups;

                var cmdParams = new CommandParam
                {
                    Groups = groups
                };

                command.Execute(cmdParams, executeCallback);
            }
            else
            {
                var cmdResult = new CommandResult
                {
                    SendType = SendType.Text,
                    Text = "Undefined command. For commands list, type */help*."
                };
                executeCallback(cmdResult);
            }
        }
    }
}
