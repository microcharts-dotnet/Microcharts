#!/usr/bin/env bash
set -euo pipefail

# Packages contain both frameworks. Build Apple samples with the framework that
# matches the selected Xcode: .NET 10 needs Xcode 26.0; .NET 11 needs Xcode 27.2.
sample_framework="${1:-net11.0}"
case "$sample_framework" in
  net10.0|net11.0) ;;
  *) echo "Usage: bash buildpackages-maui.sh [net10.0|net11.0]" >&2; exit 1 ;;
esac

dotnet pack --configuration=Release Sources/Microcharts/Microcharts.csproj
dotnet pack --configuration=Release Sources/Microcharts.Maui/Microcharts.Maui.csproj
dotnet pack --configuration=Release Sources/Microcharts.Droid/Microcharts.Droid.csproj
dotnet pack --configuration=Release Sources/Microcharts.iOS/Microcharts.iOS.csproj
dotnet pack --configuration=Release Sources/Microcharts.Metapackage/Microcharts.Metapackage.csproj

for platform in android ios maccatalyst; do
  dotnet build --configuration=Release Sources/Microcharts.Samples.Maui/Microcharts.Samples.Maui.csproj --framework "$sample_framework-$platform"
done
dotnet build --configuration=Release Sources/Microcharts.Samples.Android/Microcharts.Samples.Android.csproj --framework "$sample_framework-android"
dotnet build --configuration=Release Sources/Microcharts.Samples.iOS/Microcharts.Samples.iOS.csproj --framework "$sample_framework-ios" -p:Platform=iPhoneSimulator
