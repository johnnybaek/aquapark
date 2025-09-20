using AquaparkApp.Forms;

namespace AquaparkApp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            
            // Запуск главной формы
            Application.Run(new MainForm());
        }
    }
}
