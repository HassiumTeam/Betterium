using System;

namespace Hassium
{
    public class IfNode: OldAstNode
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

        public IfNode(OldAstNode predicate, OldAstNode body)
        {
         
            this.Children.Add(predicate);
            this.Children.Add(body);
            this.Children.Add(new CodeBlock());
        }

        public IfNode(OldAstNode predicate, OldAstNode body, OldAstNode elseBody)
        {
            this.Children.Add(predicate);
            this.Children.Add(body);
            this.Children.Add(elseBody);
        }

        public static OldAstNode Parse(OldParser parser)
        {
            parser.ExpectToken(OldTokenType.Identifier, "if");
            parser.ExpectToken(OldTokenType.Parentheses, "(");
            OldAstNode predicate = ExpressionNode.Parse(parser);
            parser.ExpectToken(OldTokenType.Parentheses, ")");
            OldAstNode ifBody = StatementNode.Parse(parser);
            if (parser.AcceptToken(OldTokenType.Identifier, "else"))
            {
                OldAstNode elseBody = StatementNode.Parse(parser);
                return new IfNode(predicate, ifBody, elseBody);
            }

            return new IfNode(predicate, ifBody);
        }
    }
}

