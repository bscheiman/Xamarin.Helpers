using System;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using System.Drawing;

namespace iOS.Helpers {
	public static partial class Extensions {
		public static void Round(this UIImageView image, int size) {
			if (image == null)
				return;

			var layer = image.Layer;
			layer.CornerRadius = size;
			layer.BorderWidth = 0f;
			layer.BorderColor = UIColor.Clear.CGColor;
			layer.MasksToBounds = true;
		}

		public static UIImage Tint(this UIImage img, UIColor tint, CGBlendMode blendMode) {
			UIGraphics.BeginImageContextWithOptions(img.Size, false, 0f);
			tint.SetFill();
			var bounds = new RectangleF(0, 0, img.Size.Width, img.Size.Height);
			UIGraphics.RectFill(bounds);

			img.Draw(bounds, blendMode, 1f);

			if (blendMode != CGBlendMode.DestinationIn)
				img.Draw(bounds, CGBlendMode.DestinationIn, 1f);

			var tintedImage = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();

			return tintedImage;
		}

		public static UIImage RoundedCorners(this UIImage image, float radius) {
			if (image == null)
				throw new ArgumentNullException("image");

			UIImage converted = image;

			image.InvokeOnMainThread(() => {
				UIGraphics.BeginImageContext(image.Size);
				float imgWidth = image.Size.Width;
				float imgHeight = image.Size.Height;

				var c = UIGraphics.GetCurrentContext();

				c.BeginPath();
				c.MoveTo(imgWidth, imgHeight / 2);
				c.AddArcToPoint(imgWidth, imgHeight, imgWidth / 2, imgHeight, radius);
				c.AddArcToPoint(0, imgHeight, 0, imgHeight / 2, radius);
				c.AddArcToPoint(0, 0, imgWidth / 2, 0, radius);
				c.AddArcToPoint(imgWidth, 0, imgWidth, imgHeight / 2, radius);
				c.ClosePath();
				c.Clip();

				image.Draw(new PointF(0, 0));
				converted = UIGraphics.GetImageFromCurrentImageContext();
				UIGraphics.EndImageContext();
			});

			return converted;
		}
	}
}

