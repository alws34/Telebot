# Telebot
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


# Features
#### Temperature Monitoring
- The application can monitor and warn you whether your cpu/gpu's temperature is above the limit defined in `settings.ini` as:

```
[Temperature]
CPU_TEMPERATURE_WARNING = 65 
GPU_TEMPERATURE_WARNING = 65
```

- You can schedule to monitor for a specific duration using the command `/temptime`
#### Intranet Scanning and Monitoring 
- Telebot has the capability to monitor the local area network for new connected/disconnected devices or just send you a static scan report of all connected device to the network.
- This feature is using an external scanning utility from [nirsoft](http://www.nirsoft.net/utils/wireless_network_watcher.html) and reads the scan report in memory.
