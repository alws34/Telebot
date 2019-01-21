using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telebot.Contracts
{
    public interface ISettings
    {
        Rectangle GetForm1Bounds();
        void SetForm1Bounds(Rectangle bounds);
        List<int> GetListView1ColumnsWidth();
        void SetListView1ColumnsWidth(List<int> widths);
        List<int> GetTelegramWhiteList();
        void SetTelegramWhiteList(List<int> list);
        long GetChatId();
        void SetChatId(long id);
        string GetTelegramToken();
        float GetCPUTemperature();
        void SetCPUTemperature(float value);
        float GetGPUTemperature();
        void SetGPUTemperature(float value);
        bool GetMonitorEnabled();
        void SetMonitorEnabled(bool enabled);
    }
}
