using System;
using Betterium;

namespace libstd
{
	public class Entry : Library
	{
		public Entry () : base (new [] {
			// io
			Function.From ("print", fxIO.print),
			Function.From ("println", fxIO.println),
			// type conversion
			Function.From ("tostr", fxConv.tostr),
			Function.From ("tonum", fxConv.tonum),
			// string manipulation
			Function.From ("strcat", fxString.strcat),
			Function.From ("strlen", fxString.strlen),
			Function.From ("strat", fxString.strat),
			Function.From ("substr", fxString.substr),
			Function.From ("startswith", fxString.startswith),
			Function.From ("endswith", fxString.endswith),
			Function.From ("upper", fxString.upper),
			Function.From ("lower", fxString.lower),
			// math
			Function.From ("pow", fxMath.pow),
			Function.From ("sqrt", fxMath.sqrt),
			Function.From ("hash", fxMath.hash),
		}) { }
	}
}

