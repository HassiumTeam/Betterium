using System;

namespace Hassium
{
    public enum BinaryOperation
    {
        Assignment,
        Addition,
        Subtraction,
        Division,
        Multiplication,
        Equals,
        LessThan,
        GreaterThan,
        NotEqualTo
    }

    public class BinOpNode : OldAstNode
    {
        public BinaryOperation BinOp { get; set; }
        public OldAstNode Left
        {
            get 
            {
                return this.Children [0];
            }
        }

        public OldAstNode Right
        {
            get
            {
                return this.Children [1];
            }
        }

        public BinOpNode(BinaryOperation type, OldAstNode left, OldAstNode right)
        {
            this.BinOp = type;
            this.Children.Add(left);
            this.Children.Add(right);
        }
    }
}

