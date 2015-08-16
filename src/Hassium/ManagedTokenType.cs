using System;

namespace Hassium
{
	public class ManagedTokenType
	{
		public TokenType Type;
		public Type RawType;

		public bool Matches<T> () {
			return typeof(T) == RawType;
		}

		public static ManagedTokenType New<T> (TokenType managed) {
			return new ManagedTokenType {
				Type = managed,
				RawType = typeof(T),
			};
		}
	}
}

