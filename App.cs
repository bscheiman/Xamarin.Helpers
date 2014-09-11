#if __IOS__
using MonoTouch.Foundation;

namespace Xamarin.Helpers {
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


#elif __ANDROID__
using Android.Content;

namespace Xamarin.Helpers {
	public static class App {
		public static Context AppContext { get; set; }

		public static string FullVersion {
			get {
				var pkgInfo = AppContext.PackageManager.GetPackageInfo(AppContext.PackageName, (Android.Content.PM.PackageInfoFlags)0);

				return string.Format("{0} ({1})", pkgInfo.VersionName, pkgInfo.VersionCode);
			}
		}
	}
}
#endif