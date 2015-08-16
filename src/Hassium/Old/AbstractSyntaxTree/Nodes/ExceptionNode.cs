using System;

namespace Hassium
{
    public class ExceptionNode: OldAstNode
    {
        public string Value { get; private set; }

        public ExceptionNode(string value)
        {
            this.Value = value;
        }
    }
}

