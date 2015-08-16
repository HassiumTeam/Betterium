using System;

namespace Hassium
{
    public class NumberNode: OldAstNode
    {
        public double Value { private set; get; }

        public NumberNode (double value)
        {
            this.Value = value;
        }
    }
}

