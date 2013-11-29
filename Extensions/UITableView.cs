using System;
using MonoTouch.UIKit;
using System.Drawing;

namespace iOS.Helpers {
	public static partial class Extensions {
		public static void Reload(this UITableView table) {
			table.ReloadData();
			table.ScrollRectToVisible(new RectangleF(0, 0, 1, 1), true);
			table.UserInteractionEnabled = true;
		}
	}
}

