using System;
using MonoTouch.Foundation;
using System.Linq;

namespace iOS.Helpers {
	public static partial class Extensions {
		public static string ToHexString(this NSData data) {
			byte[] token = data.ToArray();
			string str = "";

			for (int i = 0; i < data.Length; i++)
				str += token[i].ToString("X2");

			return str;
		}
	}
}

