using System;

namespace MasDev.Common.Extensions
{
	public static class CurrenciesExtensions
	{
		public static string ToPrettyString (this Currencies currency)
		{
			switch (currency) {
			case Currencies.Dollar:
				return "$";
			case Currencies.Euro:
				return "€";
			case Currencies.Pound:
				return "£";
			default:
				throw new NotSupportedException ("Currency not supported");
			}
		}
	}
}

