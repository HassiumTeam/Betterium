using System;

namespace Hassium
{
    public class StringNode: OldAstNode
    {
        public string Value { get; private set; }

        public StringNode(string value)
        {
            this.Value = value;
        }
    }
}

