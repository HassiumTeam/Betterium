using System;

namespace Hassium
{
	public class NodeFuncCall : AstNode
	{
		public AstNode Target { get { return Children [0]; } }
		public AstNode Arguments { get { return Children [1]; } }

		public NodeFuncCall (AstNode left, AstNode args) : base ("Function Call") {
			AddChild (left);
			AddChild (args);
		}
	}
}

