using System;
using System.Windows.Forms;
using SWE_AutomationJs_UI_Design.Data;

namespace SWE_AutomationJs_UI_Design
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            DatabaseInitializer.Initialize();
            AppBootstrap.Initialize();

            Application.Run(new MainMenu());
        }
    }
}