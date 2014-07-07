using System;
using System.Threading;
using System.Net;
using ServiceStack.Text;
using System.Text;

namespace Xamarin.Helpers {
	public static class ParseHelper {
		public static void RegisterToken(string appId, string restKey, string deviceToken, string deviceType = "ios", int badge = 0) {
			new Thread(() => {
				try {
					using (var wc = new WebClient()) {
						wc.Encoding = Encoding.UTF8;
						wc.Headers[HttpRequestHeader.ContentType] = "application/json";

						wc.Headers["X-Parse-Application-Id"] = appId;
						wc.Headers["X-Parse-REST-API-Key"] = restKey;
						wc.Headers["Content-Type"] = "application/json";

						var response = wc.UploadString("https://api.parse.com/1/installations", "POST", new {
							deviceType,
							deviceToken,
							badge,
							appVersion = App.FullVersion
						}.ToJson());

					}
				} catch {
				}
			}).Start();
		}
	}
}

