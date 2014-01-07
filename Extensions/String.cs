using System;
using MonoTouch.UIKit;
using System.Security.Cryptography;
using System.Text;

namespace iOS.Helpers {
	public static partial class Extensions {
		public static string MD5Hash(this string str) {
			using (var hash = MD5.Create())
				return hash.ComputeHash(Encoding.UTF8.GetBytes(str)).HexString();
		}

		public static string SHA1Hash(this string str) {
			using (var hash = SHA1.Create())
				return hash.ComputeHash(Encoding.UTF8.GetBytes(str)).HexString();
		}

		public static UIColor AsColor(this string hex) {
			if (string.IsNullOrEmpty(hex))
				hex = "000000";

			hex = hex.Replace("#", string.Empty);

			if (hex.Length == 6)
				hex += "FF";

			int number;

			try {
				number = Convert.ToInt32(hex, 16);
			} catch {
				number = Convert.ToInt32("000000FF", 16);
			}

			float red = ((number >> 24) & 0xFF) / 255f;
			float green = ((number >> 16) & 0xFF) / 255f;
			float blue = ((number >> 8) & 0xFF) / 255f;
			float alpha = ((number >> 0) & 0xFF) / 255f;

			return UIColor.FromRGBA(red, green, blue, alpha);
		}

		public static UIImage AsImage(this string hex) {
			var color = hex.AsColor();

			return null; // TODO
		}
	}
}

