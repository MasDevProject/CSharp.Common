﻿using System.Net.Http;
using System.Threading.Tasks;


namespace MasDev.Extensions
{
	public static class WebExtensions
	{
		public static async Task<string> AsStringAsync (this HttpResponseMessage mess)
		{
			return await mess.Content.ReadAsStringAsync ();
		}
	}
}

