namespace Wizdle.Windows
{
    internal static class Program
    {
        [STAThread]
        internal static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new WizdleForm());
        }
    }
}