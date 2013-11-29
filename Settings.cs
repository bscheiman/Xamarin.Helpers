using System;
using System.Linq.Expressions;
using MonoTouch.Foundation;

namespace iOS.Helpers {
	public static class Settings {
		public static T Retrieve<T>(Expression<Func<ConfigEntry<T>>> expr) {
			var body = (MemberExpression) expr.Body;
			var name = body.Member.Name;
			var value = NSUserDefaults.StandardUserDefaults.StringForKey(name);

			if (value == null) {
				Save(expr, expr.Compile()().Default);

				return Retrieve(expr);
			}

			return (T) Convert.ChangeType(value, typeof (T));
		}

		public static void Save<T>(Expression<Func<ConfigEntry<T>>> expr, T value) {
			var body = (MemberExpression) expr.Body;
			var name = body.Member.Name;

			NSUserDefaults.StandardUserDefaults.SetString(value.ToString(), name);
			NSUserDefaults.StandardUserDefaults.Synchronize();
		}
	}

	public class ConfigEntry<T> {
		public T Default { get; set; }
	}
}

