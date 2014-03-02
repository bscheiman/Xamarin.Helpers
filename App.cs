using System;
using MonoTouch.Foundation;

namespace iOS.Helpers {
	public static class App {
		public static string FullVersion {
			get {
				var full = NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleShortVersionString").ToString();
				var build = NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleVersion").ToString();

				return string.Format("{0}b{1}", full, build);
			}
		}
	}
}
