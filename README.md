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
- The utility by default will scan the entire network range (1..255) which will take time. In order to customize this you can configure this in the settings (wnet.cfg) by modifiying the entry:
```
UseIPAddressesRange=1 // 1 = enabled (true)
IPAddressFrom=192.168.1.1
IPAddressTo=192.168.1.55 // change this
```

# Donations
Do You Like Telebot?
Then please consider a donation to support the development of Telebot.

The Telebot project started at late 2018 as a mimic of [MagicMirror](https://magicmirror.builders/) project for Windows, which then evovled professionaly on my free time to keep this amazing project running. Donations are not required, but are appreciated. (Donators are welcome to request features developed into Telebot)

However there are more ways to help. If you are familiar with .NET, C# or Refactoring, feel free to fork and make useful modifications to pull into master branch.

[![paypal](https://www.paypalobjects.com/en_US/IL/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=LKNNJLAD48V7G&item_name=Telebot+development+support&currency_code=ILS&source=url)
