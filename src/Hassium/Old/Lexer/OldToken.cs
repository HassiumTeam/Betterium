using System;

namespace Hassium
{
    public enum OldTokenType
    {
		Identifier,
		Bracket,
		String,
		Number,
		Parentheses,
		Comma,
		Operation,
		Comparison,
		Store,
		Exception,
		EndOfLine,
		Not
    }

    public class OldToken
    {
        public OldTokenType TokenClass { get; private set; }
        public string Value { get; private set; }

        public OldToken(OldTokenType type, string value)
        {
            this.TokenClass = type;
            this.Value = value;
        }
    }
}

