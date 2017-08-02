using System;

using UIKit;
using Microcharts.iOS;
using System.Linq;

namespace Microcharts.Samples.iOS
{
	public partial class ViewController : UIViewController
	{
		protected ViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		private UIView CreateChartCard(Chart chart)
		{
			var view = new ChartView()
			{
				AutoresizingMask = UIViewAutoresizing.FlexibleWidth,
				Chart = chart,
			};
			view.HeightAnchor.ConstraintEqualTo(160).Active = true;
			view.Layer.CornerRadius = 5;
			stackView.AddArrangedSubview(view);

			return view;
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

			//this.PresentViewController(new QuickstartViewController(), true, () => { });
		}

		private void Reload(object sender, EventArgs e)
		{
			while (this.stackView.Subviews.Length > 1)
			{
				this.stackView.RemoveArrangedSubview(this.stackView.Subviews.Last());
				this.stackView.Subviews.Last().RemoveFromSuperview();
			}

			var hasPositiveValues = this.hasPositiveValuesSwitch.On;
			var hasNegativeValues = this.hasNegativeValuesSwitch.On;
			var hasSingleColor = this.hasSignleColorSwitch.On;
			var hasLabels = this.hasLabelsSwitch.On;
			var hasValueLabels = this.hasValueLabelsSwitch.On;
			var pointSize = pointSizeSlider.Value;
			var areaAlpha = (byte)(int)(255 * this.areaAlphaSlider.Value);
			var lineMode = this.hasSplinesSwitch.On ? LineMode.Spline : LineMode.Straight;

			var entries = Data.CreateEntries((int)this.valuesSlider.Value, hasPositiveValues, hasNegativeValues, hasLabels, hasValueLabels, hasSingleColor);

			CreateChartCard(new BarChart { Entries = entries, BarAreaAlpha = areaAlpha });
			CreateChartCard(new PointChart { Entries = entries, PointAreaAlpha = areaAlpha, PointSize = pointSize });
			CreateChartCard(new LineChart { Entries = entries, LineAreaAlpha = areaAlpha, PointSize = pointSize, LineSize = lineSizeSlider.Value, LineMode = lineMode });

			if(!hasSingleColor)
				CreateChartCard(new DonutChart { Entries = entries, HoleRadius = holeSlider.Value });
	
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			this.Reload(null,null);

			// Listeners
			this.hasPositiveValuesSwitch.ValueChanged += Reload;
			this.hasNegativeValuesSwitch.ValueChanged += Reload;
			this.hasSignleColorSwitch.ValueChanged += Reload;
			this.hasLabelsSwitch.ValueChanged += Reload;
			this.hasValueLabelsSwitch.ValueChanged += Reload;
			this.valuesSlider.ValueChanged += Reload;
			this.areaAlphaSlider.ValueChanged += Reload;
			this.holeSlider.ValueChanged += Reload;
			this.lineSizeSlider.ValueChanged += Reload;
			this.pointSizeSlider.ValueChanged += Reload;
			this.hasSplinesSwitch.ValueChanged += Reload;

			this.View.BackgroundColor = UIColor.FromWhiteAlpha(0.95f, 1);
		}
	}
}
