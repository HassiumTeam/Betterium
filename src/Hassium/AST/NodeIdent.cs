using System;

namespace Hassium
{
	public class NodeIdent : AstNode
	{
		public string Value;

		public NodeIdent (string val) : base ("Identifier") {
			Value = val;
		}
	}
}

