using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Xamarin.Helpers {
	public static class CssParser {
		internal static Dictionary<string, Dictionary<string, string>> CssQueries { get; set; }

		public static void Clear() {
			if (CssQueries == null)
				CssQueries = new Dictionary<string, Dictionary<string, string>>();

			CssQueries.Clear();
		}

		public static string Get(string selector, string style, string def = "") {
			if (!CssQueries.ContainsKey(selector) || !CssQueries[selector].ContainsKey(style))
				return def;

			return CssQueries[selector][style];
		}

		public static void Load(string path) {
			Clear();

			var file = File.ReadAllText(path);
			var re = new Regex(@"(?<=\}\s*)(?<selector>[^\{\}]+?)(?:\s*\{(?<style>[^\{\}]+)\})", RegexOptions.Compiled);
			var styleRe = new Regex(@"\s*(?<style>[^:]+):\s*(?<value>[^;]+);", RegexOptions.Compiled);

			foreach (Match m in re.Matches(file)) {
				var selector = m.Groups["selector"].Value.Trim();
				var styles = m.Groups["style"].Value.Trim();

				CssQueries[selector] = new Dictionary<string, string>();

				foreach (Match sm in styleRe.Matches(styles)) {
					var style = sm.Groups["style"].Value.Trim();
					var val = sm.Groups["value"].Value.Trim();

					CssQueries[selector][style] = val;
				}
			}
		}
	}
}

