using MasDev.Services.Modeling;
using System.Threading.Tasks;
using System;
using MasDev.Data;

namespace MasDev.Services.DataAccess
{
	public interface IDataAccessValidator<TDto, TModel> 
		where TDto : IDto 
		where TModel : IModel
	{
		bool CanAccess (TDto dto, IIdentityContext context);

		bool CanAccess (int id, IIdentityContext context);

		bool CanAccess (TModel model, IIdentityContext context);

		Task<bool> CanAccessAsync (TDto dto, IIdentityContext context);

		Task<bool> CanAccessAsync (int id, IIdentityContext context);

		Task<bool> CanAccessAsync (TModel model, IIdentityContext context);

		bool IsAsync { get; }
	}

	public abstract class DataAccessValidator<TDto, TModel> : IDataAccessValidator<TDto, TModel> 
		where TDto : class, IDto
		where TModel : class, IModel
	{
		protected abstract bool CanAccess (int id, IIdentityContext context);

		public virtual bool CanAccess (TDto dto, IIdentityContext context)
		{
			return dto != null && CanAccess (dto.Id, context);
		}

		public virtual bool CanAccess (TModel model, IIdentityContext context)
		{
			return model != null && CanAccess (model.Id, context);
		}

		bool IDataAccessValidator<TDto, TModel>.CanAccess (int id, IIdentityContext context)
		{
			return (this as DataAccessValidator<TDto, TModel>).CanAccess (id, context);
		}

		public Task<bool> CanAccessAsync (TDto dto, IIdentityContext context)
		{
			throw new NotSupportedException ("Asyncronous operation not supported");
		}

		public Task<bool> CanAccessAsync (TModel model, IIdentityContext context)
		{
			throw new NotSupportedException ("Asyncronous operation not supported");
		}

		public Task<bool> CanAccessAsync (int id, IIdentityContext context)
		{
			throw new NotSupportedException ("Asyncronous operation not supported");
		}

		public bool IsAsync { get { return false; } }
	}

	public abstract class AsyncDataAccessValidator<TDto, TModel> : IDataAccessValidator<TDto, TModel> 
		where TDto : class, IDto
		where TModel : class, IModel
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

		public bool CanAccess (TModel model, IIdentityContext context)
		{
			throw new NotSupportedException ("Syncronous operation not supported");
		}

		public virtual async Task<bool> CanAccessAsync (TDto dto, IIdentityContext context)
		{
			return dto != null && await CanAccessAsync (dto.Id, context);
		}

		async Task<bool> IDataAccessValidator<TDto, TModel>.CanAccessAsync (int id, IIdentityContext context)
		{
			return await (this as AsyncDataAccessValidator<TDto, TModel>).CanAccessAsync (id, context);
		}

		async Task<bool> IDataAccessValidator<TDto, TModel>.CanAccessAsync (TModel model, IIdentityContext context)
		{
			return await (this as AsyncDataAccessValidator<TDto, TModel>).CanAccessAsync (model.Id, context);
		}

		public bool IsAsync { get { return true; } }
	}
}

