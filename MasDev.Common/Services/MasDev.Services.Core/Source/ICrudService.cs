using System.Threading.Tasks;
using System.Collections.Generic;
using MasDev.Common;

namespace MasDev.Services
{
	public interface IService
	{
		Task AuthorizeAsync (int? minimumRoles = null);

		IIdentityContext IdentityContext { get; }
	}

	public interface ICrudService<TDto> : IService where TDto : IEntity
	{
		Task<TDto> CreateAsync (TDto dto);

		Task<IList<TDto>> ReadPagedAsync (int skip, int take);

		Task<TDto> ReadAsync (int id);

		Task<TDto> UpdateAsync (TDto dto);

		Task DeleteAsync (int id);
	}
}

