using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Betterium
{
	public class Interpreter : Chainable<Interpreter>, IFeedable<AstNode, Interpreter>
	{
		AstNode Codebase;
		List<Variable> Variables;
		List<Function> Functions;
		bool Debug;
		bool Warn;
		bool Error;

		public Interpreter () {
			Variables = new List<Variable> ();
			Functions = new List<Function> ();
		}

		#region IFeedable implementation

		public Interpreter Feed (AstNode codebase) {
			Codebase = codebase;
			return this;
		}

		#endregion

		public void Run () {
			InterpretNode (Codebase);
		}

		public Interpreter Verbose (Verbosity verb, bool value) {
			switch (verb) {
			case Verbosity.Debug:
				Debug = value;
				break;
			case Verbosity.Warn:
				Warn = value;
				break;
			case Verbosity.Error:
				Error = value;
				break;
			}
			return this;
		}

		object InterpretNode (AstNode node) {
			switch (node.Type) {
			case NodeType.Codeblock:
				node.Children.ForEach (n => InterpretNode (n));
				break;
			case NodeType.Statement:
				InterpretStatement (node);
				break;
			case NodeType.Operation:
				return InterpretOperation (node);
			case NodeType.Number:
				var num = node as NodeNumber;
				if (num != null)
					return num.Value;
				throw new Exception ("*** Expected number.");
			case NodeType.String:
				var str = node as NodeString;
				if (str != null)
					return str.Value;
				throw new Exception ("*** Expected string.");
			case NodeType.Function:
				var func = node as NodeFuncCall;
				if (func == null)
					throw new Exception ("*** Expected function.");
				var target = func.Target as NodeIdent;
				var fxargs = func.Arguments.Children.ToList ();
				var args = new List<object> (fxargs.Count);
				fxargs.ForEach (n => args.Add (InterpretNode (n)));
				TryCall (target.Value, args.ToArray ());
				break;
			case NodeType.Identifier:
				var ident = node as NodeIdent;
				if (ident == null)
					throw new Exception ("*** Expected identifier.");
				var first = Variables.FirstOrDefault (v => v.Name == ident.Value);
				if (first != default (Variable))
					return first.Value;
				throw new Exception ("*** Variable not found.");
			}
			return null;
		}

		object InterpretOperation (AstNode node) {
			var binop = node as NodeBinOp;
			var unop = node as NodeUnOp;

			if (binop != null)
				return InterpretOperationBin (binop);
			
			else if (unop != null)
				return InterpretOperationUn (unop);

			return null;
		}

		object InterpretOperationBin (NodeBinOp node) {
			switch (node.Type) {
			case BinOp.Addition:
				var op1 = InterpretNode (node.Left) as decimal?;
				var op2 = InterpretNode (node.Right) as decimal?;
				ThrowNullDecimal (op1, op2);
				return (decimal)op1 + (decimal)op2;
			case BinOp.Subtraction:
				op1 = InterpretNode (node.Left) as decimal?;
				op2 = InterpretNode (node.Right) as decimal?;
				ThrowNullDecimal (op1, op2);
				return (decimal)op1 - (decimal)op2;
			case BinOp.Division:
				op1 = InterpretNode (node.Left) as decimal?;
				op2 = InterpretNode (node.Right) as decimal?;
				ThrowNullDecimal (op1, op2);
				return (decimal)op1 / (decimal)op2;
			case BinOp.Multiplication:
				op1 = InterpretNode (node.Left) as decimal?;
				op2 = InterpretNode (node.Right) as decimal?;
				ThrowNullDecimal (op1, op2);
				return (decimal)op1 * (decimal)op2;
			case BinOp.Equals:
				op1 = InterpretNode (node.Left) as decimal?;
				op2 = InterpretNode (node.Right) as decimal?;
				ThrowNullDecimal (op1, op2);
				return (decimal)op1 == (decimal)op2;
			case BinOp.NotEquals:
				op1 = InterpretNode (node.Left) as decimal?;
				op2 = InterpretNode (node.Right) as decimal?;
				ThrowNullDecimal (op1, op2);
				return (decimal)op1 != (decimal)op2;
			case BinOp.GreaterThan:
				op1 = InterpretNode (node.Left) as decimal?;
				op2 = InterpretNode (node.Right) as decimal?;
				ThrowNullDecimal (op1, op2);
				return (decimal)op1 > (decimal)op2;
			case BinOp.LessThan:
				op1 = InterpretNode (node.Left) as decimal?;
				op2 = InterpretNode (node.Right) as decimal?;
				ThrowNullDecimal (op1, op2);
				return (decimal)op1 < (decimal)op2;
			case BinOp.Assignment:
				var left = node.Left as NodeIdent;
				if (left == null)
					throw new Exception ("*** Expected identifier.");
				var right = InterpretNode (node.Right);
				var first = Variables.FirstOrDefault (v => v.Name.ToLowerInvariant () == left.Value.ToLowerInvariant ());
				if (first != default (Variable))
					Variables.Remove (first);
				Variables.Add (new Variable (left.Value, right));
				return right;
			default:
				throw new Exception ("*** Invalid Binary Operation.");
			}
		}

		object InterpretOperationUn (NodeUnOp node) {
			switch (node.Type) {
			case UnOp.Not:
				var val = InterpretNode (node.Value) as bool?;
				if (val == null)
					throw new Exception ("*** Expected boolean.");
				return !(bool)val;
			default:
				throw new Exception ("*** Invalid Unary Operation.");
			}
		}

		void InterpretStatement (AstNode node) {
			var stmtif = node as NodeIf;
			var stmtwhile = node as NodeWhile;

			if (stmtif != null)
				InterpretStatementIf (stmtif);
			
			else if (stmtwhile != null)
				InterpretStatementWhile (stmtwhile);
		}

		void InterpretStatementIf (NodeIf node) {
			
		}

		void InterpretStatementWhile (NodeWhile node) {
		}

		void TryCall (object func, object[] args) {
			//Console.WriteLine (":: {0}", func);
			//for (var i = 0; i < args.Length; i++)
			//	Console.WriteLine (":: :: [{0}] {1}", i, args [i]);
			Functions.ForEach (fx => {
				try {
					if (fx.Name.ToLowerInvariant () == func.ToString ().ToLowerInvariant ())
						fx.Invoke (args);
				} catch (Exception e) {
					if (Error) {
						var func_name = (fx ?? new Function ("undefined", null)).Name;
						Console.WriteLine ("[ERR ] Couldn't call function: {0}", func_name);
						if (args.Any (arg => arg == null)) {
							Console.WriteLine ("[ERR ] Any of the arguments is NULL.");
							var i = 0;
							foreach (var arg in args)
								Console.WriteLine ("[ERR ] * [{0}]: {1}", i++, arg ?? "NULL");
						} else
							Console.WriteLine ("[ERR ] {0}", e.Message);
					}
				}
			});
		}

		void ThrowNullDecimal (decimal? op1, decimal? op2) {
			if (op1 == null || op2 == null)
				throw new Exception ("*** Expected decimal.");
		}

		public Interpreter Import (string lib, bool suppress_rebuild_lookup = false) {
			if (!lib.EndsWith (".dll"))
				lib += ".dll";
			var path = System.IO.Path.GetFullPath (lib);
			try {
				var asm = Assembly.LoadFile (path);
				var types = asm.GetTypes ().Where (t => t.IsSubclassOf (typeof(Library)));
				var libraries = types.Select (t => (Library)Activator.CreateInstance (t));
				var functions = libraries.Select (t => t.ExportedFunctions);
				functions.ToList ().ForEach (Functions.AddRange);
				if (!suppress_rebuild_lookup)
					Functions = Functions.Distinct ().ToList ();
			} catch {
				if (Warn)
					Console.WriteLine ("[WARN] Failed to import '{0}'", lib);
			}
			return this;
		}

		public Interpreter Import (IEnumerable<string> libs) {
			libs.ToList ().ForEach (lib => Import (lib, true));
			Functions = Functions.Distinct ().ToList ();
			return this;
		}
	}
}

