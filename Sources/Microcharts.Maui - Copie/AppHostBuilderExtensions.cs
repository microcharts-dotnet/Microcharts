using Microsoft.Maui.Hosting;
using SkiaSharp.Views.Maui.Controls;

namespace Microcharts.Maui
{
    public static class AppHostBuilderExtensions
    {
        public static IAppHostBuilder UseMicrocharts(this IAppHostBuilder builder) =>
            builder.UseSkiaSharpHandlers();
    }
}
