using System;

namespace Hassium
{
	public class NodeUnOp : AstNode
	{
		public UnOp Type;
		public AstNode Value { get { return Children [0]; } }

		public NodeUnOp (UnOp type, AstNode val) {
			Type = type;
			AddChild (val);
		}
	}
}

