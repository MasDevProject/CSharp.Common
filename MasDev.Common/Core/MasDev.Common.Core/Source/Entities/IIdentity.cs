using System;

namespace MasDev.Common
{
	public interface IIdentity
	{
		int Id { get; set; }

		int Roles { get; set; }

		int Flag { get; set; }
	}
}

