using System;

namespace Betterium
{
	public class NodeNumber : AstNode
	{
		public Decimal Value;

		public NodeNumber (decimal val) : base ("Number", NodeType.Number) {
			Value = val;
		}
	}
}

