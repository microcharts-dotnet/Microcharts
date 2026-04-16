dotnet pack --configuration=Release Sources/Microcharts/Microcharts.csproj
dotnet pack --configuration=Release Sources/Microcharts.Maui/Microcharts.Maui.csproj
dotnet pack --configuration=Release Sources/Microcharts.Droid/Microcharts.Droid.csproj
dotnet pack --configuration=Release Sources/Microcharts.iOS/Microcharts.iOS.csproj
dotnet pack --configuration=Release Sources/Microcharts.Metapackage/Microcharts.Metapackage.csproj

dotnet build --configuration=Release Sources/Microcharts.Samples.Maui/Microcharts.Samples.Maui.csproj
dotnet build --configuration=Release Sources/Microcharts.Samples.Android/Microcharts.Samples.Android.csproj
dotnet build --configuration=Release Sources/Microcharts.Samples.iOS/Microcharts.Samples.iOS.csproj
