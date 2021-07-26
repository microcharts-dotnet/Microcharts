using System;
using System.Collections.Generic;
using System.Linq;
using SkiaSharp;

namespace Microcharts.Samples
{
    public static class Data
    {
        #region Colors

        public static readonly SKColor TextColor = SKColors.Gray;

        public static readonly SKColor[] Colors =
        {
                        SKColor.Parse("#266489"),
                        SKColor.Parse("#68B9C0"),
                        SKColor.Parse("#90D585"),
                        SKColor.Parse("#F3C151"),
                        SKColor.Parse("#F37F64"),
                        SKColor.Parse("#424856"),
                        SKColor.Parse("#8F97A4"),
                        SKColor.Parse("#DAC096"),
                        SKColor.Parse("#76846E"),
                        SKColor.Parse("#DABFAF"),
                        SKColor.Parse("#A65B69"),
                        SKColor.Parse("#97A69D"),
                };

        private static int ColorIndex = 0;

        public static SKColor NextColor()
        {
            var result = Colors[ColorIndex];
            ColorIndex = (ColorIndex + 1) % Colors.Length;
            return result;
        }

        #endregion

        public static (string label, int value)[] PositiveData =
        {
            ("January",     400),
            ("February",    600),
            ("March",       900),
            ("April",       100),
            ("May",         200),
            ("June",        500),
            ("July",        300),
            ("August",      200),
            ("September",   200),
            ("October",     800),
            ("November",    950),
            ("December",    700),

        };

        public static (string label, int value)[] MixedData =
        {
            ("January",    -400),
            ("February",    600),
            ("March",       900),
            ("April",       100),
            ("May",        -200),
            ("June",        500),
            ("July",        300),
            ("August",     -200),
            ("September",   200),
            ("October",     800),
            ("November",    950),
            ("December",   -700),

        };

        public static (string label, int value)[] NegativeData =
        {
            ("January",     -400),
            ("February",    -600),
            ("March",       -900),
            ("April",       -100),
            ("May",         -200),
            ("June",        -500),
            ("July",        -300),
            ("August",      -200),
            ("September",   -200),
            ("October",     -800),
            ("November",    -950),
            ("December",    -700),

        };

        public static Chart[] CreateXamarinLegacySample()
        {
            ChartEntry[] entries = GenerateDefaultXamarinEntries();
            return new Chart[]
            {
                new LegacyBarChart
                {
                    Entries = entries,
                    LabelTextSize = 42,
                    LabelOrientation = Orientation.Horizontal
                },
                new LegacyPointChart
                {
                    Entries = entries,
                    LabelTextSize = 42,
                    LabelOrientation = Orientation.Horizontal
                },
                new LegacyLineChart
                {
                    Entries = entries,
                    LineMode = LineMode.Straight,
                    LineSize = 8,
                    LabelTextSize = 42,
                    PointMode = PointMode.Square,
                    PointSize = 18,
                },
            };
        }

        public static Chart[] CreateXamarinSample()
        {
            ChartEntry[] entries = GenerateDefaultXamarinEntries();
            Random r = new Random(18);
            return new Chart[]
            {
                new BarChart
                {
                    LabelOrientation = Orientation.Horizontal,
                    ValueLabelOrientation = Orientation.Horizontal,
                    LabelTextSize = 42,
                    ValueLabelTextSize= 18,
                    SerieLabelTextSize = 42,
                    ValueLabelOption = ValueLabelOption.TopOfElement,
                    LegendOption = SeriesLegendOption.Bottom,
                    Series = new List<ChartSerie>()
                    {
                        new ChartSerie()
                        {
                            Name = "UWP",
                            Color = SKColor.Parse("#2c3e50"),
                            Entries = GenerateSeriesEntry(r),
                        },
                        new ChartSerie()
                        {
                            Name = "Android",
                            Color = SKColor.Parse("#77d065"),
                            Entries = GenerateSeriesEntry(r),
                        },
                        new ChartSerie()
                        {
                            Name = "iOS",
                            Color = SKColor.Parse("#b455b6"),
                            Entries = GenerateSeriesEntry(r),
                        },
                    }
                },
                new PointChart
                {
                    LabelOrientation = Orientation.Horizontal,
                    ValueLabelOrientation = Orientation.Horizontal,
                    LabelTextSize = 42,
                    ValueLabelTextSize= 18,
                    SerieLabelTextSize = 42,
                    LegendOption = SeriesLegendOption.Bottom,
                    Series = new List<ChartSerie>()
                    {
                        new ChartSerie()
                        {
                            Name = "UWP",
                            Color = SKColor.Parse("#2c3e50"),
                            Entries = GenerateSeriesEntry(r),
                        },
                        new ChartSerie()
                        {
                            Name = "Android",
                            Color = SKColor.Parse("#77d065"),
                            Entries = GenerateSeriesEntry(r),
                        },
                        new ChartSerie()
                        {
                            Name = "iOS",
                            Color = SKColor.Parse("#b455b6"),
                            Entries = GenerateSeriesEntry(r),
                        },
                    }
                },
                new LineChart
                {
                    LabelOrientation = Orientation.Horizontal,
                    ValueLabelOrientation = Orientation.Horizontal,
                    LabelTextSize = 42,
                    ValueLabelTextSize= 18,
                    SerieLabelTextSize = 42,
                    LegendOption = SeriesLegendOption.Bottom,
                    Series = new List<ChartSerie>()
                    {
                        new ChartSerie()
                        {
                            Name = "UWP",
                            Color = SKColor.Parse("#2c3e50"),
                            Entries = GenerateSeriesEntry(r, 4),
                        },
                        new ChartSerie()
                        {
                            Name = "Android",
                            Color = SKColor.Parse("#77d065"),
                            Entries = GenerateSeriesEntry(r, 4),
                        },
                        new ChartSerie()
                        {
                            Name = "iOS",
                            Color = SKColor.Parse("#b455b6"),
                            Entries = GenerateSeriesEntry(r, 4),
                        },
                    }
                },
                new DonutChart
                {
                    Entries = entries,
                    LabelTextSize = 42,
                    GraphPosition = GraphPosition.Center,
                    LabelMode = LabelMode.RightOnly
                },
                new RadialGaugeChart
                {
                    Entries = entries,
                    LabelTextSize = 42
                },
                new RadarChart
                {
                    Entries = entries,
                    LabelTextSize = 42
                },
            };
        }

        private static ChartEntry[] GenerateDefaultXamarinEntries()
        {
            return new[]
            {
                new ChartEntry(212)
                {
                    Label = "UWP",
                    ValueLabel = "112",
                    Color = SKColor.Parse("#2c3e50"),
                },
                new ChartEntry(248)
                {
                    Label = "Android",
                    ValueLabel = "648",
                    Color = SKColor.Parse("#77d065"),
                },
                new ChartEntry(128)
                {
                    Label = "iOS",
                    ValueLabel = "428",
                    Color = SKColor.Parse("#b455b6"),
                },
                new ChartEntry(514)
                {
                    Label = "Forms",
                    ValueLabel = "214",
                    Color = SKColor.Parse("#3498db"),
                }
            };
        }

        public static IEnumerable<ExampleChartItem> CreateXamarinExampleChartItem(string chartType)
        {
            switch (chartType)
            {
                case nameof(LegacyBarChart):
                    return GenerateBarChartExample();
                case nameof(LegacyPointChart):
                    return GeneratePointChartExample();
                case nameof(LegacyLineChart):
                    return GenerateLineChartExample();
                case nameof(DonutChart):
                    return GenerateDonutChartExample();
                case nameof(RadialGaugeChart):
                    return GenerateRadialGaugeChartExample();
                case nameof(RadarChart):
                    return GenerateRadarChartExample();
                case nameof(BarChart):
                    return GenerateGroupedBarChartExample();
                case nameof(PointChart):
                    return GeneratePointSeriesChartExample();
                case nameof(LineChart):
                    return GenerateLineSeriesChartExample();
                default:
                    return null;
            }
        }

        private static IEnumerable<ExampleChartItem> GenerateLineChartExample()
        {
            yield return new ExampleChartItem()
            {
                ExampleName = "Default",
                ExampleTextDescription = "Default example",
                Chart = new LegacyLineChart
                {
                    Entries = GenerateDefaultXamarinEntries(),
                    LineMode = LineMode.Straight,
                    LineSize = 8,
                    LabelTextSize = 42,
                    PointMode = PointMode.Square,
                    PointSize = 18,
                },
            };

            yield return new ExampleChartItem()
            {
                ExampleName = "Show Y axis at right",
                ExampleTextDescription = "Display Y axis lines and values",
                Chart = new LegacyLineChart
                {
                    Entries = GenerateDefaultXamarinEntries(),
                    LineMode = LineMode.Straight,
                    LineSize = 8,
                    LabelTextSize = 42,
                    PointMode = PointMode.Square,
                    PointSize = 18,
                    ShowYAxisLines = true,
                    ShowYAxisText = true,
                    YAxisPosition = Position.Right
                }
            };

            yield return new ExampleChartItem()
            {
                ExampleName = "Show Y axis at left",
                ExampleTextDescription = "Display Y axis lines and values",
                Chart = new LegacyLineChart
                {
                    Entries = GenerateDefaultXamarinEntries(),
                    LineMode = LineMode.Straight,
                    LineSize = 8,
                    LabelTextSize = 42,
                    PointMode = PointMode.Square,
                    PointSize = 18,
                    ShowYAxisLines = true,
                    ShowYAxisText = true,
                    YAxisPosition = Position.Left
                }
            };

            yield break;
        }

        private static IEnumerable<ExampleChartItem> GenerateDonutChartExample()
        {
            yield return new ExampleChartItem()
            {
                ExampleName = "Default",
                ExampleTextDescription = "Default example",
                Chart = new DonutChart
                {
                    Entries = GenerateDefaultXamarinEntries(),
                    LabelTextSize = 42,
                    GraphPosition = GraphPosition.Center,
                    LabelMode = LabelMode.RightOnly
                },
            };

            yield return new ExampleChartItem()
            {
                ExampleName = "Two entry",
                ExampleTextDescription = "First item greater than the second one (issue #184)",
                Chart = new DonutChart
                {
                    Entries = new List<ChartEntry>()
                    {
                        new ChartEntry(458)
                        {
                            Label = "Android",
                            ValueLabel = "458",
                            Color = SKColor.Parse("#77d065"),
                        },
                        new ChartEntry(128)
                        {
                            Label = "iOS",
                            ValueLabel = "128",
                            Color = SKColor.Parse("#b455b6"),
                        },
                    },
                    LabelTextSize = 32,
                    GraphPosition = GraphPosition.Center,
                    LabelMode = LabelMode.RightOnly
                },
            };

            yield break;
        }

        private static IEnumerable<ExampleChartItem> GenerateRadialGaugeChartExample()
        {
            yield return new ExampleChartItem()
            {
                ExampleName = "Default",
                ExampleTextDescription = "Default example",
                Chart = new RadialGaugeChart
                {
                    Entries = GenerateDefaultXamarinEntries(),
                    LabelTextSize = 42
                },
            };

            yield break;
        }

        private static IEnumerable<ExampleChartItem> GenerateRadarChartExample()
        {
            yield return new ExampleChartItem()
            {
                ExampleName = "Default",
                ExampleTextDescription = "Default example",
                Chart = new RadarChart
                {
                    Entries = GenerateDefaultXamarinEntries(),
                    LabelTextSize = 42
                },
            };

            yield break;
        }

        private static IEnumerable<ExampleChartItem> GenerateGroupedBarChartExample()
        {
            Random r = new Random((int)DateTime.Now.Ticks);
            yield return new ExampleChartItem()
            {
                ExampleName = "Default",
                ExampleTextDescription = "Default example",
                Chart = new BarChart
                {
                    Entries = GenerateDefaultXamarinEntries(),
                    LabelTextSize = 42,
                    LabelOrientation = Orientation.Horizontal
                }
            };

            var entries = GenerateDefaultXamarinEntries();
            Array.ForEach(entries, (e) => e.Label = string.Empty);
            yield return new ExampleChartItem()
            {
                ExampleName = "Empty label",
                ExampleTextDescription = "Default data with empty label (issue #137)",
                Chart = new BarChart
                {
                    Entries = entries,
                    LabelTextSize = 42,
                    LabelOrientation = Orientation.Horizontal
                }
            };

            yield return new ExampleChartItem()
            {
                ExampleName = "Show Y axis at right",
                ExampleTextDescription = "Display Y axis lines and values",
                Chart = new BarChart
                {
                    Entries = GenerateDefaultXamarinEntries(),
                    LabelTextSize = 42,
                    ShowYAxisLines = true,
                    ShowYAxisText = true,
                    LabelOrientation = Orientation.Horizontal,
                    ValueLabelOrientation = Orientation.Horizontal,
                    YAxisPosition = Position.Right
                }
            };

            yield return new ExampleChartItem()
            {
                ExampleName = "Show Y axis at left",
                ExampleTextDescription = "Display Y axis lines and values",
                Chart = new BarChart
                {
                    Entries = GenerateDefaultXamarinEntries(),
                    LabelTextSize = 42,
                    ShowYAxisLines = true,
                    ShowYAxisText = true,
                    LabelOrientation = Orientation.Horizontal,
                    YAxisPosition = Position.Left
                }
            };

            yield return new ExampleChartItem()
            {
                ExampleName = "Simple chart with vertical label on top of bar",
                ExampleTextDescription = "Display Y axis lines and values, with value label on top of bar",
                Chart = new BarChart
                {
                    Entries = GenerateDefaultXamarinEntries(),
                    LabelTextSize = 42,
                    ShowYAxisLines = true,
                    ShowYAxisText = true,
                    LabelOrientation = Orientation.Horizontal,
                    ValueLabelOption = ValueLabelOption.TopOfElement,
                    YAxisPosition = Position.Left
                }
            };

            yield return new ExampleChartItem()
            {
                ExampleName = "Simple chart with horizontal label on top of bar",
                ExampleTextDescription = "Display Y axis lines and values, with value label on top of bar",
                Chart = new BarChart
                {
                    Entries = GenerateDefaultXamarinEntries(),
                    LabelTextSize = 42,
                    ShowYAxisLines = true,
                    ShowYAxisText = true,
                    LabelOrientation = Orientation.Horizontal,
                    ValueLabelOrientation = Orientation.Horizontal,
                    ValueLabelOption = ValueLabelOption.TopOfElement,
                    YAxisPosition = Position.Left
                }
            };

            yield return new ExampleChartItem()
            {
                ExampleName = "Simple chart with vertical label over bar",
                ExampleTextDescription = "Display Y axis lines and values, with value label over bar",
                Chart = new BarChart
                {
                    Entries = GenerateDefaultXamarinEntries(),
                    LabelTextSize = 42,
                    ShowYAxisLines = true,
                    ShowYAxisText = true,
                    LabelOrientation = Orientation.Horizontal,
                    ValueLabelOrientation = Orientation.Vertical,
                    ValueLabelOption = ValueLabelOption.OverElement,
                    YAxisPosition = Position.Left
                }
            };

            yield return new ExampleChartItem()
            {
                ExampleName = "Simple chart with horizontal label over bar",
                ExampleTextDescription = "Display Y axis lines and values, with value label over bar",
                Chart = new BarChart
                {
                    AnimationDuration = TimeSpan.FromSeconds(0),
                    Entries = GenerateDefaultXamarinEntries(),
                    LabelTextSize = 42,
                    ShowYAxisLines = true,
                    ShowYAxisText = true,
                    LabelOrientation = Orientation.Horizontal,
                    ValueLabelOrientation = Orientation.Horizontal,
                    ValueLabelOption = ValueLabelOption.OverElement,
                    YAxisPosition = Position.Left
                }
            };

            yield return new ExampleChartItem()
            {
                ExampleName = "Bottom legend",
                ExampleTextDescription = "Grouped bar chart with legend at bottom with vertical value label orientation",
                ExampleChartType = ExampleChartType.Series,
                Chart = new BarChart
                {
                    LabelOrientation = Orientation.Horizontal,
                    ValueLabelOrientation = Orientation.Vertical,
                    LabelTextSize = 42,
                    ValueLabelTextSize = 18,
                    SerieLabelTextSize = 42,
                    LegendOption = SeriesLegendOption.Bottom,
                    Series = new List<ChartSerie>()
                    {
                        new ChartSerie()
                        {
                            Name = "UWP",
                            Color = SKColor.Parse("#2c3e50"),
                            Entries = GenerateSeriesEntry(r),
                        },
                        new ChartSerie()
                        {
                            Name = "Android",
                            Color = SKColor.Parse("#77d065"),
                            Entries = GenerateSeriesEntry(r),
                        },
                        new ChartSerie()
                        {
                            Name = "iOS",
                            Color = SKColor.Parse("#b455b6"),
                            Entries = GenerateSeriesEntry(r),
                        },
                    }
                },
            };

            yield return new ExampleChartItem()
            {
                ExampleName = "Top legend",
                ExampleTextDescription = "Grouped bar chart with legend at top",
                ExampleChartType = ExampleChartType.Series,
                Chart = new BarChart
                {
                    LabelOrientation = Orientation.Horizontal,
                    ValueLabelOrientation = Orientation.Horizontal,
                    LabelTextSize = 42,
                    ValueLabelTextSize = 18,
                    SerieLabelTextSize = 42,
                    LegendOption = SeriesLegendOption.Top,
                    Series = new List<ChartSerie>()
                    {
                        new ChartSerie()
                        {
                            Name = "UWP",
                            Color = SKColor.Parse("#2c3e50"),
                            Entries = GenerateSeriesEntry(r),
                        },
                        new ChartSerie()
                        {
                            Name = "Android",
                            Color = SKColor.Parse("#77d065"),
                            Entries = GenerateSeriesEntry(r),
                        },
                        new ChartSerie()
                        {
                            Name = "iOS",
                            Color = SKColor.Parse("#b455b6"),
                            Entries = GenerateSeriesEntry(r),
                        },
                    }
                },
            };

            yield return new ExampleChartItem()
            {
                ExampleName = "No legend",
                ExampleTextDescription = "Grouped bar chart without legend",
                ExampleChartType = ExampleChartType.Series,
                Chart = new BarChart
                {
                    LabelOrientation = Orientation.Horizontal,
                    ValueLabelOrientation = Orientation.Horizontal,
                    LabelTextSize = 42,
                    ValueLabelTextSize = 18,
                    SerieLabelTextSize = 42,
                    LegendOption = SeriesLegendOption.None,
                    Series = new List<ChartSerie>()
                    {
                        new ChartSerie()
                        {
                            Name = "UWP",
                            Color = SKColor.Parse("#2c3e50"),
                            Entries = GenerateSeriesEntry(r),
                        },
                        new ChartSerie()
                        {
                            Name = "Android",
                            Color = SKColor.Parse("#77d065"),
                            Entries = GenerateSeriesEntry(r),
                        },
                        new ChartSerie()
                        {
                            Name = "iOS",
                            Color = SKColor.Parse("#b455b6"),
                            Entries = GenerateSeriesEntry(r),
                        },
                    }
                },
            };

            yield return new ExampleChartItem()
            {
                ExampleName = "Y Axis with text",
                ExampleTextDescription = "Grouped bar chart with default legend and Y Axis",
                ExampleChartType = ExampleChartType.Series,
                Chart = new BarChart
                {
                    LabelOrientation = Orientation.Horizontal,
                    ValueLabelOrientation = Orientation.Horizontal,
                    LabelTextSize = 42,
                    ValueLabelTextSize = 18,
                    SerieLabelTextSize = 42,
                    ShowYAxisLines = true,
                    ShowYAxisText = true,
                    YAxisPosition = Position.Left,
                    Series = new List<ChartSerie>()
                    {
                        new ChartSerie()
                        {
                            Name = "UWP",
                            Color = SKColor.Parse("#2c3e50"),
                            Entries = GenerateSeriesEntry(r),
                        },
                        new ChartSerie()
                        {
                            Name = "Android",
                            Color = SKColor.Parse("#77d065"),
                            Entries = GenerateSeriesEntry(r),
                        },
                        new ChartSerie()
                        {
                            Name = "iOS",
                            Color = SKColor.Parse("#b455b6"),
                            Entries = GenerateSeriesEntry(r),
                        },
                    }
                },
            };

            yield return new ExampleChartItem()
            {
                ExampleName = "Value label top of Bar item",
                ExampleTextDescription = "Grouped bar chart with default legend and Y Axis and value display on top of bars.",
                ExampleChartType = ExampleChartType.Series,
                Chart = new BarChart
                {
                    LabelOrientation = Orientation.Horizontal,
                    ValueLabelOrientation = Orientation.Horizontal,
                    LabelTextSize = 42,
                    ValueLabelTextSize = 18,
                    ValueLabelOption = ValueLabelOption.TopOfElement,
                    SerieLabelTextSize = 42,
                    ShowYAxisLines = true,
                    ShowYAxisText = true,
                    YAxisPosition = Position.Left,
                    Series = new List<ChartSerie>()
                    {
                        new ChartSerie()
                        {
                            Name = "UWP",
                            Color = SKColor.Parse("#2c3e50"),
                            Entries = GenerateSeriesEntry(r),
                        },
                        new ChartSerie()
                        {
                            Name = "Android",
                            Color = SKColor.Parse("#77d065"),
                            Entries = GenerateSeriesEntry(r),
                        },
                        new ChartSerie()
                        {
                            Name = "iOS",
                            Color = SKColor.Parse("#b455b6"),
                            Entries = GenerateSeriesEntry(r),
                        },
                    }
                },
            };

            yield return new ExampleChartItem()
            {
                ExampleName = "Value label over Bar item",
                ExampleTextDescription = "Grouped bar chart with default legend and Y Axis and value display over bars.",
                ExampleChartType = ExampleChartType.Series,
                Chart = new BarChart
                {
                    LabelOrientation = Orientation.Horizontal,
                    ValueLabelOrientation = Orientation.Horizontal,
                    LabelTextSize = 42,
                    ValueLabelTextSize = 18,
                    ValueLabelOption = ValueLabelOption.OverElement,
                    SerieLabelTextSize = 42,
                    ShowYAxisLines = true,
                    ShowYAxisText = true,
                    YAxisPosition = Position.Left,
                    Series = new List<ChartSerie>()
                    {
                        new ChartSerie()
                        {
                            Name = "UWP",
                            Color = SKColor.Parse("#2c3e50"),
                            Entries = GenerateSeriesEntry(r),
                        },
                        new ChartSerie()
                        {
                            Name = "Android",
                            Color = SKColor.Parse("#77d065"),
                            Entries = GenerateSeriesEntry(r),
                        },
                        new ChartSerie()
                        {
                            Name = "iOS",
                            Color = SKColor.Parse("#b455b6"),
                            Entries = GenerateSeriesEntry(r),
                        },
                    }
                },
            };

            yield return new ExampleChartItem()
            {
                ExampleName = "Empty label",
                ExampleTextDescription = "Default data with empty label (issue #137)",
                ExampleChartType = ExampleChartType.Series,
                Chart = new BarChart
                {
                    LabelOrientation = Orientation.Horizontal,
                    ValueLabelOrientation = Orientation.Horizontal,
                    LabelTextSize = 42,
                    ValueLabelTextSize = 18,
                    SerieLabelTextSize = 42,
                    ShowYAxisLines = true,
                    ShowYAxisText = true,
                    YAxisPosition = Position.Left,
                    Series = new List<ChartSerie>()
                        {
                            new ChartSerie()
                            {
                                Name = "UWP",
                                Color = SKColor.Parse("#2c3e50"),
                                Entries = GenerateSeriesEntry(r, withLabel:false),
                            },
                            new ChartSerie()
                            {
                                Name = "Android",
                                Color = SKColor.Parse("#77d065"),
                                Entries = GenerateSeriesEntry(r, withLabel:false),
                            },
                            new ChartSerie()
                            {
                                Name = "iOS",
                                Color = SKColor.Parse("#b455b6"),
                                Entries = GenerateSeriesEntry(r, withLabel:false),
                            },
                        }
                },
            };

            yield break;
        }

        private static IEnumerable<ExampleChartItem> GeneratePointSeriesChartExample()
        {
            Random r = new Random((int)DateTime.Now.Ticks);
            yield return new ExampleChartItem()
            {
                ExampleName = "Default",
                ExampleTextDescription = "Default example",
                Chart = new PointChart
                {
                    Entries = GenerateDefaultXamarinEntries(),
                    LabelTextSize = 42,
                    LabelOrientation = Orientation.Horizontal
                }
            };

            yield return new ExampleChartItem()
            {
                ExampleName = "Show Y axis at right",
                ExampleTextDescription = "Display Y axis lines and values",
                Chart = new PointChart
                {
                    Entries = GenerateDefaultXamarinEntries(),
                    LabelTextSize = 42,
                    ShowYAxisLines = true,
                    ShowYAxisText = true,
                    LabelOrientation = Orientation.Horizontal,
                    ValueLabelOrientation = Orientation.Horizontal,
                    YAxisPosition = Position.Right
                }
            };

            yield return new ExampleChartItem()
            {
                ExampleName = "Show Y axis at left",
                ExampleTextDescription = "Display Y axis lines and values",
                Chart = new PointChart
                {
                    Entries = GenerateDefaultXamarinEntries(),
                    LabelTextSize = 42,
                    ShowYAxisLines = true,
                    ShowYAxisText = true,
                    LabelOrientation = Orientation.Horizontal,
                    YAxisPosition = Position.Left
                }
            };

            yield return new ExampleChartItem()
            {
                ExampleName = "Value label vertical on top of point",
                ExampleTextDescription = "Display Y axis lines and values with value label on top of point",
                Chart = new PointChart
                {
                    Entries = GenerateDefaultXamarinEntries(),
                    LabelTextSize = 42,
                    ShowYAxisLines = true,
                    ShowYAxisText = true,
                    ValueLabelTextSize = 20,
                    LabelOrientation = Orientation.Horizontal,
                    ValueLabelOrientation = Orientation.Vertical,
                    ValueLabelOption = ValueLabelOption.TopOfElement,
                    YAxisPosition = Position.Left
                }
            };

            yield return new ExampleChartItem()
            {
                ExampleName = "Value label horizontal on top of point",
                ExampleTextDescription = "Display Y axis lines and values with value label on top of point",
                Chart = new PointChart
                {
                    Entries = GenerateDefaultXamarinEntries(),
                    LabelTextSize = 42,
                    ShowYAxisLines = true,
                    ShowYAxisText = true,
                    ValueLabelTextSize = 20,
                    LabelOrientation = Orientation.Horizontal,
                    ValueLabelOrientation = Orientation.Horizontal,
                    ValueLabelOption = ValueLabelOption.TopOfElement,
                    YAxisPosition = Position.Left
                }
            };

            yield return new ExampleChartItem()
            {
                ExampleName = "Value label on point vertical",
                ExampleTextDescription = "Display Y axis lines and values with value label on top of point",
                Chart = new PointChart
                {
                    Entries = GenerateDefaultXamarinEntries(),
                    LabelTextSize = 42,
                    ShowYAxisLines = true,
                    ShowYAxisText = true,
                    LabelOrientation = Orientation.Horizontal,
                    ValueLabelTextSize = 20,
                    ValueLabelOption = ValueLabelOption.OverElement,
                    YAxisPosition = Position.Left
                }
            };

            yield return new ExampleChartItem()
            {
                ExampleName = "Value label on point horizontal",
                ExampleTextDescription = "Display Y axis lines and values with value label on top of point",
                Chart = new PointChart
                {
                    Entries = GenerateDefaultXamarinEntries(),
                    LabelTextSize = 42,
                    ShowYAxisLines = true,
                    ShowYAxisText = true,
                    LabelOrientation = Orientation.Horizontal,
                    ValueLabelTextSize = 20,
                    ValueLabelOrientation = Orientation.Horizontal,
                    ValueLabelOption = ValueLabelOption.OverElement,
                    YAxisPosition = Position.Left
                }
            };

            yield return new ExampleChartItem()
            {
                ExampleName = "Bottom legend",
                ExampleTextDescription = "Grouped point chart with legend at bottom with vertical value label orientation",
                ExampleChartType = ExampleChartType.Series,
                Chart = new PointChart
                {
                    LabelOrientation = Orientation.Horizontal,
                    ValueLabelOrientation = Orientation.Vertical,
                    LabelTextSize = 42,
                    PointSize = 28,
                    ValueLabelTextSize = 18,
                    SerieLabelTextSize = 42,
                    LegendOption = SeriesLegendOption.Bottom,
                    Series = new List<ChartSerie>()
                    {
                        new ChartSerie()
                        {
                            Name = "UWP",
                            Color = SKColor.Parse("#2c3e50"),
                            Entries = GenerateSeriesEntry(r),
                        },
                        new ChartSerie()
                        {
                            Name = "Android",
                            Color = SKColor.Parse("#77d065"),
                            Entries = GenerateSeriesEntry(r),
                        },
                        new ChartSerie()
                        {
                            Name = "iOS",
                            Color = SKColor.Parse("#b455b6"),
                            Entries = GenerateSeriesEntry(r),
                        },
                    }
                },
            };

            yield return new ExampleChartItem()
            {
                ExampleName = "Top legend",
                ExampleTextDescription = "Grouped point chart with legend at top",
                ExampleChartType = ExampleChartType.Series,
                Chart = new PointChart
                {
                    LabelOrientation = Orientation.Horizontal,
                    ValueLabelOrientation = Orientation.Horizontal,
                    LabelTextSize = 42,
                    ValueLabelTextSize = 18,
                    SerieLabelTextSize = 42,
                    LegendOption = SeriesLegendOption.Top,
                    Series = new List<ChartSerie>()
                    {
                        new ChartSerie()
                        {
                            Name = "UWP",
                            Color = SKColor.Parse("#2c3e50"),
                            Entries = GenerateSeriesEntry(r),
                        },
                        new ChartSerie()
                        {
                            Name = "Android",
                            Color = SKColor.Parse("#77d065"),
                            Entries = GenerateSeriesEntry(r),
                        },
                        new ChartSerie()
                        {
                            Name = "iOS",
                            Color = SKColor.Parse("#b455b6"),
                            Entries = GenerateSeriesEntry(r),
                        },
                    }
                },
            };

            yield return new ExampleChartItem()
            {
                ExampleName = "No legend",
                ExampleTextDescription = "Grouped point chart without legend",
                ExampleChartType = ExampleChartType.Series,
                Chart = new PointChart
                {
                    LabelOrientation = Orientation.Horizontal,
                    ValueLabelOrientation = Orientation.Horizontal,
                    LabelTextSize = 42,
                    ValueLabelTextSize = 18,
                    SerieLabelTextSize = 42,
                    LegendOption = SeriesLegendOption.None,
                    Series = new List<ChartSerie>()
                    {
                        new ChartSerie()
                        {
                            Name = "UWP",
                            Color = SKColor.Parse("#2c3e50"),
                            Entries = GenerateSeriesEntry(r),
                        },
                        new ChartSerie()
                        {
                            Name = "Android",
                            Color = SKColor.Parse("#77d065"),
                            Entries = GenerateSeriesEntry(r),
                        },
                        new ChartSerie()
                        {
                            Name = "iOS",
                            Color = SKColor.Parse("#b455b6"),
                            Entries = GenerateSeriesEntry(r),
                        },
                    }
                },
            };

            yield return new ExampleChartItem()
            {
                ExampleName = "Y Axis with text",
                ExampleTextDescription = "Grouped point chart with default legend and Y Axis",
                ExampleChartType = ExampleChartType.Series,
                Chart = new PointChart
                {
                    LabelOrientation = Orientation.Horizontal,
                    ValueLabelOrientation = Orientation.Horizontal,
                    LabelTextSize = 42,
                    ValueLabelTextSize = 18,
                    SerieLabelTextSize = 42,
                    ShowYAxisLines = true,
                    ShowYAxisText = true,
                    YAxisPosition = Position.Left,
                    Series = new List<ChartSerie>()
                    {
                        new ChartSerie()
                        {
                            Name = "UWP",
                            Color = SKColor.Parse("#2c3e50"),
                            Entries = GenerateSeriesEntry(r),
                        },
                        new ChartSerie()
                        {
                            Name = "Android",
                            Color = SKColor.Parse("#77d065"),
                            Entries = GenerateSeriesEntry(r),
                        },
                        new ChartSerie()
                        {
                            Name = "iOS",
                            Color = SKColor.Parse("#b455b6"),
                            Entries = GenerateSeriesEntry(r),
                        },
                    }
                },
            };

            yield break;
        }

        private static IEnumerable<ExampleChartItem> GenerateLineSeriesChartExample()
        {
            Random r = new Random((int)DateTime.Now.Ticks);
            yield return new ExampleChartItem()
            {
                ExampleName = "Default",
                ExampleTextDescription = "Default example",
                Chart = new LineChart
                {
                    Entries = GenerateDefaultXamarinEntries(),
                    LabelTextSize = 42,
                    LabelOrientation = Orientation.Horizontal
                }
            };

            yield return new ExampleChartItem()
            {
                ExampleName = "Show Y axis at right",
                ExampleTextDescription = "Display Y axis lines and values",
                Chart = new LineChart
                {
                    Entries = GenerateDefaultXamarinEntries(),
                    LabelTextSize = 42,
                    ShowYAxisLines = true,
                    ShowYAxisText = true,
                    LabelOrientation = Orientation.Horizontal,
                    ValueLabelOrientation = Orientation.Horizontal,
                    YAxisPosition = Position.Right
                }
            };

            yield return new ExampleChartItem()
            {
                ExampleName = "Show Y axis at left",
                ExampleTextDescription = "Display Y axis lines and values",
                Chart = new LineChart
                {
                    Entries = GenerateDefaultXamarinEntries(),
                    LabelTextSize = 42,
                    ShowYAxisLines = true,
                    ShowYAxisText = true,
                    LabelOrientation = Orientation.Horizontal,
                    YAxisPosition = Position.Left
                }
            };

            yield return new ExampleChartItem()
            {
                ExampleName = "Bottom legend",
                ExampleTextDescription = "Multiple lines chart with legend at bottom with vertical value label orientation",
                ExampleChartType = ExampleChartType.Series,
                Chart = new LineChart
                {
                    LabelOrientation = Orientation.Horizontal,
                    ValueLabelOrientation = Orientation.Vertical,
                    LabelTextSize = 42,
                    PointSize = 28,
                    ValueLabelTextSize = 18,
                    SerieLabelTextSize = 42,
                    LegendOption = SeriesLegendOption.Bottom,
                    Series = new List<ChartSerie>()
                    {
                        new ChartSerie()
                        {
                            Name = "UWP",
                            Color = SKColor.Parse("#2c3e50"),
                            Entries = GenerateSeriesEntry(r, 5),
                        },
                        new ChartSerie()
                        {
                            Name = "Android",
                            Color = SKColor.Parse("#77d065"),
                            Entries = GenerateSeriesEntry(r, 5),
                        },
                        new ChartSerie()
                        {
                            Name = "iOS",
                            Color = SKColor.Parse("#b455b6"),
                            Entries = GenerateSeriesEntry(r, 5),
                        },
                    }
                },
            };

            yield return new ExampleChartItem()
            {
                ExampleName = "Top legend",
                ExampleTextDescription = "Multiple lines with legend at top and no area",
                ExampleChartType = ExampleChartType.Series,
                Chart = new LineChart
                {
                    LabelOrientation = Orientation.Horizontal,
                    ValueLabelOrientation = Orientation.Horizontal,
                    ValueLabelOption = ValueLabelOption.OverElement,
                    LabelTextSize = 42,
                    ValueLabelTextSize = 18,
                    SerieLabelTextSize = 42,
                    LineAreaAlpha = 0,
                    LegendOption = SeriesLegendOption.Top,
                    Series = new List<ChartSerie>()
                    {
                        new ChartSerie()
                        {
                            Name = "UWP",
                            Color = SKColor.Parse("#2c3e50"),
                            Entries = GenerateSeriesEntry(r, 5),
                        },
                        new ChartSerie()
                        {
                            Name = "Android",
                            Color = SKColor.Parse("#77d065"),
                            Entries = GenerateSeriesEntry(r, 5),
                        },
                        new ChartSerie()
                        {
                            Name = "iOS",
                            Color = SKColor.Parse("#b455b6"),
                            Entries = GenerateSeriesEntry(r, 5),
                        },
                    }
                },
            };

            yield return new ExampleChartItem()
            {
                ExampleName = "Straight lines",
                ExampleTextDescription = "Multiple lines without legend and line in Straight mode",
                ExampleChartType = ExampleChartType.Series,
                Chart = new LineChart
                {
                    LabelOrientation = Orientation.Horizontal,
                    ValueLabelOrientation = Orientation.Horizontal,
                    LabelTextSize = 42,
                    ValueLabelTextSize = 18,
                    SerieLabelTextSize = 42,
                    LineMode = LineMode.Straight,
                    LineAreaAlpha = 0,
                    LegendOption = SeriesLegendOption.None,
                    Series = new List<ChartSerie>()
                    {
                        new ChartSerie()
                        {
                            Name = "UWP",
                            Color = SKColor.Parse("#2c3e50"),
                            Entries = GenerateSeriesEntry(r, 5),
                        },
                        new ChartSerie()
                        {
                            Name = "Android",
                            Color = SKColor.Parse("#77d065"),
                            Entries = GenerateSeriesEntry(r, 5),
                        },
                        new ChartSerie()
                        {
                            Name = "iOS",
                            Color = SKColor.Parse("#b455b6"),
                            Entries = GenerateSeriesEntry(r, 5),
                        },
                    }
                },
            };

            yield return new ExampleChartItem()
            {
                ExampleName = "Y Axis with text",
                ExampleTextDescription = "Multiple lines chart with default legend and Y Axis",
                ExampleChartType = ExampleChartType.Series,
                Chart = new LineChart
                {
                    LabelOrientation = Orientation.Horizontal,
                    ValueLabelOrientation = Orientation.Horizontal,
                    LabelTextSize = 42,
                    ValueLabelTextSize = 18,
                    SerieLabelTextSize = 42,
                    LineAreaAlpha = 0,
                    ShowYAxisLines = true,
                    ShowYAxisText = true,
                    YAxisPosition = Position.Left,
                    Series = new List<ChartSerie>()
                    {
                        new ChartSerie()
                        {
                            Name = "UWP",
                            Color = SKColor.Parse("#2c3e50"),
                            Entries = GenerateSeriesEntry(r, 5),
                        },
                        new ChartSerie()
                        {
                            Name = "Android",
                            Color = SKColor.Parse("#77d065"),
                            Entries = GenerateSeriesEntry(r, 5),
                        },
                        new ChartSerie()
                        {
                            Name = "iOS",
                            Color = SKColor.Parse("#b455b6"),
                            Entries = GenerateSeriesEntry(r, 5),
                        },
                    }
                },
            };

            yield break;
        }

        private static IEnumerable<ExampleChartItem> GeneratePointChartExample()
        {
            yield return new ExampleChartItem()
            {
                ExampleName = "Default",
                ExampleTextDescription = "Default example",
                Chart = new LegacyPointChart
                {
                    Entries = GenerateDefaultXamarinEntries(),
                    LabelTextSize = 42,
                    LabelOrientation = Orientation.Horizontal
                },
            };

            yield return new ExampleChartItem()
            {
                ExampleName = "Show Y axis at right",
                ExampleTextDescription = "Display Y axis lines and values",
                Chart = new LegacyPointChart
                {
                    Entries = GenerateDefaultXamarinEntries(),
                    LabelTextSize = 42,
                    ShowYAxisLines = true,
                    ShowYAxisText = true,
                    LabelOrientation = Orientation.Horizontal,

                    YAxisPosition = Position.Right
                }
            };

            yield return new ExampleChartItem()
            {
                ExampleName = "Show Y axis at left",
                ExampleTextDescription = "Display Y axis lines and values",
                Chart = new LegacyPointChart
                {
                    Entries = GenerateDefaultXamarinEntries(),
                    LabelTextSize = 42,
                    ShowYAxisLines = true,
                    ShowYAxisText = true,
                    LabelOrientation = Orientation.Horizontal,

                    YAxisPosition = Position.Left
                }
            };

            yield break;
        }

        private static IEnumerable<ExampleChartItem> GenerateBarChartExample()
        {
            yield return new ExampleChartItem()
            {
                ExampleName = "Default",
                ExampleTextDescription = "Default example",
                Chart = new LegacyBarChart
                {
                    Entries = GenerateDefaultXamarinEntries(),
                    LabelTextSize = 42,
                    LabelOrientation = Orientation.Horizontal
                }
            };

            yield return new ExampleChartItem()
            {
                ExampleName = "Show Y axis at right",
                ExampleTextDescription = "Display Y axis lines and values",
                Chart = new LegacyBarChart
                {
                    Entries = GenerateDefaultXamarinEntries(),
                    LabelTextSize = 42,
                    ShowYAxisLines = true,
                    ShowYAxisText = true,
                    LabelOrientation = Orientation.Horizontal,
                    ValueLabelOrientation = Orientation.Horizontal,
                    YAxisPosition = Position.Right
                }
            };

            yield return new ExampleChartItem()
            {
                ExampleName = "Show Y axis at left",
                ExampleTextDescription = "Display Y axis lines and values",
                Chart = new LegacyBarChart
                {
                    Entries = GenerateDefaultXamarinEntries(),
                    LabelTextSize = 42,
                    ShowYAxisLines = true,
                    ShowYAxisText = true,
                    LabelOrientation = Orientation.Horizontal,
                    YAxisPosition = Position.Left
                }
            };

            yield break;
        }

        private static IEnumerable<ChartEntry> GenerateSeriesEntry(Random r, int labelNumber = 3, bool withLabel = true)
        {
            List<ChartEntry> entries = new List<ChartEntry>();

            int label = 2020 - ((labelNumber - 1) * 5);
            var value = r.Next(0, 700);
            do
            {
                entries.Add(new ChartEntry(value) { ValueLabel = value.ToString(), Label = withLabel ? label.ToString() : null });
                value = r.Next(0, 700);
                label += 5;
            }
            while (label <= 2020);

            return entries;
        }

        public static Chart[] CreateQuickstart()
        {
            var entries = new[]
            {
                new ChartEntry(200)
                {
                        Label = "Week 1",
                        ValueLabel = "200",
                        Color = SKColor.Parse("#266489")
                },
                new ChartEntry(400)
                {
                        Label = "Week 2",
                        ValueLabel = "400",
                        Color = SKColor.Parse("#68B9C0")
                },
                new ChartEntry(100)
                {
                        Label = "Week 3",
                        ValueLabel = "100",
                        Color = SKColor.Parse("#90D585")
                },
                new ChartEntry(600)
                {
                    Label = "Week 4",
                    ValueLabel = "600",
                    Color = SKColor.Parse("#32a852")
                },
                new ChartEntry(600)
                {
                    Label = "Week 5",
                    ValueLabel = "1600",
                    Color = SKColor.Parse("#8EC0D8")
                }
            };

            return new Chart[]
            {
                new LegacyBarChart
                {
                    Entries = entries,
                    LabelTextSize = 55,
                    LabelOrientation = Orientation.Horizontal,
                    Margin = 10
                },
                new LegacyPointChart
                {
                    Entries = entries,
                    LabelTextSize = 55,
                    LabelOrientation = Orientation.Horizontal,
                    Margin = 10
                },
                new LegacyLineChart
                {
                    Entries = entries,
                    LabelTextSize = 55,
                    LabelOrientation = Orientation.Horizontal,
                    Margin = 10
                },
                new DonutChart
                {
                    Entries = entries,
                    LabelTextSize = 60
                },
                new RadialGaugeChart
                {
                    Entries = entries,
                    LabelTextSize = 60
                },
                new RadarChart
                {
                    Entries = entries,
                    LabelTextSize = 60
                }
            };
        }

        public static ChartEntry[] CreateEntries(int values, bool hasPositiveValues, bool hasNegativeValues, bool hasLabels, bool hasValueLabel, bool isSingleColor)
        {
            ColorIndex = 0;

            (string label, int value)[] data;

            if (hasPositiveValues && hasNegativeValues)
            {
                data = MixedData;
            }
            else if (hasPositiveValues)
            {
                data = PositiveData;
            }
            else if (hasNegativeValues)
            {
                data = NegativeData;
            }
            else
            {
                data = new (string label, int value)[0];
            }

            data = data.Take(values).ToArray();

            return data.Select(d => new ChartEntry(d.value)
            {
                Label = hasLabels ? d.label : null,
                ValueLabel = hasValueLabel ? d.value.ToString() : null,
                TextColor = TextColor,
                Color = isSingleColor ? Colors[2] : NextColor(),
            }).ToArray();
        }
    }
}
