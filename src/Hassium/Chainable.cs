﻿using System;

namespace Hassium
{
	public abstract class Chainable<T> where T : new() {
		public static T GrabNew () {
			return new T ();
		}
	}
}

