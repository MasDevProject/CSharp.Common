using System.Threading.Tasks;
using System.Collections.Generic;
using MasDev.Common;

namespace MasDev.Services
{
	public interface IService
	{

	}

	public interface ICrudService<TDto> : IService where TDto : IEntity
	{
		Task<TDto> CreateAsync (TDto dto, IIdentityContext context);

		Task<IList<TDto>> ReadAsync (int skip, int take, IIdentityContext context);

		Task<TDto> ReadAsync (int id, IIdentityContext context);

		Task<TDto> UpdateAsync (TDto dto, IIdentityContext context);

		Task DeleteAsync (int id, IIdentityContext context);
	}
}

