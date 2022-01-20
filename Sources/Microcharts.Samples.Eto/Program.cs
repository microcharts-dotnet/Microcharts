using System;
using Eto.Forms;

namespace Microcharts.Samples.Eto
{
	static class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			new Application(global::Eto.Platform.Detect).Run(new MainForm());
		}
	}
}
