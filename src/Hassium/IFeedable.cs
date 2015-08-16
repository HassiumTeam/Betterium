using System;

namespace Hassium
{
	public interface IFeedable<in TIn, out TOut> {
		TOut Feed (TIn objs);
	}
}

