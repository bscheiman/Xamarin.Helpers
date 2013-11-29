using System;
using MonoTouch.UIKit;

namespace iOS.Helpers {
	public static partial class Extensions {
		public static UITextField AfterReturnSelect(this UITextField orig, UITextField dest) {
			orig.ShouldReturn = delegate {
				dest.BecomeFirstResponder();
				return true;
			};

			return dest;
		}

		public static UITextField End(this UITextField orig) {
			orig.ShouldReturn = delegate {
				orig.ResignFirstResponder();
				return true;
			};

			return orig;
		}

		public static UITextField ThenSelect(this UITextField orig, UITextField dest) {
			return AfterReturnSelect(orig, dest);
		}
	}
}

