using System;

namespace Betterium
{
	public class NodeUnOp : AstNode
	{
		public UnOp Type;
		public AstNode Value { get { return Children [0]; } }

		public NodeUnOp (UnOp type, AstNode val) : base ("Unary Operation", NodeType.Operation) {
			Type = type;
			AddChild (val);
		}
	}
}

