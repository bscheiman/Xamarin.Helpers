using System;
using Android.App;

namespace Xamarin.Helpers {
	public static partial class Extensions {
		public static void InvokeOnMainThread(this Activity act, Action action) {
			act.RunOnUiThread(action);
		}
	}
}

