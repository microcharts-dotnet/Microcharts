@echo off
msbuild /t:pack /p:Configuration=Release Sources\Microcharts\Microcharts.csproj
msbuild /t:pack /p:Configuration=Release Sources\Microcharts.Droid\Microcharts.Droid.csproj
msbuild /t:pack /p:Configuration=Release Sources\Microcharts.Forms\Microcharts.Forms.csproj
msbuild /t:pack /p:Configuration=Release Sources\Microcharts.iOS\Microcharts.iOS.csproj
msbuild /t:pack /p:Configuration=Release Sources\Microcharts.macOS\Microcharts.macOS.csproj
msbuild /t:pack /p:Configuration=Release Sources\Microcharts.Uwp\Microcharts.Uwp.csproj
msbuild /t:pack /p:Configuration=Release Sources\Microcharts.Avalonia\Microcharts.Avalonia.csproj
msbuild /t:pack /p:Configuration=Release Sources\Microcharts.Eto\Microcharts.Eto.csproj
msbuild /t:pack /p:Configuration=Release Sources\Microcharts.Maui\Microcharts.Maui.csproj
msbuild /t:pack /p:Configuration=Release Sources\Microcharts.Uno\Microcharts.Uno.csproj
msbuild /t:pack /p:Configuration=Release Sources\Microcharts.Uno.WinUI\Microcharts.Uno.WinUI.csproj
msbuild /t:pack /p:Configuration=Release Sources\Microcharts.WinUI\Microcharts.WinUI.csproj
