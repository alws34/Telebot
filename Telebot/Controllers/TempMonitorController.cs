namespace Telebot.Controllers
{
    public class TempMonitorController
    {
        private readonly Form1 form1;

        public TempMonitorController()
        {
            form1 = Program.container.GetInstance<Form1>();
        }

        public string GetMonitorStatus()
        {
            return form1.tempMonitor.IsActive.ToString();
        }
    }
}
