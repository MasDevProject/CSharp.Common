﻿using System;
using System.Threading;

namespace MasDev.Common.Tasks
{
	public static class CancellationTokenExtensions
	{
		public static bool WaitCancellationRequested (this CancellationToken token, TimeSpan timeout)
		{
			return token.WaitHandle.WaitOne (timeout);
		}
	}
}

