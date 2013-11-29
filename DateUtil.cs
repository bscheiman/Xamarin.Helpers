using System;

namespace iOS.Helpers {
	public static class DateUtil {
		public static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public static long Now {
			get { return (long)(DateTime.UtcNow - Epoch).TotalSeconds; }
		}
	}
}

