using System;

namespace Hassium
{
    public class ArgListNode: OldAstNode
    {
        public static ArgListNode Parse(OldParser parser)
        {
            ArgListNode ret = new ArgListNode();
            parser.ExpectToken(OldTokenType.Parentheses, "(");

            while (!parser.MatchToken(OldTokenType.Parentheses, ")"))
            {
                ret.Children.Add(ExpressionNode.Parse(parser));
                if (!parser.AcceptToken(OldTokenType.Comma))
                {
                    break;
                }
            }
            parser.ExpectToken(OldTokenType.Parentheses, ")");

            return ret;
        }
    }
}

