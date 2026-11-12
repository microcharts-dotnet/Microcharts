# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build Commands

```bash
# Build the full solution on Windows
dotnet build Sources/Microcharts.slnx --configuration Release

# Build the core and MAUI libraries for both frameworks
dotnet build Sources/Microcharts.Maui/Microcharts.Maui.csproj --configuration Release

# Pack both frameworks and build matching samples on macOS
bash buildpackages-maui.sh net10.0  # Xcode 26.0
bash buildpackages-maui.sh net11.0  # Xcode 27.2

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

Microcharts is a cross-platform charting library built on **SkiaSharp 4.x** targeting **.NET 10 and .NET 11**. All chart rendering is platform-agnostic SkiaSharp drawing; platform projects provide thin `ChartView` wrappers. Building the repository requires the .NET 11 SDK pinned in `global.json`.

### Project Dependency Graph

```
Microcharts.Core (.NET 10 + .NET 11: base, iOS, Android, Mac Catalyst, Windows)
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

- **pull-request.yml** - Runs on pull requests (and manual dispatch); builds both framework versions on `windows-latest`, `macos-26`, and `xcode-27`. Apple samples build separately: .NET 10 with Xcode `26.0.1`, .NET 11 with Xcode `27.2`.
- **publish.yml** - Manual trigger (`workflow_dispatch`); always publishes packages to GitHub Packages (`microcharts-dotnet` org), and additionally to nuget.org when the `publish_to_nuget` input is set (OIDC trusted publishing)

Both workflows use .NET SDK `11.0.100` and workload set `11.0.100.1`, pinned in `global.json` with `sdk.version` and `sdk.workloadVersion`. Its .NET 10 Apple workloads require Xcode 26.0; the .NET 11 workloads require Xcode 27.2. Bump the SDK, workload set and Xcode together. NuGet dependency versions are pinned independently in `Sources/Directory.Build.props`: `SkiaSharpVersion` for the SkiaSharp family and `MauiVersion` for MAUI packages, selected per target framework so .NET 10 consumers use MAUI 10 and .NET 11 consumers use MAUI 11.
