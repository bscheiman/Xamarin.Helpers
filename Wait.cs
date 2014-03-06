using System;
using System.Collections.Generic;
using System.Threading;
using MonoTouch.Foundation;

namespace Xamarin.Helpers {
	public class Wait {
		readonly object LockObj = new object();
		volatile bool Continue;
		List<Action> Funcs { get; set; }
		int Sleep { get; set; }
		static readonly NSObject UIThread = new NSObject();

		internal Wait() {
			Funcs = new List<Action>();
		}

		public void Abort() {
			lock (LockObj) {
				if (!Continue)
					return;

				Continue = false;
			}
		}

		public Wait Commit(bool sameThread = false) {
			Continue = true;

			Action act = () => {
				Thread.Sleep(Sleep);

				lock (LockObj) {
					if (!Continue)
						return;

					foreach (var f in Funcs)
						f();
				}
			};

			if (sameThread)
				act();
			else
				new Thread(() => UIThread.InvokeOnMainThread(() => act())).Start();

			return this;
		}

		public static Wait For(int ms) {
			if (ms < 0)
				throw new ArgumentOutOfRangeException();

			return new Wait {
				Sleep = ms
			};
		}

		public Wait Then(Action f) {
			if (f != null)
				Funcs.Add(f);

			return this;
		}
	}
}

