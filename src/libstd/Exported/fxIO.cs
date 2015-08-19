using System;
using System.Linq;

namespace libstd
{
	public static class fxIO {
		public static object print (params object[] args) {
			Console.Write (args [0].ToString (), args);
			return null;
		}

		public static object println (object[] args) {
			Console.WriteLine (args [0].ToString (), args);
			return null;
		}
	}
}

