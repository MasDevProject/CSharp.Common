using System;
using MonoTouch.Dialog;
using UIKit;
using Foundation;

namespace MasDev.iOS.Views.Elements
{
	public class StepperElement : Element, IElementSizing
	{
		const string _additionalSpace = "   ";

		protected const string Identifier = "StepperElement";
		protected const float RowHeight = 44f;
		protected const float Padding = 8f;

		protected double MinValue = 1;
		protected double MaxValue = 20;

		public UIStepper Stepper { get ; private set; }

		protected double _value;
		public double Value
		{ 
			get { return _value; }
			set 
			{
				_value = value;
				UpdateValue(_value);
			}
		}

		public double MaximumValue
		{
			get { return MaxValue; }
			set
			{
				if (Stepper != null)
					Stepper.MaximumValue = value;

				MaxValue = value;
			}
		}

		public double MinimumValue
		{
			get { return MinValue; }
			set
			{
				if (Stepper != null)
					Stepper.MinimumValue = value;

				MinValue = value;
			}
		}

		public StepperElement(string caption, double value) : base(caption)
		{
			_value = value;
		}

		public override UITableViewCell GetCell (UITableView tv)
		{
			var stepperViewCell = tv.DequeueReusableCell (Identifier);

			if (stepperViewCell == null) {
				stepperViewCell = new UITableViewCell (UITableViewCellStyle.Value1, Identifier);
				stepperViewCell.SelectionStyle = UITableViewCellSelectionStyle.None;
			}

			if (Stepper == null)
			{
				Stepper = new UIStepper ();
				Stepper.MinimumValue = MinValue;
				Stepper.MaximumValue = MaxValue;

				Stepper.ValueChanged += (object sender, EventArgs e) => 
				{
					_value = Stepper.Value;
					stepperViewCell.DetailTextLabel.Text = _value + _additionalSpace;
				};
			}

			Stepper.Value = _value;

			stepperViewCell.TextLabel.Text = Caption;

			stepperViewCell.DetailTextLabel.Text = _value + _additionalSpace;
			stepperViewCell.DetailTextLabel.TextAlignment = UITextAlignment.Left;

			stepperViewCell.AccessoryView = Stepper;

			return stepperViewCell;
		}

		public nfloat GetHeight (UITableView tableView, NSIndexPath indexPath)
		{
			return RowHeight;
		}

		private void UpdateValue(double value)
		{
			if(Stepper != null)
				Stepper.Value = value;
		}
	}
}