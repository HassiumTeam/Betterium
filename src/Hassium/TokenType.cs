using System;

namespace Hassium
{
	public enum TokenType {
		// ;
		ExpressionTerminator,
		// xyz
		Identifier,
		// {
		OpeningBracket,
		// }
		ClosingBracket,
		// (
		OpeningParen,
		// )
		ClosingParen,
		// "test" 'test'
		String,
		// 123 0.123
		Number,
		// ,
		Commata,
		// + - * /
		BinOp,
		// !
		UnOp,
		// < = > !=
		CompOp,
		// :=
		AssignOp,
	}
}

