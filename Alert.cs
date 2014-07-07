#if IPHONE
using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Threading.Tasks;

namespace Xamarin.Helpers {
	public static class Alert {
		static readonly NSObject UIThread = new NSObject();

		static UIAlertView AlertView { get; set; }

		public static Task<string> Input(string title, string message, string button = "Ok", UIKeyboardType type = UIKeyboardType.ASCIICapable, string defValue = "") {
			var tcs = new TaskCompletionSource<string>();
			var alert = new UIAlertView(title, message, null, button, null);
			alert.AlertViewStyle = UIAlertViewStyle.PlainTextInput;

			var txt = alert.GetTextField(0);
			txt.Text = defValue;
			txt.KeyboardType = type;

			alert.Clicked += (sender, e) => tcs.SetResult(alert.GetTextField(0).Text);

			UIThread.InvokeOnMainThread(alert.Show);

			return tcs.Task;
		}

		public static void Show(string title, string message, string button = "Ok", Action yesFunc = null) {
			UIThread.InvokeOnMainThread(() => {
				var alert = new UIAlertView(title, message, null, button);

				if (yesFunc != null) {
					alert.Clicked += (sender, e) => { 
						if (e.ButtonIndex == 0)
							yesFunc(); 
					};
				}

				alert.Show();
			});
		}

		public static void Confirm(string title, string message, string yesButton = "Yes", string noButton = "No", Action yesFunc = null, Action noFunc = null) {
			UIThread.InvokeOnMainThread(() => {
				if (AlertView != null)
					AlertView.Dispose();

				AlertView = new UIAlertView(title, message, null, yesButton, noButton);

				AlertView.Clicked += (sender, e) => { 
					if (e.ButtonIndex == 0 && yesFunc != null)
						yesFunc();
					else if (e.ButtonIndex == 1 && noFunc != null)
						noFunc(); 
				};

				AlertView.Show();
			});
		}
	}
}
#else
using System;
using Android.Widget;
using System.Threading.Tasks;
using Android.Text;
using Android.App;
using Android.Views;

namespace Xamarin.Helpers {
	public static class Alert {
		public static EditText InputText { get; set; }
		public static AlertDialog LastAlert { get; set; }
		public static Activity UIThread { get; set; }
		public static int ThemeId { get; set; }

		public static Task<string> Input(string title, string message, string button, InputTypes types = InputTypes.ClassText) {
			var tcs = new TaskCompletionSource<string>();

			InputText = new EditText(UIThread) {
				InputType = types
			};

			UIThread.RunOnUiThread(() => {
				var alert = new AlertDialog.Builder(new ContextThemeWrapper(UIThread, ThemeId));

				alert.SetTitle(title);
				alert.SetMessage(message);
				alert.SetView(InputText);
				alert.SetCancelable(false);
				alert.SetPositiveButton(button, (s, e) => tcs.SetResult(InputText.Text));
				alert.Show();
			});

			return tcs.Task;
		}

		public static Task<bool> Confirm(string title, string message, string yesButton = "Yes", string noButton = "No", Action yesFunc = null, Action noFunc = null) {
			var tcs = new TaskCompletionSource<bool>();

			UIThread.RunOnUiThread(() => {
				var alert = new AlertDialog.Builder(new ContextThemeWrapper(UIThread, ThemeId));

				alert.SetTitle(title);
				alert.SetMessage(message);
				alert.SetCancelable(false);
				alert.SetPositiveButton(yesButton, (s, e) => {
					tcs.SetResult(true);

					if (yesFunc != null)
						yesFunc();
				});
				alert.SetNegativeButton(noButton, (s, e) => {
					tcs.SetResult(false);

					if (noFunc != null)
						noFunc();
				});
				alert.Show();
			});

			return tcs.Task;
		}

		public static Task<bool> Show(string title, string message, string button = "Ok") {
			var tcs = new TaskCompletionSource<bool>();

			UIThread.RunOnUiThread(() => {
				var alert = new AlertDialog.Builder(new ContextThemeWrapper(UIThread, ThemeId));

				if (!string.IsNullOrEmpty(title))
					alert.SetTitle(title);

				alert.SetMessage(message);
				alert.SetCancelable(false);
				alert.SetPositiveButton(button, (s, e) => tcs.SetResult(true));

				alert.Show();
			});

			return tcs.Task;
		}
	}
}
#endif
