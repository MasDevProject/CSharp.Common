using System;
using MasDev.Common.Modeling;


namespace MasDev.Common.Rest.Auth
{
	public interface ICredentials : IModel
	{
		int Roles { get; set; }



		DateTime LastIssuedUTC { get; set; }



		bool IsEnabled { get; set; }
	}
}

