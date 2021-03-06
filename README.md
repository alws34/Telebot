# Telebot

An open source modular smart monitoring system. With a modular design, Telebot allows you to extend it and turn it into your personal assistant. Communication with Telebot is via Telegram (Or any other service that implements `IBotClient`). You must create a bot by following the steps [here](https://core.telegram.org/bots) and use the token to setup the bot.

# Setup

Create a settings.ini file and place it within the same folder as binaries. Make sure you have the bot token and the user id (who will control the bot) in the following format:
 
 ```
[Telegram]
Token = <Bot_Token>
AdminId = <User_ID>
```

Necessary files will be copied to bin directory upon project build.

# Architecture

#### Telebot module (Non-GUI)
The application is a [Tray-based](https://docs.microsoft.com/en-us/windows/win32/shell/notification-area) modular application based on [Model–view–presenter (MVP) architectural pattern](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93presenter) (**Notification-view**). Each module is encapsulated by the [Command design pattern](https://en.wikipedia.org/wiki/Command_pattern).

#### CPUID module
This module is an abstraction to the cpuid sdk library.

#### Updater module
Encapsulates the software update logic.

#### BotSdk module
Definitions shared between plugins and main module in order to allow communication.

#### .\Plugins
Includes all preprogrammed plugins that extends the application behaviour. 
Extending functionality is possible by [creating a plugin](https://github.com/jdahan91/Telebot/wiki/Creating-a-Plugin).

# Features
#### Temperature Monitoring
The application can monitor and warn you whether your cpu/gpu's temperature is above the limit defined in `settings.ini` as:

```
[Temperature]
CPU_TEMPERATURE_WARNING = 65 
GPU_TEMPERATURE_WARNING = 65
```

You can schedule to monitor for a specific duration using the command `/temptime`

#### Intranet Scanning and Monitoring 

Telebot has the capability to listen for devices on the network or just send you a static scan report of all connected devices.

This feature is using an external scanning utility from [nirsoft](http://www.nirsoft.net/utils/wireless_network_watcher.html) and reads the scan report in memory.

The utility by default will scan the entire network range (1..255) which will take time. In order to customize this you can configure this in the settings (wnet.cfg) by modifiying the entry:

```
UseIPAddressesRange=1 // 1 = enabled (true)
IPAddressFrom=192.168.1.1
IPAddressTo=192.168.1.55 // change this
```

# Binaries

The latest build of Telebot can be downloaded from [this](https://raw.githubusercontent.com/jdahan91/Telebot/master/Telebot/Update/Release.zip) direct link (which is always updated to latest build) or from the [Release](https://github.com/jdahan91/Telebot/releases) section. Of course that Telebot supports OTA updates by utilizing the `/update` command.

Note that Telebot checks for an update every hour and notifies **only** if there's an update available.

# Donations

Do You Like Telebot?
Then please consider a donation to support the development of Telebot.

The Telebot project started at late 2018 as a mimic of [MagicMirror](https://magicmirror.builders/) project for Windows, which then evolved professionally on my free time to keep this amazing project running. Donations are not required, but are appreciated. (Donors are welcome to request features developed into Telebot)

However there are more ways to help. If you are familiar with .NET, C# or Refactoring, feel free to fork and make useful modifications to pull into master branch.

[![paypal](https://www.paypalobjects.com/en_US/IL/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=LKNNJLAD48V7G&item_name=Telebot+development+support&currency_code=ILS&source=url)
