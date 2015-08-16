using System;
using System.Collections.Generic;

namespace Hassium
{
    public class OldParser
    {
        private List<OldToken> tokens = new List<OldToken>();
        private int position = 0;

        public bool EndOfStream
        {
            get
            {
                return this.tokens.Count <= position;
            }
        }

        public OldParser(List<OldToken> tokens)
        {
            this.tokens = tokens;
        }

        public OldAstNode Parse()
        {
            CodeBlock block = new CodeBlock();
            while (!EndOfStream)
            {
                block.Children.Add(StatementNode.Parse(this));
            }
            return block;
        }

        public OldToken CurrentToken()
        {
            return tokens[position];
        }

        public bool MatchToken(OldTokenType clazz)
        {
            return position < tokens.Count && tokens[position].TokenClass == clazz;
        }

        public bool MatchToken(OldTokenType clazz, string value)
        {
            return position < tokens.Count && tokens[position].TokenClass == clazz && tokens[position].Value == value;
        }

        public bool AcceptToken(OldTokenType clazz)
        {
            if (MatchToken(clazz))
            {
                position++;
                return true;
            }

            return false;
        }

        public bool AcceptToken(OldTokenType clazz, string value)
        {
            if (MatchToken(clazz, value))
            {
                position++;
                return true;
            }

            return false;
        }

        public OldToken ExpectToken(OldTokenType clazz)
        {
            if (!MatchToken(clazz))
            {
                return new OldToken(OldTokenType.Exception, "Tokens did not match");
            }

            return tokens[position++];
        }

        public OldToken ExpectToken(OldTokenType clazz, string value)
        {
            if (!MatchToken(clazz, value))
            {
                return new OldToken(OldTokenType.Exception, "Tokens did not match");
            }

            return tokens[position++];
        }



    }
}

