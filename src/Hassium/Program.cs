using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Codeaddicts.libArgument;

namespace Hassium
{
    public static class HassiumInterpreter
    {
        public static void Main(string[] args)
        {
			var options = ArgumentParser.Parse<Options> (args);

			// Check input
			if (options.input == null) {
				Console.WriteLine ("Please specify an input file.");
				ArgumentParser.Help ();
				return;
			}

			// Check w flags
			if (options.w_all) {
				options.w_error = true;
				options.w_warn = true;
				options.w_debug = true;
				options.w_time = true;
			}

			// Read input file
			var input = File.ReadAllText (options.input);

			// Grab scanner
			var scanner = Scanner
				.GrabNew ()
				.Feed (input);

			// Instantiate stopwatch
			var stopwatch = new Stopwatch ();

			IEnumerable<Token> tokens;
			try {
				stopwatch.Restart ();
				tokens = scanner.Scan ();
			} catch (Exception e) {
				if (options.w_error)
					Console.WriteLine (e.Message);
				return;
			} finally {
				stopwatch.Stop ();
			}
			if (options.w_time)
				Console.WriteLine ("Scanning the source took {0} milliseconds.", stopwatch.ElapsedMilliseconds);
			
			var parser = Parser
				.GrabNew ()
				.Feed (tokens);

			AstNode ast;
			try {
				stopwatch.Restart ();
				ast = parser.Parse ();
				DumpAst (ast);
			} catch (Exception e) {
				if (options.w_error)
					Console.WriteLine (e.Message);
			} finally {
				stopwatch.Stop ();
			}
			if (options.w_time)
				Console.WriteLine ("Parsing the tokens took {0} milliseconds.", stopwatch.ElapsedMilliseconds);

			/*
			Interpreter
				.GrabNew ()
				.Feed (ast)
				.Execute ();
			*/

			/*
            List<Token> tokens = new Lexer(File.ReadAllText(args[0])).Tokenize();
            //Debug.PrintTokens(tokens);
            Parser hassiumParser = new Parser(tokens);
            AstNode ast = hassiumParser.Parse();
            new Interpreter(ast).Execute();
            */
        }

		public static void DumpAst (AstNode node, int depth = 0) {
			Console.WriteLine ("{0} {1}", "".PadLeft (depth, '-'), node);
			foreach (var child in node.Children) {
				DumpAst (child, depth + 1);
			}
		}
    }
}

