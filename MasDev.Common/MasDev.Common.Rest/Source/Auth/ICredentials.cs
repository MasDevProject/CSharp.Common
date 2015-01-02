using System;
using MasDev.Data;


namespace MasDev.Rest.Auth
{
	public interface ICredentials : IModel
	{
		int Roles { get; set; }



		DateTime LastIssuedUTC { get; set; }


		int Flag { get; set; }


		bool IsEnabled { get; set; }
	}
}

