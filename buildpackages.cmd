@echo off
dotnet msbuild /t:restore /p:Configuration=Release Sources\Microcharts.sln
msbuild /t:build,pack /p:Configuration=Release Sources\Microcharts\Microcharts.csproj
msbuild /t:build,pack /p:Configuration=Release Sources\Microcharts.Droid\Microcharts.Droid.csproj
msbuild /t:build,pack /p:Configuration=Release Sources\Microcharts.iOS\Microcharts.iOS.csproj
rem msbuild /t:build,pack /p:Configuration=Release Sources\Microcharts.macOS\Microcharts.macOS.csproj
msbuild /t:build,pack /p:Configuration=Release Sources\Microcharts.Uwp\Microcharts.Uwp.csproj
msbuild /t:build,pack /p:Configuration=Release Sources\Microcharts.Maui\Microcharts.Maui.csproj
msbuild /t:build,pack /p:Configuration=Release Sources\Microcharts.WinUI\Microcharts.WinUI.csproj
dotnet pack Sources\Microcharts.Metapackage\Microcharts.Metapackage.csproj
