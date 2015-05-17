using MasDev.Services.Modeling;
using System.Threading.Tasks;
using System;

namespace MasDev.Services.DataAccess
{
	public interface IDataAccessValidator<TDto> where TDto : class, IDto
	{
		bool CanAccess (TDto dto, IIdentityContext context);

		bool CanAccess (int id, IIdentityContext context);

		Task<bool> CanAccessAsync (TDto dto, IIdentityContext context);

		Task<bool> CanAccessAsync (int id, IIdentityContext context);

		bool IsAsync { get; }
	}

	public abstract class DataAccessValidator<TDto> : IDataAccessValidator<TDto> where TDto : class, IDto
	{
		protected abstract bool CanAccess (int id, IIdentityContext context);

		public virtual bool CanAccess (TDto dto, IIdentityContext context)
		{
			return dto != null && CanAccess (dto.Id, context);
		}

		bool IDataAccessValidator<TDto>.CanAccess (int id, IIdentityContext context)
		{
			return (this as DataAccessValidator<TDto>).CanAccess (id, context);
		}

		public Task<bool> CanAccessAsync (TDto dto, IIdentityContext context)
		{
			throw new NotSupportedException ("Asyncronous operation not supported");
		}

		public Task<bool> CanAccessAsync (int id, IIdentityContext context)
		{
			throw new NotSupportedException ("Asyncronous operation not supported");
		}

		public bool IsAsync { get { return false; } }
	}

	public abstract class AsyncDataAccessValidator<TDto> : IDataAccessValidator<TDto> where TDto : class, IDto
	{
		protected abstract Task<bool> CanAccessAsync (int id, IIdentityContext context);

		public virtual bool CanAccess (TDto dto, IIdentityContext context)
		{
			throw new NotSupportedException ("Syncronous operation not supported");
		}

		public bool CanAccess (int id, IIdentityContext context)
		{
			throw new NotSupportedException ("Syncronous operation not supported");
		}

		public virtual async Task<bool> CanAccessAsync (TDto dto, IIdentityContext context)
		{
			return dto != null && await CanAccessAsync (dto.Id, context);
		}

		async Task<bool> IDataAccessValidator<TDto>.CanAccessAsync (int id, IIdentityContext context)
		{
			return await (this as AsyncDataAccessValidator<TDto>).CanAccessAsync (id, context);
		}

		public bool IsAsync { get { return true; } }
	}
}

