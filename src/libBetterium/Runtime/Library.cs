using System;

namespace Betterium
{
	public class Library {

		/// <summary>
		/// The exported functions.
		/// </summary>
		public Function[] ExportedFunctions;

		/// <summary>
		/// Initializes a new instance of the <see cref="Betterium.Library"/> class.
		/// </summary>
		/// <param name="exports">Exported functions.</param>
		public Library (Function[] exports) {
			ExportedFunctions = exports;
		}
	}
}

