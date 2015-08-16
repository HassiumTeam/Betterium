using System;
using System.Collections.Generic;

namespace Hassium
{
	public class AstNode
	{
		public List<AstNode> Children;

		public AstNode () {
			Children = new List<AstNode> ();
		}

		public void AddChild (AstNode node) {
			Children.Add (node);
		}
	}
}

