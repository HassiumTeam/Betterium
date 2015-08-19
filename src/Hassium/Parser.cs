using System;
using System.Collections.Generic;

namespace Hassium
{
	public class Parser : Chainable<Parser>, IFeedable<IEnumerable<Token>, Parser>
	{
		int pos;
		List<Token> tokens;

		#region IFeedable implementation

		public Parser Feed (IEnumerable<Token> tokens) {
			this.tokens = new List<Token> (tokens);
			return this;
		}

		#endregion

		public AstNode Parse () {
			var block = new AstNode ();
			while (CanAdvance ())
				block.AddChild (ParseStatement ());
			return block;
		}

		AstNode ParseStatement () {
			if (Match (TokenType.Identifier)) {
				Console.WriteLine ("stmt::ident");
				var ident = tokens [pos].UnboxAs<string> ();
				switch (ident) {
				case "if":
					return ParseIf ();
				case "while":
					return ParseWhile ();
				default:
					var expr = ParseExpression ();
					Expect (TokenType.ExpressionTerminator);
					return expr;
				}
			} else if (Match (TokenType.OpeningBracket)) {
				Console.WriteLine ("stmt::opbr");
				return ParseCodeBlock ();
			} else {
				var expr = ParseExpression ();
				Expect (TokenType.ExpressionTerminator);
				return expr;
			}
		}

		AstNode ParseCodeBlock () {
			Console.WriteLine ("block::");
			var block = new AstNode ();
			Expect (TokenType.OpeningBracket);
			while (CanAdvance () && !Match (TokenType.ClosingBracket))
				block.AddChild (ParseStatement ());
			Expect (TokenType.ClosingBracket);
			return block;
		}

		AstNode ParseExpression () {
			Console.WriteLine ("expr::");
			return ParseAssignment ();
		}

		AstNode ParseAssignment () {
			Console.WriteLine ("ass::");
			var left = ParseEquality ();
			if (Accept (TokenType.AssignOp)) {
				var right = ParseAssignment ();
				return new NodeBinOp (BinOp.Assignment, left, right);
			}
			return left;
		}

		AstNode ParseEquality () {
			Console.WriteLine ("eq::");
			var left = ParseAdditive ();
			if (Accept (TokenType.CompOp)) {
				var compop = tokens [pos].UnboxAs<string> ();
				AstNode right;
				switch (compop) {
				case "=":
					right = ParseEquality ();
					return new NodeBinOp (BinOp.Equals, left, right);
				case "!=":
					right = ParseEquality ();
					return new NodeBinOp (BinOp.NotEquals, left, right);
				case "<":
					right = ParseEquality ();
					return new NodeBinOp (BinOp.LessThan, left, right);
				case ">":
					right = ParseEquality ();
					return new NodeBinOp (BinOp.GreaterThan, left, right);
				}
			}
			return left;
		}

		AstNode ParseAdditive () {
			Console.WriteLine ("add::");
			var left = ParseMultiplicative ();
			if (Accept (TokenType.BinOp)) {
				var binop = tokens [pos].UnboxAs<string> ();
				AstNode right;
				switch (binop) {
				case "+":
					right = ParseAdditive ();
					return new NodeBinOp (BinOp.Addition, left, right);
				case "-":
					right = ParseAdditive ();
					return new NodeBinOp (BinOp.Subtraction, left, right);
				}
			}
			return left;
		}

		AstNode ParseMultiplicative () {
			Console.WriteLine ("mul::");
			var left = ParseUnary ();
			if (Accept (TokenType.BinOp)) {
				var binop = tokens [pos].UnboxAs<string> ();
				AstNode right;
				switch (binop) {
				case "*":
					right = ParseMultiplicative ();
					return new NodeBinOp (BinOp.Multiplication, left, right);
				case "/":
					right = ParseMultiplicative ();
					return new NodeBinOp (BinOp.Division, left, right);
				}
			}
			return left;
		}

		AstNode ParseUnary () {
			Console.WriteLine ("un::");
			if (Accept (TokenType.UnOp)) {
				var unop = tokens [pos].UnboxAs<string> ();
				AstNode right;
				switch (unop) {
				case "!":
					right = ParseUnary ();
					return new NodeUnOp (UnOp.Not, right);
				}
			}
			return ParseFunctionCall ();
		}

		AstNode ParseFunctionCall () {
			Console.WriteLine ("call::");
			var term = ParseTerm ();
			return ParseFunctionCall (term);
		}

		AstNode ParseFunctionCall (AstNode left) {
			Console.WriteLine ("call::::");
			return Accept (TokenType.OpeningParen)
				? ParseFunctionCall (new NodeFuncCall (left, ParseArgList ()))
				: left;
		}

		AstNode ParseArgList () {
			Console.WriteLine ("args::");
			var arglist = new AstNode ();
			Expect (TokenType.OpeningParen);
			while (!Match (TokenType.ClosingParen)) {
				arglist.AddChild (ParseExpression ());
				if (!Accept (TokenType.Commata))
					break;
			}
			Expect (TokenType.ClosingParen);
			return arglist;
		}

		AstNode ParseTerm () {
			Console.WriteLine ("term::");
			if (Match (TokenType.Number))
				return new NodeNumber (Expect (TokenType.Number).UnboxAs<Double> ());
			else if (Accept (TokenType.OpeningParen)) {
				var statement = ParseExpression ();
				Expect (TokenType.ClosingParen);
				return statement;
			}
			else if (Match (TokenType.String))
				return new NodeString (Expect (TokenType.String).UnboxAs<String> ());
			else if (Match (TokenType.Identifier))
				return new NodeIdent (Expect (TokenType.Identifier).UnboxAs<String> ());
			else
				ThrowUnexpected ();
			return new AstNode ();
		}

		AstNode ParseIf () {
			Expect (TokenType.Identifier, "if");
			Expect (TokenType.OpeningParen);
			var predicate = ParseExpression ();
			Expect (TokenType.ClosingParen);
			var body = ParseStatement ();
			if (Accept (TokenType.Identifier, "else")) {
				var elseBody = ParseStatement ();
				return new NodeIf (predicate, body, elseBody);
			}
			return new NodeIf (predicate, body);
		}

		AstNode ParseWhile () {
			Expect (TokenType.Identifier, "while");
			Expect (TokenType.OpeningParen);
			var predicate = ParseExpression ();
			Expect (TokenType.ClosingParen);
			var body = ParseStatement ();
			if (Accept (TokenType.Identifier, "else")) {
				var elseBody = ParseStatement ();
				return new NodeWhile (predicate, body, elseBody);
			}
			return new NodeWhile (predicate, body);
		}

		bool Match (TokenType type) {
			return CanAdvance () && tokens [pos].Type == type;
		}

		bool Match (TokenType type, object val) {
			return CanAdvance () && tokens [pos].Type == type && tokens [pos].Value == val;
		}

		bool Accept (TokenType type) {
			if (Match (type)) {
				pos++;
				return true;
			}
			return false;
		}

		bool Accept (TokenType type, object val) {
			if (Match (type, val)) {
				pos++;
				return true;
			}
			return false;
		}

		Token Expect (TokenType type) {
			if (!Match (type))
				ThrowExpected (type);
			return tokens [pos++];
		}

		Token Expect (TokenType type, object val) {
			if (!Match (type, val))
				ThrowExpected (type);
			return tokens [pos++];
		}

		void ThrowExpected (TokenType type) {
			var format = string.Format ("*** Expected: '{0}'; Got: '{1}'\n*** Value: '{2}' at line {3}; position {4}",
				type, tokens [pos].Type, tokens [pos].Value, tokens [pos].Line, tokens [pos].LinePos);
			throw new Exception (format);
		}

		void ThrowUnexpected () {
			var format = string.Format ("*** Unexpected: '{0}'", tokens [pos].Type);
			throw new Exception (format);
		}

		bool CanAdvance (int count = 1) {
			return pos + count < tokens.Count;
		}
	}
}

