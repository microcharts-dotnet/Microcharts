# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build Commands

```bash
# Build the full solution (preferred)
dotnet build Sources/Microcharts.slnx --configuration Release

# Pack all NuGet packages (macOS/Linux)
./buildpackages-maui.sh

# Clean all bin/obj directories
./clean.sh

# Pack individual packages
dotnet pack Sources/Microcharts/Microcharts.csproj --configuration Release
dotnet pack Sources/Microcharts.Maui/Microcharts.Maui.csproj --configuration Release
dotnet pack Sources/Microcharts.Droid/Microcharts.Droid.csproj --configuration Release
dotnet pack Sources/Microcharts.iOS/Microcharts.iOS.csproj --configuration Release
dotnet pack Sources/Microcharts.Metapackage/Microcharts.Metapackage.csproj --configuration Release
```

Required .NET workloads: `android`, `ios`, `maccatalyst`, `maui`.

There are no test projects in this repository.

## Architecture

Microcharts is a cross-platform charting library built on **SkiaSharp 4.x** targeting **.NET 10**. All chart rendering is platform-agnostic SkiaSharp drawing; platform projects provide thin `ChartView` wrappers.

### Project Dependency Graph

```
Microcharts.Core (net10.0, net10.0-ios, net10.0-android, net10.0-maccatalyst, net10.0-windows)
  ^           ^            ^
  |           |            |
  Maui      iOS         Droid
  (ChartView wrappers - each extends SKCanvasView for its platform)
```

The `Microcharts` meta-package aggregates all platform packages via a `.nuspec` file with target-framework-specific dependency groups.

### Chart Class Hierarchy

All charts inherit from `Chart` (abstract base in `Sources/Microcharts/Charts/Chart.cs`):

- **Chart** - Provides `Draw(SKCanvas, width, height)`, animation, property change notification, and the `Invalidated` event that platform views subscribe to for re-rendering.
  - **SimpleChart** - Single-series charts: `PieChart`, `DonutChart`, `RadialGaugeChart`, `HalfRadialGaugeChart`, `RadarChart`
  - **SeriesChart** - Multi-series with `ChartSerie` collections
    - **PointChart** - `LineChart`
    - **AxisBasedChart** - `BarChart` (handles axis drawing, labels, grid)

### Rendering Pipeline

1. Platform `ChartView` subscribes to `Chart.Invalidated` via weak event handler (prevents memory leaks)
2. On invalidation, view calls platform-specific redraw (`InvalidateSurface()` on MAUI, `SetNeedsDisplayInRect()` on iOS, `Invalidate()` on Android)
3. `Chart.Draw()` fills background, then delegates to `DrawContent()` (abstract, implemented by each chart type)
4. Charts animate by interpolating `AnimationProgress` from 0 to 1 over `AnimationDuration`

### Data Model

- `ChartEntry` - A single data point (Value, Label, ValueLabel, Color)
- `ChartSerie` - Named collection of entries with optional color override

### Platform Views

Each platform project contains a single `ChartView` class that extends `SKCanvasView` and exposes a `Chart` property. MAUI's version adds `BindableProperty` for XAML binding. MAUI apps must call `UseMicrocharts()` on `MauiAppBuilder`.

## Versioning and Packaging

Version is managed centrally in `Sources/Directory.Build.props` (`VersionMain` property). All package output goes to the `/artifacts` directory. The publish.yml workflow appends the GitHub run number as a prerelease suffix.

## CI/CD

- **pull-request.yml** - Runs on pull requests (and manual dispatch); builds on `windows-latest` and `macos-26` (with Xcode `26.6` via `maxim-lobanov/setup-xcode@v1`)
- **publish.yml** - Manual trigger (`workflow_dispatch`); always publishes packages to GitHub Packages (`microcharts-dotnet` org), and additionally to nuget.org when the `publish_to_nuget` input is set (OIDC trusted publishing)

Both workflows pin .NET SDK `10.0.401` and workload set `10.0.401` (iOS 26.5, MAUI 10.0.20). Newer workload sets bring the iOS 27 workload, which needs Xcode 27, and raise `$(MauiVersion)` — the `Microsoft.Maui.Controls` floor of the published `Microcharts.Maui` package. Bump the SDK, workload set and Xcode together.
