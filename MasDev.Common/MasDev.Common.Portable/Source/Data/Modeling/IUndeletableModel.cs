

namespace MasDev.Common.Modeling
{
	public interface IUndeletableModel : IModel
	{
		bool IsDeleted { get; set; }
	}
}

