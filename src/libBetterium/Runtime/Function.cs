using System;
using System.Collections.Generic;

namespace Betterium
{
	public class Function
	{
		public readonly string Name;
		readonly FunctionInvoker Invoker;

		public Function (string name, FunctionInvoker invoker) {
			Name = name;
			Invoker = invoker;
		}

		public static Function From (string name, FunctionInvoker invoker) {
			return new Function (name, invoker);
		}

		public object Invoke (params object[] args) {
			return Invoker (args);
		}
	}
}

