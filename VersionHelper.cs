using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using MonoTouch.Foundation;
using ServiceStack.Text;

namespace iOS.Helpers {
	public static class VersionHelper {
		public static bool IsNewVersionAvailable(string appId) {
			try {
				using (var wc = new WebClient()) {
					var obj = wc.DownloadString(string.Format("http://itunes.apple.com/lookup?id={0}", appId)).FromJson<iTunesResponse>();
					var currVersion = Version.Parse(NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleVersion").ToString());
					var newVersion = Version.Parse(obj.Results[0].Version);

					return newVersion.CompareTo(currVersion) > 0;
				}
			} catch {
				return false;
			}
		}

		[DataContract]
		public class Result {
			[DataMember(Name = "version")]
			public string Version { get; set; }
		}

		[DataContract]
		public class iTunesResponse {
			[DataMember(Name = "results")]
			public List<Result> Results { get; set; }
		}
	}
}

