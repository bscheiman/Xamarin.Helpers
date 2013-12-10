using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace iOS.Helpers {
	public static class Alert {
		static NSObject UIThread = new NSObject();
		private static UIAlertView AlertView { get; set; }

		public static void Show(string title, string message, string button = "Ok", Action yesFunc = null) {
			UIThread.InvokeOnMainThread(() => {
				if (AlertView != null)
					AlertView.Dispose();

				AlertView = new UIAlertView(title, message, null, button);

				if (yesFunc != null) {
					AlertView.Clicked += (sender, e) => { 
						if (e.ButtonIndex == 0)
							yesFunc(); 
					};
				}

				AlertView.Show();
			});
		}
	}
}

