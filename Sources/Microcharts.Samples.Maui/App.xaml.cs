using Microsoft.Maui.Controls;
using Application = Microsoft.Maui.Controls.Application;

namespace Microcharts.Samples.Maui
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }
    }
}
