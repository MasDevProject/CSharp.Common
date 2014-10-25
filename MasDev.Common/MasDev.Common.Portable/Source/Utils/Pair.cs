
namespace MasDev.Common.Utils
{
	public class Pair<TFirst, TSecond>
	{
		public TFirst First { get; set; }
		public TSecond Second { get; set; }

		public Pair (TFirst first, TSecond second)
		{
			First = first;
			Second = second;
		}
	}
}

