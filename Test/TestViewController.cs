using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using iOS.Helpers;

namespace Test {
	public partial class TestViewController : UIViewController {
		public TestViewController() : base("TestViewController", null) {
		}

		public override void DidReceiveMemoryWarning() {
			base.DidReceiveMemoryWarning();
		}

		public override void ViewDidLoad() {
			base.ViewDidLoad();

			Console.WriteLine(Settings.Retrieve(() => ConfigDefaults.StringEntry));
			Alert.Show("iOS.Helpers", "Testing!");
			Alert.Show("Test", Settings.Retrieve(() => ConfigDefaults.BoolEntry).ToString());
		}
	}

	public static class ConfigDefaults {
		public static ConfigEntry<string> StringEntry = new ConfigEntry<string> {
			Default = "oh hai"
		};

		public static ConfigEntry<float> FloatEntry = new ConfigEntry<float> {
			Default = 2
		};

		public static ConfigEntry<bool> BoolEntry = new ConfigEntry<bool> {
			Default = true
		};
	}
}

