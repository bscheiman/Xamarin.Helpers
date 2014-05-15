using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace Xamarin.Helpers {
	public static class ApplicationHelper {
		public static void ResetNotifications(bool cancel = false) {
			UIApplication.SharedApplication.ApplicationIconBadgeNumber = 1;
			UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;

			if (cancel)
				UIApplication.SharedApplication.CancelAllLocalNotifications();
		}

		public static void ScheduleLocalNotification(string body) {
			UIApplication.SharedApplication.ScheduleLocalNotification(new UILocalNotification {
				FireDate = DateTime.Now.AddMinutes(-1),
				TimeZone = NSTimeZone.LocalTimeZone,
				AlertBody = body,
				RepeatInterval = 0
			});
		}

		public static void PostNotification(string name, object obj = null) {
			if (obj == null)
				NSNotificationCenter.DefaultCenter.PostNotificationName(name, null);
			else
				NSNotificationCenter.DefaultCenter.PostNotificationName(name, NSObject.FromObject(obj));
		}
	}
}

