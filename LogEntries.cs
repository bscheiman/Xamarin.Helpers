using System;
using System.Net;
using System.Collections.Concurrent;
using System.Threading;
using System.IO;
using System.Net.Sockets;

namespace iOS.Helpers {
	public static class LogEntries {
		private static string TokenKey { get; set; }
		private static BlockingCollection<string> LogQueue { get; set; }

		public static void Init(string tokenKey) {
			TokenKey = tokenKey;
			LogQueue = new BlockingCollection<string>();

			new Thread(SendLogic).Start();
		}

		private	static string DeviceName {
			get {
				#if ANDROID
				return string.Format("{0} ({1})", Build.Model, Build.Serial);
				#else
				return MonoTouch.UIKit.UIDevice.CurrentDevice.Name;
				#endif
			}
		}

		private static void SendLogic() {
			try {
				using (var client = new TcpClient("data.logentries.com", 80)) {
					client.NoDelay = true;
					using (var stream = client.GetStream())
					using (var writer = new StreamWriter(stream)) {
						foreach (var str in LogQueue.GetConsumingEnumerable()) {
							writer.WriteLine(string.Format("{0} [{1}] {2}", TokenKey, DeviceName, str));
							writer.Flush();
						}
					}
				}
			} catch {
				Thread.Sleep(5000);
				SendLogic();
			}
		}

		public static void Stop() {
			LogQueue.CompleteAdding();
		}

		public static void Send(string str, params object[] obj) {
			if (string.IsNullOrEmpty(TokenKey))
				throw new ArgumentException("Please call Init() first.");

			var finalStr = string.Format(str, obj);

			LogQueue.Add(finalStr.Replace("\n", "\n" + TokenKey));
		}

		public static void Send(object obj) {
			Send(obj.ToString());
		}
	}
}

