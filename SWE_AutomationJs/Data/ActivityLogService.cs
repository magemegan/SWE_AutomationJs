using System;
using System.IO;

namespace SWE_AutomationJs_UI_Design.Data
{
    internal static class ActivityLogService
    {
        private static readonly string LogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "activity-log.txt");

        public static void Log(string employeeId, string action, string details)
        {
            string line = string.Format(
                "{0:u} | {1} | {2} | {3}",
                DateTime.UtcNow,
                string.IsNullOrWhiteSpace(employeeId) ? "SYSTEM" : employeeId,
                action,
                details ?? string.Empty);

            File.AppendAllText(LogPath, line + Environment.NewLine);
        }
    }
}
