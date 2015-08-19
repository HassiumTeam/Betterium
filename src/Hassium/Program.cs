using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using Codeaddicts.libArgument;

namespace Betterium
{
    public class Betterium
    {
		Options Options;

		Betterium (Options options) {
			this.Options = options;
		}

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
			}

			// Read input file
			var source = File.ReadAllText (options.input);

			// Create Betterium instance
			var instance = new Betterium (options);
			instance.Run (source);
        }

		void Run (string src) {
			if (Options.time)
				RunTimed (src);
			else
				RunNormal (src);
		}

		void RunNormal (string src) {
			var tokens = Scanner
				.GrabNew ()
				.Feed (src)
				.Scan ();
			var codebase = Parser
				.GrabNew ()
				.Feed (tokens)
				.Parse ();
			Interpreter
				.GrabNew ()
				.Feed (codebase)
				.Run ();
		}

		void RunTimed (string src) {
			// Grab scanner
			var scanner = Scanner
				.GrabNew ()
				.Feed (src);

			// Instantiate stopwatch
			var stopwatch = new Stopwatch ();

			IEnumerable<Token> tokens;
			try {
				stopwatch.Restart ();
				tokens = scanner.Scan ();
				stopwatch.Stop ();
			} catch (Exception e) {
				if (Options.w_error)
					Console.WriteLine (e.Message);
				return;
			} finally {
				stopwatch.Stop ();
			}
			Console.WriteLine ("Scanning the source took {0} milliseconds.", stopwatch.ElapsedMilliseconds);

			var parser = Parser
				.GrabNew ()
				.Feed (tokens);

			AstNode ast;
			try {
				stopwatch.Restart ();
				ast = parser.Parse ();
				stopwatch.Stop ();
				DumpAst (ast);
			} catch (Exception e) {
				if (Options.w_error)
					Console.WriteLine (e.Message);
			} finally {
				stopwatch.Stop ();
			}
			Console.WriteLine ("Parsing the tokens took {0} milliseconds.", stopwatch.ElapsedMilliseconds);

			/*
			Interpreter
				.GrabNew ()
				.Feed (ast)
				.Execute ();
			*/
		}

		void DumpAst (AstNode node, int depth = 0) {
			Console.WriteLine ("{0}{1}", "".PadLeft (depth * 2, '-'), node.Name);
			foreach (var child in node.Children)
				DumpAst (child, depth + 1);
		}
    }
}

