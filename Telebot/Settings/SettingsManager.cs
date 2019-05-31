using IniParser;
using IniParser.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Telebot.Settings
{
    public class SettingsManager : FileIniDataParser, ISettings
    {
        private const string FILE_NAME = "settings.ini";
        private readonly string filePath;

        private readonly IniData data;

        public SettingsManager()
        {
            filePath = $"{Application.StartupPath}\\{FILE_NAME}";

            if (!File.Exists(filePath))
            {
                File.Create(filePath);
            }

            try
            {
                data = ReadFile(filePath);
            }
            catch
            {

            }
        }

        public void CommitChanges()
        {
            WriteFile(filePath, data);
        }

        public string TelegramToken
        {
            get
            {
                string s = data["Telegram"]["Token"];
                if (string.IsNullOrEmpty(s))
                {
                    return string.Empty;
                }
                return s;
            }
        }

        public int TelegramAdminId
        {
            get
            {
                string s = data["Telegram"]["AdminId"];
                if (string.IsNullOrEmpty(s))
                {
                    return 0;
                }
                return Convert.ToInt32(s);
            }
        }

        public Rectangle Form1Bounds
        {
            get
            {
                string s = data["GUI"]["Form1.Bounds"];
                if (string.IsNullOrEmpty(s))
                {
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

        public List<int> ListView1ColumnsWidth
        {
            get
            {
                string s = data["GUI"]["listview1.Columns.Width"];
                if (string.IsNullOrEmpty(s))
                {
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

        public float CPUTemperature
        {
            get
            {
                string s = data["Temperature.Monitor"]["CPU_TEMPERATURE_WARNING"];
                if (string.IsNullOrEmpty(s))
                {
                    return 65.0f;
                }
                return (float)Convert.ToDouble(s);
            }
            set
            {
                data["Temperature.Monitor"]["CPU_TEMPERATURE_WARNING"] = value.ToString();
            }
        }

        public float GPUTemperature
        {
            get
            {
                string s = data["Temperature.Monitor"]["GPU_TEMPERATURE_WARNING"];
                if (string.IsNullOrEmpty(s))
                {
                    return 65.0f;
                }
                return (float)Convert.ToDouble(s);
            }
            set
            {
                data["Temperature.Monitor"]["GPU_TEMPERATURE_WARNING"] = value.ToString();
            }
        }

        public bool TempMonEnabled
        {
            get
            {
                string s = data["Temperature.Monitor"]["Enabled"];
                if (string.IsNullOrEmpty(s))
                    return false;
                return Convert.ToBoolean(s);
            }
            set
            {
                data["Temperature.Monitor"]["Enabled"] = value.ToString();
            }
        }
    }
}
