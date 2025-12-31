namespace Wizdle.Wpf;

using System.Windows;

public partial class MainWindow : Window
{
    private readonly WizdleEngine _wizdleEngine;

    public MainWindow(WizdleEngine wizdleEngine)
    {
        InitializeComponent();
        _wizdleEngine = wizdleEngine ?? throw new ArgumentNullException(nameof(wizdleEngine));
    }
}
