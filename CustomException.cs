using System;

namespace Xamarin.Helpers {
	public class CustomException<T> : Exception where T : new() {
		public T CustomData { get; set; }

		public CustomException(string message, T data) : base(message) {
			CustomData = data;
		}
	}
}

