using System;
using System.Runtime.InteropServices;
using MonoTouch.UIKit;

namespace Xamarin.Helpers {
	public static class Device {
		public static bool IsIPad { get; private set; }

		public static bool IsRetina { get; private set; }

		public static bool IsSeven {
			get {
				return UIDevice.CurrentDevice.CheckSystemVersion(7, 0);
			}
		}

		public static bool IsTall {
			get {
				return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone && Math.Abs(UIScreen.MainScreen.Bounds.Size.Height - 568) < 0.01;
			}
		}

		public static iOSDevice RealDevice { get; private set; }

		static Device() {
			RealDevice = GetHardwareVersion();
			IsRetina = UIScreen.MainScreen.Scale > 1.0;
		}

		public static iOSDevice GetHardwareVersion() {
			const string hardwareProperty = "hw.machine";

			var pLen = Marshal.AllocHGlobal(sizeof(int));
			sysctlbyname(hardwareProperty, IntPtr.Zero, pLen, IntPtr.Zero, 0);

			var length = Marshal.ReadInt32(pLen);

			if (length == 0) {
				Marshal.FreeHGlobal(pLen);
				return iOSDevice.Unknown;
			}

			var pStr = Marshal.AllocHGlobal(length);
			sysctlbyname(hardwareProperty, pStr, pLen, IntPtr.Zero, 0);

			var hardwareStr = Marshal.PtrToStringAnsi(pStr);
			var ret = iOSDevice.Unknown;

			switch (hardwareStr) {
				case "iPhone1,1":
					ret = iOSDevice.iPhone2G;
					break;

				case "iPhone1,2":
					ret = iOSDevice.iPhone3G;
					break;

				case "iPhone2,1":
					ret = iOSDevice.iPhone3Gs;
					break;

				case "iPhone3,1":
					ret = iOSDevice.iPhone4;
					break;

				case "iPod1,1":
					ret = iOSDevice.iPod1G;
					break;

				case "iPod2,1":
					ret = iOSDevice.iPod2G;
					break;

				case "iPod3,1":
					ret = iOSDevice.iPod3G;
					break;

				case "iPad1,1":
					ret = iOSDevice.iPad1G;
					break;

				case "iPad2,1":
					ret = iOSDevice.iPad2G;
					break;

				case "iPad3,1":
					ret = iOSDevice.iPad3G;
					break;

				case "x86_32":
				case "x86_64":
				case "i386":
					ret = iOSDevice.Simulator;
					break;
			}

			Marshal.FreeHGlobal(pLen);
			Marshal.FreeHGlobal(pStr);

			return ret;
		}

		[DllImport(MonoTouch.Constants.SystemLibrary)]
		internal static extern int sysctlbyname([MarshalAs(UnmanagedType.LPStr)] string property, IntPtr output, IntPtr oldLen, IntPtr newp,
		                                        uint newlen);
	}

	public enum iOSDevice {
		iPhone2G,
		iPhone3G,
		iPhone3Gs,
		iPhone4,
		iPod1G,
		iPod2G,
		iPod3G,
		Simulator,
		iPad1G,
		iPad2G,
		iPad3G,
		Unknown
	}
}

