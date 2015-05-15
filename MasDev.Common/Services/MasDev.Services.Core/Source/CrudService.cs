using MasDev.Services.Modeling;
using MasDev.Data;
using MasDev.Patterns.Injection;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

namespace MasDev.Services
{
	public class CrudService<TDto, TModel, TMapper, TRepository> : IDisposable, ICrudService<TDto>
		where TDto: class, IDto
		where TModel : class, IModel, new()
		where TRepository : class, IRepository<TModel>
		where TMapper : DtoMapper<TDto, TModel>
	{
		protected TRepository Repository { get; private set; }

		protected TMapper DtoMapper { get; private set; }

		protected Identity Identity { get; private set; }

		protected int? Scope { get; private set; }

		protected virtual IQueryable<TModel> Query { get { return Repository.Query; } }

		public CrudService (Identity identity, int? scope)
		{
			Repository = Injector.Resolve<TRepository> ();
			DtoMapper = Injector.Resolve<TMapper> ();
			Identity = identity;
			Scope = scope;
		}

		protected virtual TDto Map (TModel model)
		{ 
			return DtoMapper.Map (model, Identity, Scope);
		}

		protected virtual TModel Map (TDto dto)
		{ 
			return DtoMapper.Map (dto, Identity);
		}

		public virtual async Task<TDto> CreateAsync (TDto dto)
		{
			var model = DtoMapper.Map (dto, Identity);
			await Repository.CreateAsync (model);
			return DtoMapper.Map (model, Identity);
		}

		public virtual async Task<IList<TDto>> ReadAsync (int skip, int take)
		{
			var models = await Query.Skip (skip).Take (take).ToListAsync ();

			return !models.Any () ? 
				Enumerable.Empty<TDto> ().ToList () : 
				models.Select (Map).ToList ();

		}

		public virtual async Task<TDto> ReadAsync (int id)
		{
			var model = await Repository.ReadAsync (id);

			return model == null ? 
				null : 
				Map (model);
		}

		public virtual async Task<TDto> UpdateAsync (TDto dto)
		{
			var model = await Repository.ReadAsync (dto.Id);
			if (model == null)
				throw new ServiceUpdateException ();

			DtoMapper.MapForUpdate (dto, model, Identity, Scope);
			await Repository.UpdateAsync (model);
			return DtoMapper.Map (model, Identity, Scope);
		}

		public virtual async Task DeleteAsync (int id)
		{
			var model = await Repository.ReadAsync (id);
			if (model == null)
				throw new ServiceDeleteException ();
			await Repository.DeleteAsync (model);
		}

		public void Dispose ()
		{
			if (Repository != null)
				Repository.Dispose ();
		}
	}

	public class ServiceUpdateException : Exception
	{

	}

	public class ServiceDeleteException : Exception
	{

	}
}

