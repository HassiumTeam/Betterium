using System;

namespace Hassium
{
    public class IdentifierNode: OldAstNode
    {
        public string Identifier { get; private set; }

        public IdentifierNode(string value)
        {
            this.Identifier = value;
        }

        public override string ToString()
        {
            return Identifier;
        }
    }
}

