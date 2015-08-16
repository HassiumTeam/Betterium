using System;

namespace Hassium
{
	public class NodeBinOp : AstNode
	{
		public BinOp Type;

		public AstNode Left { get { return Children [0]; } }
		public AstNode Right { get { return Children [1]; } }

		public NodeBinOp (BinOp type, AstNode left, AstNode right) {
			Type = type;
			AddChild (left);
			AddChild (right);
		}
	}
}

