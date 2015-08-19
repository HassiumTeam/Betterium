using System;
using Codeaddicts.libArgument.Attributes;

namespace Betterium
{
	public class Options
	{
		[Argument ("-i", "--input")]
		[Docs ("Input file.")]
		public string input;

		[Switch ("-wall")]
		[Docs ("Print all debug messages.")]
		public bool w_all;

		[Switch ("-werr")]
		[Docs ("Print exceptions.")]
		public bool w_error;

		[Switch ("-wwarn")]
		[Docs ("Print warnings.")]
		public bool w_warn;

		[Switch ("-wdebug")]
		[Docs ("Print debug information.")]
		public bool w_debug;

		[Switch ("--time")]
		[Docs ("Print time measurements.")]
		public bool time;
	}
}

