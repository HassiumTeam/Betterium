using System;

namespace Hassium
{
    public class CodeBlock: OldAstNode
    {
        public static OldAstNode Parse(OldParser parser)
        {
            CodeBlock block = new CodeBlock();
            parser.ExpectToken(OldTokenType.Bracket, "{");

            while (!parser.EndOfStream && !parser.MatchToken(OldTokenType.Bracket, "}"))
            {
                block.Children.Add(StatementNode.Parse(parser));
            }

            parser.ExpectToken(OldTokenType.Bracket, "}");
            return block;
        }
    }
}

