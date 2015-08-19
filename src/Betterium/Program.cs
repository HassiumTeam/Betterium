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
			Options = options;
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
			RunNormal (src);
		}

		void RunNormal (string src) {
			var stopwatch = new Stopwatch ();

			stopwatch.Start ();
			var tokens = Scanner
				.GrabNew ()
				.Feed (src)
				.Scan ();
			stopwatch.Stop ();

			if (Options.time)
				Console.WriteLine ("[INFO] Lexical analysis took {0} milliseconds.", stopwatch.ElapsedMilliseconds);

			stopwatch.Restart ();
			var codebase = Parser
				.GrabNew ()
				.Feed (tokens)
				.Parse ();
			stopwatch.Stop ();

			if (Options.time)
				Console.WriteLine ("[INFO] Logical analysis took {0} milliseconds.", stopwatch.ElapsedMilliseconds);

			if (Options.dump_ast) {
				Console.WriteLine ("[INFO] AST dump:");
				DumpAst (codebase);
			}

			Console.Write ("\n");
			Interpreter
				.GrabNew ()
				.Feed (codebase)
				.Import (Options.lib.Split (','))
				.Run ();
		}

		void DumpAst (AstNode node, int depth = 0) {
			Console.WriteLine ("{0}* {1}", "".PadLeft (depth * 2, ' '), node.Name);
			foreach (var child in node.Children)
				DumpAst (child, depth + 1);
		}
    }
}

