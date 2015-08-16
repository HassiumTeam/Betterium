using System;
using Codeaddicts.libArgument.Attributes;

namespace Hassium
{
	public class Options
	{
		[Argument ("/i", "/input", "-i", "--input")]
		[Docs ("Input file.")]
		public string input;

		[Switch ("/wall", "-wall")]
		[Docs ("Print all debug messages.")]
		public bool w_all;

		[Switch ("/werr", "-werr")]
		[Docs ("Print exceptions.")]
		public bool w_error;

		[Switch ("/wwarn", "-wwarn")]
		[Docs ("Print warnings.")]
		public bool w_warn;

		[Switch ("/wtime", "-wtime")]
		[Docs ("Print time measurements.")]
		public bool w_time;

		[Switch ("/wdebug", "-wdebug")]
		[Docs ("Print debug information.")]
		public bool w_debug;
	}
}

