using System;
using System.Threading.Tasks;
using MonoTouch.Foundation;

namespace iOS.Helpers {
	public static partial class Extensions {
		static NSObject UIThread = new NSObject();

		public static Task ContinueOnUIThread<T>(this Task<T> task, Action<Task<T>> action, TaskContinuationOptions tco) {
			return task.ContinueWith(t => UIThread.InvokeOnMainThread(() => action(t)), tco);
		}

		public static Task ContinueOnUIThread<T>(this Task<T> task, Action<Task<T>> action) {
			return task.ContinueWith(t => UIThread.InvokeOnMainThread(() => action(t)));
		}
	}
}

