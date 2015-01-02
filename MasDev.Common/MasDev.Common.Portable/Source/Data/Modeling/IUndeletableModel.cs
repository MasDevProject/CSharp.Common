

namespace MasDev.Data
{
	public interface IUndeletableModel : IModel
	{
		bool IsDeleted { get; set; }
	}
}

