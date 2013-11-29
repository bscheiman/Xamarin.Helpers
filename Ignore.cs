using System;

namespace iOS.Helpers {
	public static class Ignore {
		public static T Exception<T>(Func<T> act, T def) {
			try {
				return act();
			} catch {
				return def;
			}
		}
	}
}

