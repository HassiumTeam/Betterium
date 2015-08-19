using System;

namespace Betterium
{
	public class NodeString : AstNode
	{
		public string Value;

		public NodeString (string val) : base ("String", NodeType.String) {
			Value = val;
		}
	}
}

