using System;

namespace Hassium
{
    public enum UnaryOperation
    {
        Not
    }

    //Class for the urinary operations
    public class UnaryOpNode: OldAstNode
    {
        public UnaryOperation UnOp { get; set; }

        public OldAstNode Value
        {
            get
            {
                return this.Children[0];
            }
        }

        public UnaryOpNode(UnaryOperation type, OldAstNode value)
        {
            this.UnOp = type;
            this.Children.Add(value);
        }
    }
}

