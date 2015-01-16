using System;

namespace MasDev.Common.Security
{
	public class Sha : IHashAlgorithm
	{
		private static Sha _instance;

		private Sha ()
		{ 
			// singleton
		}

		public static Sha Instance ()
		{
			if (_instance == null)
				_instance = new Sha ();

			return _instance;
		}

		public string Hash (string message, string salt)
		{
			throw new NotImplementedException ();
		}

		public byte[] Hash (byte[] message, byte[] salt)
		{
			throw new NotImplementedException ();
		}
	}
}

