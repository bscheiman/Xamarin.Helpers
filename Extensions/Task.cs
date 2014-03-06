using System;
using System.Threading.Tasks;
using MonoTouch.Foundation;

namespace Xamarin.Helpers {
	public static partial class Extensions {
		static readonly NSObject UIThread = new NSObject();
		public static Action<Exception> LogFunction { get; set; }

		public static Task ContinueOnUIThread<T>(this Task<T> task, Action<Task<T>> action, TaskContinuationOptions tco) {
			return task.ContinueWith(t => UIThread.InvokeOnMainThread(() => {
				try {
					action(t);
				} catch (Exception ex) {
					if (LogFunction == null)
						Alert.Show("Error", ex.ToString());
					else
						LogFunction(ex);

					#if DEBUG
					Alert.Show("Error", ex.ToString());
					#endif
				}
			}), tco);
		}

		public static Task ContinueOnUIThread(this Task task, Action<Task> action, TaskContinuationOptions tco) {
			return task.ContinueWith(t => UIThread.InvokeOnMainThread(() => {
				try {
					action(t);
				} catch (Exception ex) {
					if (LogFunction == null)
						Alert.Show("Error", ex.ToString());
					else
						LogFunction(ex);

					#if DEBUG
					Alert.Show("Error", ex.ToString());
					#endif
				}
			}), tco);
		}

		public static Task ContinueOnUIThread<T>(this Task<T> task, Action<Task<T>> action) {
			return task.ContinueWith(t => UIThread.InvokeOnMainThread(() => {
				try {
					action(t);
				} catch (Exception ex) {
					if (LogFunction == null)
						Alert.Show("Error", ex.ToString());
					else
						LogFunction(ex);

					#if DEBUG
					Alert.Show("Error", ex.ToString());
					#endif
				}
			}));
		}
	}
}

