#if IPHONE
using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Threading.Tasks;

namespace Xamarin.Helpers {
	public static class Sheet {
		static readonly NSObject UIThread = new NSObject();

		public static Task<int> Show(UIView view, string title, string cancel, string destroy, string[] options) {
			var tcs = new TaskCompletionSource<int>();
			var sheet = new UIActionSheet(title, null, cancel, destroy, options);

			sheet.Clicked += (sender, e) => {
				if (e.ButtonIndex == sheet.CancelButtonIndex)
					tcs.SetResult(-1);
				else if (e.ButtonIndex == sheet.DestructiveButtonIndex)
					tcs.SetResult(-2);
				else
					tcs.SetResult(e.ButtonIndex - 1);
			};

			UIThread.InvokeOnMainThread(() => sheet.ShowInView(view));
			return tcs.Task;
		}
	}
	
}
#endif
