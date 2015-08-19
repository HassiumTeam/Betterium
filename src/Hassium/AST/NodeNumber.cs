using System;

namespace Hassium
{
	public class NodeNumber : AstNode
	{
		public Double Value;

		public NodeNumber (double val) : base ("Number") {
			Value = val;
		}
	}
}

