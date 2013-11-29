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
	}
}

