﻿namespace MasDev.Services
{
	/// <summary>
	/// Which type of redirect
	/// </summary>
	public enum RedirectType
	{
		/// <summary>
		/// HTTP 301 - All future requests should be to this URL
		/// </summary>
		Permanent,
		/// <summary>
		/// HTTP 307 - Redirect this request but allow future requests to the original URL
		/// </summary>
		Temporary,
		/// <summary>
		/// HTTP 303 - Redirect this request using an HTTP GET
		/// </summary>
		SeeOther
	}

	public sealed class Redirect
	{
		public string To { get; private set; }

		public RedirectType ResourceType { get; private set; }

		public Redirect (string resourceUrl, RedirectType redirectType)
		{
			To = resourceUrl;
			ResourceType = redirectType;
		}
	}
}