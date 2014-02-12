using System;
using System.Threading.Tasks;
using MonoTouch.Foundation;

namespace iOS.Helpers {
	public static partial class Extensions {
		static readonly NSObject UIThread = new NSObject();

		public static Task ContinueOnUIThread<T>(this Task<T> task, Action<Task<T>> action, TaskContinuationOptions tco) {
			return task.ContinueWith(t => UIThread.InvokeOnMainThread(() => {
				try {
					action(t);
				} catch (Exception ex) {
					Alert.Show("Error", ex.ToString());
				}
			}), tco);
		}

		public static Task ContinueOnUIThread(this Task task, Action<Task> action, TaskContinuationOptions tco) {
			return task.ContinueWith(t => UIThread.InvokeOnMainThread(() => {
				try {
					action(t);
				} catch (Exception ex) {
					Alert.Show("Error", ex.ToString());
				}
			}), tco);
		}

		public static Task ContinueOnUIThread<T>(this Task<T> task, Action<Task<T>> action) {
			return task.ContinueWith(t => UIThread.InvokeOnMainThread(() => {
				try {
					action(t);
				} catch (Exception ex) {
					Alert.Show("Error", ex.ToString());
				}
			}));
		}
	}
}

