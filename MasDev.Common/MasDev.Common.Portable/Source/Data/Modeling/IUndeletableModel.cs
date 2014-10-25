

namespace MasDev.Common.Modeling
{
	public interface IUndeletableModel : IModel
	{
		bool IsEnabled { get; set; }
	}
}

