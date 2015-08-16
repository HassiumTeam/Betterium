using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Hassium
{
	public class Scanner : Chainable<Scanner>, IFeedable<string, Scanner>
	{
		int pos, line, linepos;
		string source;
		List<Token> tokens;

		public Scanner () {
			pos = -1;
			line = -1;
			linepos = 0;
			tokens = new List<Token> ();
		}

		#region IFeedable implementation

		public Scanner Feed (string source) {
			this.source = source;
			return this;
		}

		#endregion

		public IEnumerable<Token> Scan () {
			while (CanAdvance ()) {
				
				EatWhitespace ();

				if (!CanAdvance ())
					break;

				if (char.IsLetterOrDigit (Peek ())) {
					tokens.Add (ScanData ());
					continue;
				}

				switch (Peek ()) {
				case '\'':
				case '"':
					tokens.Add (ScanString ());
					break;
				case '$':
					SkipComment ();
					break;
				case ';':
					tokens.Add (new Token (TokenType.ExpressionTerminator, ReadStr ()));
					break;
				case '(':
					tokens.Add (new Token (TokenType.OpeningParen, ReadStr ()));
					break;
				case ')':
					tokens.Add (new Token (TokenType.ClosingParen, ReadStr ()));
					break;
				case '{':
					tokens.Add (new Token (TokenType.OpeningBracket, ReadStr ()));
					break;
				case '}':
					tokens.Add (new Token (TokenType.ClosingBracket, ReadStr ()));
					break;
				case ',':
					tokens.Add (new Token (TokenType.Commata, ReadStr ()));
					break;
				case '+':
				case '-':
				case '*':
				case '/':
					tokens.Add (new Token (TokenType.BinOp, ReadStr ()));
					break;
				case '=':
				case '<':
				case '>':
					tokens.Add (new Token (TokenType.CompOp, ReadStr ()));
					break;
				case '!':
					if (Peek (2) == '=') {
						Skip (2);
						tokens.Add (new Token (TokenType.CompOp, "!="));
					} else
						tokens.Add (new Token (TokenType.UnOp, ReadStr ()));
					break;
				case ':':
					if (Peek (2) == '=') {
						Skip (2);
						tokens.Add (new Token (TokenType.AssignOp, ":="));
					} else
						ThrowUnexpectedToken ();
					break;
				default:
					ThrowUnexpectedToken ();
					break;
				}
			}
			return tokens;
		}

		void ThrowUnexpectedToken (string token = null) {
			token = token ?? Peek ().ToString ();
			var format = string.Format ("Unexpected token '{0}' at line {1}, position {2}", token, line, linepos);
			throw new Exception (format);
		}

		Token ScanData () {
			var accum = new StringBuilder ();
			while (CanAdvance () && (char.IsLetterOrDigit (Peek ()) || Peek () == '.'))
				accum.Append (Read ());
			var tmp = .0d;
			return double.TryParse (accum.ToString (), NumberStyles.Float, CultureInfo.InvariantCulture, out tmp)
				? new Token (TokenType.Number, tmp)
				: new Token (TokenType.Identifier, accum.ToString ());
		}

		Token ScanString () {
			var accum = new StringBuilder ();
			var opening = Read ();
			while (CanAdvance ()) {
				if (Peek () == opening) {
					Skip ();
					break;
				} else if (Peek () == '\\') {
					accum.Append (Read ());
					switch (Peek ()) {
					default:
						accum.Append (Read ());
						break;
					}
				} else
					accum.Append (Read ());
			}
			return new Token (TokenType.String, accum.ToString ());
		}

		void SkipComment () {
			Skip ();
			while (CanAdvance () && Peek () != '$')
				Skip ();
			Skip ();
		}

		void EatWhitespace () {
			while (CanAdvance () && char.IsWhiteSpace (Peek ())) {
				if (Peek () == '\n')
					line++;
				Skip ();
			}
		}

		bool CanAdvance (int count = 1) {
			return pos + count < source.Length;
		}

		char Peek (int lookahead = 1) {
			return CanAdvance (lookahead) ? source [pos + lookahead] : '\0';
		}

		char Read () {
			return source [++pos];
		}

		string ReadStr () {
			return Read ().ToString ();
		}

		void Skip (int count = 1) {
			for (var i = 0; i < count; i++) {
				if (Peek () == '\n') {
					line++;
					linepos = 0;
				}
				else
					linepos++;
				pos += 1;
			}
		}
	}
}

