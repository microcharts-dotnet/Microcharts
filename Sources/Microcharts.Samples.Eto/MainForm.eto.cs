
namespace Microcharts.Samples.Eto
{
    using global::Eto.Forms;
    using global::Eto.Drawing;
    partial class MainForm : Form
    {
        void InitializeComponent()
        {
            Title = "My Eto Form";
            MinimumSize = new Size(200, 150);
            Size = MinimumSize * 4;
            Padding = 10;

            Content = new StackLayout
            {
                Items =
                {
                    "Hello World!",
					// add more controls here
				}
            };

            var quitCommand = new Command { MenuText = "Quit", Shortcut = Application.Instance.CommonModifier | Keys.Q };
            quitCommand.Executed += (sender, e) => Application.Instance.Quit();

            var aboutCommand = new Command { MenuText = "About..." };
            aboutCommand.Executed += (sender, e) => new AboutDialog().ShowDialog(this);

            // create menu
            Menu = new MenuBar
            {
                Items =
                {
					// File submenu0
//					new SubMenuItem { Text = "&File", Items = { clickMe } },
					// new SubMenuItem { Text = "&Edit", Items = { /* commands/items */ } },
					// new SubMenuItem { Text = "&View", Items = { /* commands/items */ } },
				},
                ApplicationItems =
                {
					// application (OS X) or file menu (others)
					new ButtonMenuItem { Text = "&Preferences..." },
                },
                QuitItem = quitCommand,
                AboutItem = aboutCommand
            };
        }
    }
}
