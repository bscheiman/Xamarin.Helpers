using System;

namespace iOS.Helpers {
	public static partial class Extensions {
		public static string HexString(this byte[] arr) {
			return BitConverter.ToString(arr).Replace("-", "");
		}
	}
}

