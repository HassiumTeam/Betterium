using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Hassium
{
    public class Lexer
    {
        private string code = "";
        private int position = 0;
        private List<OldToken> result = new List<OldToken>();

        public Lexer(string code)
        {
            this.code = code;
        }

        public List<OldToken> Tokenize()
        {
            whiteSpaceMonster();

            while (peekChar() != -1)
            {
                if (char.IsLetterOrDigit((char)peekChar()))
                {
                    result.Add(scanData());
                }
                else if ((char)(peekChar()) == '\"')
                {
                    result.Add(scanString());
                }
                else if ((char)(peekChar()) == '$')
                {
                    scanComment();
                }
                else if ((char)(peekChar()) == ';')
                {
                    result.Add(new OldToken(OldTokenType.EndOfLine, ((char)readChar()).ToString()));
                }
                else if ((char)(peekChar()) == '(' || (char)(peekChar()) == ')')
                {
                    result.Add(new OldToken(OldTokenType.Parentheses, ((char)readChar()).ToString()));
                }
                else if ((char)(peekChar()) == '{' || (char)(peekChar()) == '}')
                {
                    result.Add(new OldToken(OldTokenType.Bracket, ((char)readChar()).ToString()));
                }
                else if ((char)(peekChar()) == ',')
                {
                    result.Add(new OldToken(OldTokenType.Comma, ((char)readChar()).ToString()));
                }
                else if ("+-/*".Contains((((char)peekChar()).ToString())))
                {
                    result.Add(new OldToken(OldTokenType.Operation, ((char)readChar()).ToString()));
                }
                else if ("=<>".Contains((((char)peekChar()).ToString())))
                {
                    result.Add(new OldToken(OldTokenType.Comparison, ((char)readChar()).ToString()));
                }
                else if ((char)(peekChar()) == '!' && (char)(peekChar(1)) == '=')
                {
                    result.Add(new OldToken(OldTokenType.Comparison, ((char)readChar()).ToString() + ((char)readChar()).ToString()));
                }
                else if ((char)(peekChar()) == ':' && (char)(peekChar(1)) == '=')
                {
                    result.Add(new OldToken(OldTokenType.Store, ((char)readChar()).ToString() + ((char)readChar()).ToString()));

                }
                else if ((char)(peekChar()) == '!' && !((char)(peekChar(1)) == '='))
                {
                    result.Add(new OldToken(OldTokenType.Not, ((char)readChar()).ToString()));
                }
                else
                {
                    result.Add(new OldToken(OldTokenType.Exception, "Unexpected " + ((char)peekChar()).ToString() + " encountered"));
                    readChar();
                }

                whiteSpaceMonster();
            }

            return result;
        }

        private void scanComment()
        {
            readChar();
            while(peekChar() != '$' && peekChar() != -1)
            {
                readChar();
            }
            readChar();
        }

        private OldToken scanString()
        {
            readChar();
            string result = "";

            while (peekChar() != '\"' && peekChar() != -1)
                result += ((char)readChar()).ToString();

            readChar();

            return new OldToken(OldTokenType.String, result);
        }

        private OldToken scanData()
        {
            string result = "";
            double temp = 0;;
            while ((char.IsLetterOrDigit((char)peekChar()) && peekChar() != -1) || ((char)(peekChar()) == '.'))
                result += ((char)readChar()).ToString();
            if (double.TryParse(result, out temp))
                return new OldToken(OldTokenType.Number, result);
            return new OldToken(OldTokenType.Identifier, result);
        }

        private void whiteSpaceMonster()
        {
            while(char.IsWhiteSpace((char)peekChar())) readChar();
        }

        private int peekChar()
        {
            if (position < code.Length)
                return code[position];
            else
                return -1;
        }
        private int peekChar(int n)
        {
            if (position + n < code.Length)
                return code[position + n];
            else
                return -1;
        }

        private int readChar()
        {
            if (position < code.Length)
                return code[position++];
            else
                return -1;
        }
    }
}

