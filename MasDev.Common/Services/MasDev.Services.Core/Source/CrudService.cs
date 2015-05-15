using MasDev.Services.Modeling;
using MasDev.Data;
using MasDev.Patterns.Injection;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

namespace MasDev.Services
{
	public class CrudService<TDto, TModel, TRepository> : ICrudService<TDto>
		where TDto: class, IDto
		where TModel : class, IModel, new()
		where TRepository : class, IRepository<TModel>
	{
		protected TRepository Repository { get; private set; }

		protected ICommunicationMapper<TDto, TModel> CommunicationMapper { get; private set; }

		protected IValidator<TDto> Validator { get; private set; }

		protected IContext Context { get; private set; }

		protected virtual IQueryable<TModel> Query { get { return Repository.Query; } }

		public CrudService (IContext context)
		{
			Repository = Injector.Resolve<TRepository> ();
			CommunicationMapper = Injector.Resolve<ICommunicationMapper<TDto, TModel>> ();
			Validator = Injector.Resolve<IValidator<TDto>> ();
			Context = context;
		}

		public virtual async Task<TDto> CreateAsync (TDto dto)
		{
			var model = await Map (dto);
			await Repository.CreateAsync (model);
			return await Map (model);
		}

		public virtual async Task<IList<TDto>> ReadAsync (int skip, int take)
		{
			var models = await Query.Skip (skip).Take (take).ToListAsync ();

			if (!models.Any ())
				return Enumerable.Empty<TDto> ().ToList ();

			var conversionTasks = models.Select (Map).ToList ();
			await Task.WhenAll (conversionTasks);
			return conversionTasks.Select (t => t.Result).ToList ();
		}

		public virtual async Task<TDto> ReadAsync (int id)
		{
			var model = await Repository.ReadAsync (id);

			if (model == null)
				return null;

			return await Map (model);
		}

		public virtual async Task<TDto> UpdateAsync (TDto dto)
		{
			var model = await Repository.ReadAsync (dto.Id);
			if (model == null)
				throw new ServiceUpdateException ();

			if (CommunicationMapper.IsAsync)
				await CommunicationMapper.MapForUpdateAsync (dto, model, Context);
			else
				CommunicationMapper.MapForUpdate (dto, model, Context);
			
			await Repository.UpdateAsync (model);
			return await Map (model);
		}

		public virtual async Task DeleteAsync (int id)
		{
			var model = await Repository.ReadAsync (id);
			if (model == null)
				throw new ServiceDeleteException ();
			await Repository.DeleteAsync (model);
		}

		protected virtual async Task<TDto> Map (TModel model)
		{
			return CommunicationMapper.IsAsync ? 
				await CommunicationMapper.MapAsync (model, Context) : 
				CommunicationMapper.Map (model, Context);
		}

		protected virtual async Task<TModel> Map (TDto dto)
		{
			if (Validator.IsAsync)
				await Validator.ValidateAsync (dto, Context);
			else
				Validator.Validate (dto, Context);

			return CommunicationMapper.IsAsync ? 
				await CommunicationMapper.MapAsync (dto, Context) : 
				CommunicationMapper.Map (dto, Context);
		}
	}

	public class ServiceUpdateException : Exception
	{

	}

	public class ServiceDeleteException : Exception
	{

	}
}

