namespace Wizdle.Windows
{
    using System;
    using System.Windows.Forms;

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