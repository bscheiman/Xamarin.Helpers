using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Threading.Tasks;

namespace iOS.Helpers {
	public static class Alert {
		static NSObject UIThread = new NSObject();
		private static UIAlertView AlertView { get; set; }

		public static Task<string> Input(string title, string message, string button = "Ok", UIKeyboardType type = UIKeyboardType.ASCIICapable, string defValue = "") {
			var tcs = new TaskCompletionSource<string>();
			AlertView = new UIAlertView(title, message, null, button, null);
			AlertView.AlertViewStyle = UIAlertViewStyle.PlainTextInput;

			var txt = AlertView.GetTextField(0);
			txt.Text = defValue;
			txt.KeyboardType = type;

			AlertView.Clicked += (sender, e) => tcs.SetResult(AlertView.GetTextField(0).Text);

			UIThread.InvokeOnMainThread(AlertView.Show);

			return tcs.Task;
		}


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

		public static void Confirm(string title, string message, string yesButton = "Yes", string noButton = "No", Action yesFunc = null) {
			UIThread.InvokeOnMainThread(() => {
				if (AlertView != null)
					AlertView.Dispose();

				AlertView = new UIAlertView(title, message, null, yesButton, noButton);

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

