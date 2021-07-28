using Microcharts.Maui;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Hosting;

[assembly: XamlCompilationAttribute(XamlCompilationOptions.Compile)]

namespace Microcharts.Samples.Maui
{
    public class Startup : IStartup
    {
        public void Configure(IAppHostBuilder appBuilder)
        {
            appBuilder
                .UseMauiApp<App>()
                .UseMicrocharts();
        }
    }
}
