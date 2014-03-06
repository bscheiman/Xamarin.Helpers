using System;

namespace Xamarin.Helpers {
	public static class Ignore {
		public static T Exception<T>(Func<T> act, T def) {
			try {
				return act();
			} catch {
				return def;
			}
		}
		
		public static void Exception(Action act) {
			try {
				act();
			} catch {
			}
		}
	}
}

