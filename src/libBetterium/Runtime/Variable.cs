using System;

namespace Betterium
{
	public class Variable
	{
		public string Name;
		public object Value;

		public Variable (string name, object value) {
			Name = name;
			Value = value;
		}

		public T UnboxAs<T> () {
			return (T)Value;
		}
	}
}

