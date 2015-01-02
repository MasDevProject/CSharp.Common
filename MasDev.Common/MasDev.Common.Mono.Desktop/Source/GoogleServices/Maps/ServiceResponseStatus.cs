using System;


namespace MasDev.GoogleServices.Maps
{
	[Flags]
	public enum ServiceResponseStatus
	{
		Unknown = 0,
		Ok = -1,
		InvalidRequest = 1,
		ZeroResults = 2,
		OverQueryLimit = 3,
		RequestDenied = 4
	}
}

