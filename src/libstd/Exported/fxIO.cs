using System;
using System.Linq;

namespace libstd
{
	public static class fxIO {
		public static object print (params object[] args) {
			Console.WriteLine (args [0]);
			return null;
		}
	}
}

