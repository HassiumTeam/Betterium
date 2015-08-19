using System;

namespace libstd
{
	public static class fxString {
		public static object strcat (object[] args) {
			var str1 = (string)args [0];
			var str2 = (string)args [1];
			return str1 + str2;
		}

		public static object strlen (object[] args) {
			var str = (string)args [0];
			return str.Length;
		}

		public static object strat (object[] args) {
			var str = (string)args [0];
			var pos = (decimal)args [1];
			return str [(int)pos].ToString ();
		}

		public static object substr (object[] args) {
			var str = (string)args [0];
			var start = (decimal)args [1];
			if (args.Length == 3) {
				var len = (decimal)args [2];
				return str.Substring ((int)start, (int)len);
			}
			return str.Substring ((int)start);
		}

		public static object startswith (object[] args) {
			var str1 = (string)args [0];
			var str2 = (string)args [1];
			return str1.StartsWith (str2);
		}

		public static object endswith (object[] args) {
			var str1 = (string)args [0];
			var str2 = (string)args [1];
			return str1.EndsWith (str2);
		}

		public static object upper (object[] args) {
			var str = (string)args [0];
			return str.ToUpperInvariant ();
		}

		public static object lower (object[] args) {
			var str = (string)args [0];
			return str.ToLowerInvariant ();
		}
	}
}

