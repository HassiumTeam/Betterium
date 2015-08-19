using System;

namespace Betterium
{
	public interface IFeedable<in TIn, out TOut> {
		TOut Feed (TIn objs);
	}
}

