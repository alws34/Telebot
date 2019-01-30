using IniParser;
using IniParser.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Telebot.Contracts;

namespace Telebot.Managers
{
    public class SettingsManager : FileIniDataParser, ISettings
    {
        private const string FILE_NAME = "settings.ini";
        private readonly string filePath;

        private readonly IniData data;

        public SettingsManager()
        {
            filePath = $"{Application.StartupPath}\\{FILE_NAME}";

            if (!File.Exists(filePath)) {
                File.Create(filePath);
            }

            try {
                data = ReadFile(filePath);
            }
            catch {

            }
        }

        ~SettingsManager()
        {
            WriteFile(filePath, data);
        }

        public bool MonitorEnabled
        {
            get
            {
                return Convert.ToBoolean(data["Temperature.Monitor"]["Enabled"]);
            }
            set
            {
                data["Temperature.Monitor"]["Enabled"] = value.ToString();
            }
        }

        public long ChatId
        {
            get
            {
                return Convert.ToInt64(data["Telegram"]["Chat.Id"]);
            }
            set
            {
                data["Telegram"]["Chat.Id"] = value.ToString();
            }
        }

        public float CPUTemperature
        {
            get
            {
                string s = data["Temperature.Monitor"]["CPU_TEMPERATURE_WARNING"];
                if (string.IsNullOrEmpty(s)) {
                    return 65.0f;
                }
                return (float)Convert.ToDouble(s);
            }
            set
            {
                data["Temperature.Monitor"]["CPU_TEMPERATURE_WARNING"] = value.ToString();
            }
        }

        public Rectangle Form1Bounds
        {
            get
            {
                string s = data["GUI"]["Form1.Bounds"];
                if (string.IsNullOrEmpty(s)) {
                    return new Rectangle(50, 50, 150, 150);
                }
                return JsonConvert.DeserializeObject<Rectangle>(s);
            }
            set
            {
                string s = JsonConvert.SerializeObject(value);
                data["GUI"]["Form1.Bounds"] = s;
            }
        }

        public float GPUTemperature
        {
            get
            {
                string s = data["Temperature.Monitor"]["GPU_TEMPERATURE_WARNING"];
                if (string.IsNullOrEmpty(s)) {
                    return 65.0f;
                }
                return (float)Convert.ToDouble(s);
            }
            set
            {
                data["Temperature.Monitor"]["GPU_TEMPERATURE_WARNING"] = value.ToString();
            }
        }

        public List<int> ListView1ColumnsWidth
        {
            get
            {
                string s = data["GUI"]["listview1.Columns.Width"];
                if (string.IsNullOrEmpty(s)) {
                    return new List<int> { 50, 150 };
                }
                return JsonConvert.DeserializeObject<List<int>>(s);
            }
            set
            {
                string s = JsonConvert.SerializeObject(value);
                data["GUI"]["listview1.Columns.Width"] = s;
            }
        }

        public string TelegramToken
        {
            get
            {
                string s = data["Telegram"]["Token"];
                if (string.IsNullOrEmpty(s)) {
                    return string.Empty;
                }
                return s;
            }
        }

        public List<int> TelegramWhiteList
        {
            get
            {
                string s = data["Telegram"]["WhiteList"];
                if (string.IsNullOrEmpty(s)) {
                    return new List<int>();
                }
                return JsonConvert.DeserializeObject<List<int>>(s);
            }
        }
    }
}
