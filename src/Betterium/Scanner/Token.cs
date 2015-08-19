using System;
using System.Linq;

namespace Betterium
{
	public class Token
	{
		public int Line;
		public int LinePos;

		public TokenType Type { get; private set; }
		public object Value { get; private set; }

		public Token (TokenType type, object value, int line, int linepos) {
			Line = line;
			LinePos = linepos;
			Type = type;
			Value = value;
		}

		public T UnboxAs<T> () {
			return (T)Value;
		}
	}
}

