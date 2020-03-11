# Telebot <img src="https://github.com/jdahan91/Telebot/blob/master/Telebot/icon.ico" width="24" height="24" />
A 64-Bit Windows Telegram bot that monitors system activity such as utilization and temperature and reports them to the client upon request (from commander).

# Setup instructions
 - Create a settings.ini file and place it within the same folder as binaries. Make sure you have the bot token and the user id (who will control the bot) in the following format:
 
 ```
[Telegram]
Token = <Bot_Token>
AdminId = <User_ID> // The telegram user who will command the bot.
```

- Copy cpuidsdk64.dll and SetVol.exe to binaries folder.

# Application architecture

#### Telebot module (Non-GUI):
The application is a [Tray-based](https://docs.microsoft.com/en-us/windows/win32/shell/notification-area) application based on [Model–view–presenter (MVP) architectural pattern](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93presenter) (**Notification-view**). Each command is encapsulated by the [Command design pattern](https://en.wikipedia.org/wiki/Command_pattern).

#### SpecInfo module:
The application that fetches full computer hardware information and saves it within same directory to file *spec.txt*. This program is used with the **/spec** command.

#### CPUID module (Core functionality):
This module is an abstraction to the cpuid library which is the core of the entire application to perform hardware analyzations tasks.
