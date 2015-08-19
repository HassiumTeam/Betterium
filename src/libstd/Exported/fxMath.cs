using System;
using System.Text;
using System.Security.Cryptography;

namespace libstd
{
	public static class fxMath {
		public static object pow (object[] args) {
			var op1 = (decimal)args [0];
			var op2 = (decimal)args [1];
			return Math.Pow ((double)op1, (double)op2);
		}

		public static object sqrt (object[] args) {
			var op1 = (decimal)args [0];
			return Math.Sqrt ((double)op1);
		}

		public static object hash (object[] args) {
			var op1 = args [0].ToString ();
			var op2 = args [1].ToString ();
			var bytes = Encoding.UTF8.GetBytes (op2);
			var algorithm = ((HashAlgorithm)CryptoConfig.CreateFromName (op1.ToUpper ()));
			var computed = algorithm.ComputeHash (bytes);
			return BitConverter.ToString (computed).ToLowerInvariant ();
		}
	}
}

