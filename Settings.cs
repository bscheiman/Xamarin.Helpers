#if IPHONE
using System;
using System.Linq.Expressions;
using MonoTouch.Foundation;
using System.Globalization;

namespace Xamarin.Helpers {
	public static class Settings {
		public static T NativeGet<T>(string key) {
			try {
				var value = NSUserDefaults.StandardUserDefaults.StringForKey(key);

				return value != null ? (T)Convert.ChangeType(value, typeof(T)) : default(T);
			} catch {
				return default(T);
			}
		}

		public static void NativeSet<T>(string key, T value) {
			NSUserDefaults.StandardUserDefaults.SetString(value.ToString(), key);
			NSUserDefaults.StandardUserDefaults.Synchronize();
		}

		public static T Retrieve<T>(Expression<Func<ConfigEntry<T>>> expr) {
			var body = (MemberExpression)expr.Body;
			var name = body.Member.Name;
			var value = NSUserDefaults.StandardUserDefaults.StringForKey(name);

			if (value == null) {
				Save(expr, expr.Compile()().Default);

				return Retrieve(expr);
			}

			return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
		}

		public static void Save<T>(Expression<Func<ConfigEntry<T>>> expr, T value) {
			var body = (MemberExpression)expr.Body;
			var name = body.Member.Name;

			NSUserDefaults.StandardUserDefaults.SetString(value.ToString(), name);
			NSUserDefaults.StandardUserDefaults.Synchronize();
		}

		public static void Save(Expression<Func<ConfigEntry<float>>> expr, float value) {
			var body = (MemberExpression)expr.Body;
			var name = body.Member.Name;

			NSUserDefaults.StandardUserDefaults.SetString(value.ToString(CultureInfo.InvariantCulture), name);
			NSUserDefaults.StandardUserDefaults.Synchronize();
		}

		public static void Save(Expression<Func<ConfigEntry<double>>> expr, double value) {
			var body = (MemberExpression)expr.Body;
			var name = body.Member.Name;

			NSUserDefaults.StandardUserDefaults.SetString(value.ToString(CultureInfo.InvariantCulture), name);
			NSUserDefaults.StandardUserDefaults.Synchronize();
		}

		public static void Toggle(Expression<Func<ConfigEntry<bool>>> expr) {
			var val = Retrieve(expr);

			Save(expr, !val);
		}

		public static void Clear() {
			var dict = NSUserDefaults.StandardUserDefaults.AsDictionary();

			foreach (var key in dict)
				NSUserDefaults.StandardUserDefaults.RemoveObject(key.Key.ToString());

			NSUserDefaults.StandardUserDefaults.Synchronize();
		}
	}

	public class ConfigEntry<T> {
		public T Default { get; set; }
	}
}
#else
using System;
using System.Linq.Expressions;
using Android.Preferences;
using Android.Content;

namespace Xamarin.Helpers {
	public static class Settings {
		static Context AppContext { get; set; }

		public static void Init(Context ctx) {
			AppContext = ctx;
		}

		public static T NativeGet<T>(string key) {
			var value = PreferenceManager.GetDefaultSharedPreferences(AppContext).GetString(key, "");

			return value != null ? (T)Convert.ChangeType (value, typeof(T)) : default(T);
		}

		public static void NativeSet<T>(string key, T value) {
			var editor = PreferenceManager.GetDefaultSharedPreferences(AppContext).Edit();

			editor.PutString(key, value.ToString());
			editor.Commit();
		}

		public static T Retrieve<T>(Expression<Func<ConfigEntry<T>>> expr) {
			var body = (MemberExpression)expr.Body;
			var name = body.Member.Name;
			var value = PreferenceManager.GetDefaultSharedPreferences(AppContext).GetString(name, "");

			if (value == null) {
				Save(expr, expr.Compile()().Default);

				return Retrieve(expr);
			}

			try {
				return (T)Convert.ChangeType(value, typeof(T));
			} catch {
				return default(T);
			}
		}

		public static void Save<T>(Expression<Func<ConfigEntry<T>>> expr, T value) {
			var body = (MemberExpression)expr.Body;
			var name = body.Member.Name;

			var editor = PreferenceManager.GetDefaultSharedPreferences(AppContext).Edit();

			editor.PutString(name, value.ToString());
			editor.Commit();
		}

		public static void Toggle(Expression<Func<ConfigEntry<bool>>> expr) {
			var val = Retrieve(expr);

			Save(expr, !val);
		}
	}

	public class ConfigEntry<T> {
		public T Default { get; set; }
	}
}
#endif