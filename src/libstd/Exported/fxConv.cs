using System;

namespace libstd
{
	public static class fxConv {
		public static object tostr (object[] args) {
			var op = args [0];
			return op.ToString ();
		}

		public static object tonum (object[] args) {
			var op = args [0];
			return Convert.ToDecimal (op);
		}
	}
}

