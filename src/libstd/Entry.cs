using System;
using Betterium;

namespace libstd
{
	public class Entry : Library
	{
		public Entry () : base (new [] {
			Function.From ("print", fxIO.print),
		}) { }
	}
}

