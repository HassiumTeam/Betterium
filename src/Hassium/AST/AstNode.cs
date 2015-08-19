using System;
using System.Collections.Generic;

namespace Hassium
{
	public class AstNode
	{
		public string Name = "AstNode";
		public List<AstNode> Children;

		public AstNode () {
			Children = new List<AstNode> ();
		}

		public AstNode (string name) : this () {
			Name = name;
		}

		public void AddChild (AstNode node) {
			Children.Add (node);
		}
	}
}

