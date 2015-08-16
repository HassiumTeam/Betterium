using System;

namespace Hassium
{
    public class WhileNode: OldAstNode
    {
        public OldAstNode Predicate
        {
            get
            {
                return this.Children[0];
            }
        }
        public OldAstNode Body
        {
            get
            {
                return this.Children[1];
            }
        }
        public OldAstNode ElseBody
        {
            get
            {
                return this.Children[2];
            }
        }

        public WhileNode(OldAstNode predicate, OldAstNode body)
        {

            this.Children.Add(predicate);
            this.Children.Add(body);
            this.Children.Add(new CodeBlock());
        }

        public WhileNode(OldAstNode predicate, OldAstNode body, OldAstNode elseBody)
        {
            this.Children.Add(predicate);
            this.Children.Add(body);
            this.Children.Add(elseBody);
        }

        public static OldAstNode Parse(OldParser parser)
        {
            parser.ExpectToken(OldTokenType.Identifier, "while");
            parser.ExpectToken(OldTokenType.Parentheses, "(");
            OldAstNode predicate = ExpressionNode.Parse(parser);
            parser.ExpectToken(OldTokenType.Parentheses, ")");
            OldAstNode whileBody = StatementNode.Parse(parser);
            if (parser.AcceptToken(OldTokenType.Identifier, "else"))
            {
                OldAstNode elseBody = StatementNode.Parse(parser);
                return new WhileNode(predicate, whileBody, elseBody);
            }

            return new WhileNode(predicate, whileBody);
        }
    }
}

