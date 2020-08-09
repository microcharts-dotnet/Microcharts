using System.Linq;
using SkiaSharp;
using Topten.RichTextKit;

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

        public static Chart[] CreateXamarinSample()
        {
            var entries = new[]
            {
                new ChartEntry(212)
                {
                    Label = "UWP",
                    ValueLabel = "112",
                    Color = SKColor.Parse("#2c3e50")
                },
                new ChartEntry(248)
                {
                    Label = "Android",
                    ValueLabel = "648",
                    Color = SKColor.Parse("#77d065")
                },
                new ChartEntry(128)
                {
                    Label = "iOS",
                    ValueLabel = "428",
                    Color = SKColor.Parse("#b455b6")
                },
                new ChartEntry(514)
                {
                    Label = "Forms",
                    ValueLabel = "214",
                    Color = SKColor.Parse("#3498db")
                }
            };

            return new Chart[]
            {
                new BarChart
                {
                    Entries = entries,
                    LabelTextSize = 42,
                    LabelOrientation = Orientation.Horizontal
                },
                new PointChart
                {
                    Entries = entries,
                    LabelTextSize = 42,
                    LabelOrientation = Orientation.Horizontal
                },
                new LineChart
                {
                    Entries = entries,
                    LineMode = LineMode.Straight,
                    LineSize = 8,
                    LabelTextSize = 42,
                    PointMode = PointMode.Square,
                    PointSize = 18,
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
                }
            };
        }

        public static Chart[] RightToLeftTest()
        {
            var entries = new[]
            {
                new ChartEntry(200)
                {
                    Label = "שבוע 1",
                    ValueLabel = "200",
                    Color = SKColor.Parse("#266489"),
                },
                new ChartEntry(400)
                {
                    Label = "שבוע 2",
                    ValueLabel = "400",
                    Color = SKColor.Parse("#68B9C0"),
                },
                new ChartEntry(100)
                {
                    Label = "שבוע 3",
                    ValueLabel = "100",
                    Color = SKColor.Parse("#90D585"),
                }
            };

            return new Chart[]
            {
                new BarChart
                {
                    Entries = entries,
                    LabelTextSize = 60,
                    LabelColor = SKColors.Red,
                    LabelOrientation = Orientation.Horizontal,
                    TextDirection = TextDirection.RTL
                },
                new PointChart
                {
                    Entries = entries,
                    LabelTextSize = 60,
                    LabelColor = SKColors.Red,
                    LabelOrientation = Orientation.Horizontal,
                    TextDirection = TextDirection.RTL
                },
                new LineChart
                {
                    Entries = entries,
                    LineMode = LineMode.Straight,
                    LineSize = 8,
                    LabelTextSize = 60,
                    LabelColor = SKColors.Red,
                    PointMode = PointMode.Square,
                    PointSize = 18,
                    TextDirection = TextDirection.RTL
                },
                new DonutChart
                {
                    Entries = entries,
                    LabelTextSize = 60,
                    GraphPosition = GraphPosition.Center,
                    LabelMode = LabelMode.RightOnly,
                    TextDirection = TextDirection.RTL
                },
                new RadialGaugeChart
                {
                    Entries = entries,
                    LabelTextSize = 60,
                    TextDirection = TextDirection.RTL
                },
                new RadarChart
                {
                    Entries = entries,
                    LabelTextSize = 60,
                    TextDirection = TextDirection.RTL
                }
            };
        }

        public static Chart[] CreateQuickstart()
        {
            var entries = new[]
            {
                new ChartEntry(200)
                {
                        Label = "Week 1",
                        ValueLabel = "200",
                        Color = SKColor.Parse("#266489"),
                },
                new ChartEntry(400)
                {
                        Label = "Week 2",
                        ValueLabel = "400",
                        Color = SKColor.Parse("#68B9C0"),
                },
                new ChartEntry(100)
                {
                        Label = "Week 3",
                        ValueLabel = "100",
                        Color = SKColor.Parse("#90D585"),
                },
                new ChartEntry(600)
                {
                    Label = "Week 4",
                    ValueLabel = "600",
                    Color = SKColor.Parse("#32a852"),
                }
            };

            return new Chart[]
            {
                new BarChart
                {
                    Entries = entries,
                    LabelTextSize = 55,
                    LabelOrientation = Orientation.Horizontal,
                    Margin = 10
                },
                new PointChart
                {
                    Entries = entries,
                    LabelTextSize = 55,
                    LabelOrientation = Orientation.Horizontal,
                    Margin = 10
                },
                new LineChart
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
