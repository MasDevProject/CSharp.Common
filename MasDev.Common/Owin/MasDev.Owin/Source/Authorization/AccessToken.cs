﻿using System;


namespace MasDev.Owin.Auth
{
	public class AccessToken
	{
		public Credentials Credentials { get; set; }

		public DateTime CreationUtc { get; set; }

		public DateTime ExpirationUtc { get; set; }

		public int? Scope { get; set; }
	}
}

