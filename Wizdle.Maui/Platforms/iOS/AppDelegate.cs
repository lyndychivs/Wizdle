namespace Wizdle.Maui;

using Foundation;

using Microsoft.Maui;
using Microsoft.Maui.Hosting;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp()
    {
        return MauiProgram.CreateMauiApp();
    }
}
