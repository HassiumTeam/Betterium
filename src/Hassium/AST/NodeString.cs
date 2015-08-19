using System;

namespace Hassium
{
	public class NodeString : AstNode
	{
		public string Value;

		public NodeString (string val) : base ("String") {
			Value = val;
		}
	}
}

