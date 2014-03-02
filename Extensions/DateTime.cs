using System;

namespace iOS.Helpers {
	public static partial class Extensions {
		public static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public static long ToEpoch(this DateTime dt) {
			return (long)(dt.ToUniversalTime() - Epoch).TotalSeconds;
		}
	}
}

