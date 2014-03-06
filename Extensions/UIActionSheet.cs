using MonoTouch.UIKit;

namespace Xamarin.Helpers {
	public static partial class Extensions {
		public static UIActionSheet Tint(this UIActionSheet sheet, UIColor color) {
			sheet.WillPresent += (sender, e) => {
				foreach (var view in sheet.Subviews) {
					if (view is UIButton) {
						var btn = (UIButton)view;

						btn.SetTitleColor(color, UIControlState.Normal);
						btn.SetTitleColor(color, UIControlState.Highlighted);
						btn.SetTitleColor(color, UIControlState.Selected);
					}
				}
			};

			return sheet;
		}

		public static UIActionSheet Tint(this UIActionSheet sheet, string color) {
			return sheet.Tint(color.AsColor());
		}
	}
}

