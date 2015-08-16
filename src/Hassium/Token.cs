using System;
using System.Linq;

namespace Hassium
{
	public class Token
	{
		public static ManagedTokenType[] Lookup = {
			ManagedTokenType.New<String> (TokenType.String),
			ManagedTokenType.New<Double> (TokenType.Number),
		};

		public TokenType Type { get; private set; }
		public object Value { get; private set; }

		public Token (TokenType type, object value) {
			Type = type;
			Value = value;
		}

		public T UnboxAs<T> () {
			return (T)Value;
		}
	}
}

