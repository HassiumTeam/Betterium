using System;
using System.Collections.Generic;

namespace Betterium
{
	public class AstNode
	{
		public string Name;
		public NodeType Type;
		public List<AstNode> Children;

		public AstNode () {
			Name = "AstNode";
			Type = NodeType.Codeblock;
			Children = new List<AstNode> ();
		}

		public AstNode (string name) : this () {
			Name = name;
		}

		public AstNode (NodeType type) : this () {
			Type = type;
		}

		public AstNode (string name, NodeType type) : this () {
			Name = name;
			Type = type;
		}

		public void AddChild (AstNode node) {
			Children.Add (node);
		}
	}
}

