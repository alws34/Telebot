using System.Text;
using Telebot.Temperature;

namespace Telebot.Commands.Status
{
    public class MonitorsStatus : IStatus
    {
        private readonly ITempMon[] tempMons;

        public MonitorsStatus()
        {
            tempMons = Program.tempMonFactory.GetAllEntities();
        }

        public string Execute()
        {
            string BoolToStr(bool condition)
            {
                return condition ? "Active" : "Inactive";
            }

            var result = new StringBuilder();

            foreach (ITempMon tempMon in tempMons)
            {
                string name = tempMon.GetType().Name.Replace("TempMon", "");
                string active = BoolToStr(tempMon.IsActive);
                result.AppendLine($"*{name}* 🌡️: {active}");
            }

            return result.ToString().TrimEnd();
        }
    }
}
