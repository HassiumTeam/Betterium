﻿using System;

namespace Hassium
{
	public class NodeIf : AstNode
	{
		public AstNode Predicate { get { return Children [0]; } }
		public AstNode Body { get { return Children [1]; } }
		public AstNode ElseBody { get { return Children [2]; } }

		public NodeIf (AstNode predicate, AstNode body, AstNode elseBody = null) {
			AddChild (predicate);
			AddChild (body);
			AddChild (elseBody ?? new AstNode ());
		}
	}
}

