using System;

namespace Betterium
{
	public class Interpreter : Chainable<Interpreter>, IFeedable<AstNode, Interpreter>
	{
		AstNode Codebase;

		#region IFeedable implementation

		public Interpreter Feed (AstNode codebase) {
			this.Codebase = codebase;
			return this;
		}

		#endregion

		public void Run () {
		}
	}
}

