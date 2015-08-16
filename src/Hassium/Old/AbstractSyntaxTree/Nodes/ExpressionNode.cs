using System;

namespace Hassium
{
    public static class ExpressionNode
    {
        public static OldAstNode Parse(OldParser parser)
        {
            return ParseAssignment(parser);
        }


        private static OldAstNode ParseAssignment (OldParser parser)
        {
            OldAstNode left = ParseEquality(parser);

            if (parser.AcceptToken(OldTokenType.Store))
            {
                OldAstNode right = ParseAssignment(parser);
                return new BinOpNode(BinaryOperation.Assignment, left, right);
            }
            else
            {
                return left;
            }
        }

        private static OldAstNode ParseEquality (OldParser parser)
        {
            OldAstNode left = ParseAdditive(parser);
            if (parser.AcceptToken(OldTokenType.Comparison, "="))
            {
                OldAstNode right = ParseEquality(parser);
                return new BinOpNode(BinaryOperation.Equals, left, right);
            }
            else if (parser.AcceptToken(OldTokenType.Comparison, "!="))
            {
                OldAstNode right = ParseEquality(parser);
                return new BinOpNode(BinaryOperation.NotEqualTo, left, right);
            }
            else if (parser.AcceptToken(OldTokenType.Comparison, "<"))
            {
                OldAstNode right = ParseEquality(parser);
                return new BinOpNode(BinaryOperation.LessThan, left, right);
            }
            else if (parser.AcceptToken(OldTokenType.Comparison, ">"))
            {
                OldAstNode right = ParseEquality(parser);
                return new BinOpNode(BinaryOperation.GreaterThan, left, right);
            }
            else
            {
                return left;
            }
        }

        private static OldAstNode ParseAdditive (OldParser parser)
        {
            OldAstNode left = ParseMultiplicative(parser);

            if (parser.AcceptToken(OldTokenType.Operation, "+"))
            {
                OldAstNode right = ParseAdditive(parser);
                return new BinOpNode(BinaryOperation.Addition, left, right);
            }
            else if (parser.AcceptToken(OldTokenType.Operation, "-"))
            {
                OldAstNode right = ParseAdditive(parser);
                return new BinOpNode(BinaryOperation.Subtraction, left, right);
            }
            else
            {
                return left;
            }
        }

        private static OldAstNode ParseMultiplicative (OldParser parser)
        {
            OldAstNode left = ParseUnary(parser);
            
            if (parser.AcceptToken(OldTokenType.Operation, "*"))
            {
                OldAstNode right = ParseMultiplicative(parser);
                return new BinOpNode(BinaryOperation.Multiplication, left, right);
            }
            else if (parser.AcceptToken(OldTokenType.Operation, "/"))
            {
                OldAstNode right = ParseMultiplicative(parser);
                return new BinOpNode(BinaryOperation.Division, left, right);
            }
            else
            {
                return left;
            }
        }

        private static OldAstNode ParseUnary(OldParser parser)
        {
            if (parser.AcceptToken(OldTokenType.Not, "!"))
            {
                return new UnaryOpNode(UnaryOperation.Not, ParseUnary(parser));
            }
            else
            {
                return ParseFunctionCall(parser);
            }
        }

        private static OldAstNode ParseFunctionCall(OldParser parser)
        {
            return ParseFunctionCall(parser, ParseTerm(parser));
        }
        private static OldAstNode ParseFunctionCall(OldParser parser, OldAstNode left)
        {
            if (parser.AcceptToken(OldTokenType.Parentheses, "("))
            {
                return ParseFunctionCall(parser, new FunctionCallNode(left, ArgListNode.Parse(parser)));
            }
            else
            {
                return left;
            }
        }

        private static OldAstNode ParseTerm (OldParser parser)
        {
            if (parser.MatchToken(OldTokenType.Number))
            {
                return new NumberNode(Convert.ToDouble(parser.ExpectToken(OldTokenType.Number).Value));
            }
            else if (parser.AcceptToken(OldTokenType.Parentheses, "("))
            {
                OldAstNode statement = ExpressionNode.Parse(parser);
                parser.ExpectToken(OldTokenType.Parentheses, ")");
                return statement;
            }
            else if (parser.MatchToken(OldTokenType.String))
            {
                return new StringNode(parser.ExpectToken(OldTokenType.String).Value);
            }
            else if (parser.MatchToken(OldTokenType.Identifier))
            {
                return new IdentifierNode(parser.ExpectToken(OldTokenType.Identifier).Value);
            }
            else
            {
                throw new Exception("Unexpected in Parser: " + parser.CurrentToken().Value);
            }

        }
    }
}

