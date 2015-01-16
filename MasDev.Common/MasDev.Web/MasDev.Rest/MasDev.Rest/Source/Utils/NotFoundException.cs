﻿using System.Net;


namespace MasDev.Rest
{
	public class NotFoundException : BaseApiException
	{
		public NotFoundException (string message) : base (HttpStatusCode.NotFound, message)
		{
		}



		public NotFoundException () : base (HttpStatusCode.NotFound, null)
		{
		}
	}
}
