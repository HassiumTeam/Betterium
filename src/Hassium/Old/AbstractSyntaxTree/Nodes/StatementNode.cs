using System;

namespace Hassium
{
    public class StatementNode: OldAstNode
    {
        public static OldAstNode Parse(OldParser parser)
        {
            if (parser.MatchToken(OldTokenType.Identifier, "if"))
            {
                return IfNode.Parse(parser);
            }
            else if (parser.MatchToken(OldTokenType.Identifier, "while"))
            {
                return WhileNode.Parse(parser);
            }
            else if (parser.MatchToken(OldTokenType.Bracket, "{"))
            {
                return CodeBlock.Parse(parser);
            }
            else
            {
                OldAstNode expr = ExpressionNode.Parse(parser);
                parser.ExpectToken(OldTokenType.EndOfLine);
                return expr;
            }
        }
    }
}

