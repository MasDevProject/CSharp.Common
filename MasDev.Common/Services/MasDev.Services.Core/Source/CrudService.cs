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
		protected TRepository Repository { get { return Injector.Resolve<TRepository> (); } }

		protected ICommunicationMapper<TDto, TModel> CommunicationMapper { get { return Injector.Resolve <ICommunicationMapper<TDto, TModel>> (); } }

		protected IValidator<TDto> Validator { get { return Injector.Resolve<IValidator<TDto>> (); } }

		protected virtual IQueryable<TModel> Query (IIdentityContext context)
		{
			return Repository.Query;
		}

		public virtual async Task<TDto> CreateAsync (TDto dto, IIdentityContext context)
		{
			var model = await Map (dto, context);
			await Repository.CreateAsync (model);
			return await Map (model, context);
		}

		public virtual async Task<IList<TDto>> ReadAsync (int skip, int take, IIdentityContext context)
		{
			var models = await Query (context).Skip (skip).Take (take).ToListAsync ();

			if (!models.Any ())
				return Enumerable.Empty<TDto> ().ToList ();

			var conversionTasks = models.Select (m => Map (m, context)).ToList ();
			await Task.WhenAll (conversionTasks);
			return conversionTasks.Select (t => t.Result).ToList ();
		}

		public virtual async Task<TDto> ReadAsync (int id, IIdentityContext context)
		{
			var model = await Repository.ReadAsync (id);

			if (model == null)
				return null;

			return await Map (model, context);
		}

		public virtual async Task<TDto> UpdateAsync (TDto dto, IIdentityContext context)
		{
			var model = await Repository.ReadAsync (dto.Id);
			if (model == null)
				throw new ServiceUpdateException ();

			if (CommunicationMapper.IsAsync)
				await CommunicationMapper.MapForUpdateAsync (dto, model, context);
			else
				CommunicationMapper.MapForUpdate (dto, model, context);
			
			await Repository.UpdateAsync (model);
			return await Map (model, context);
		}

		public virtual async Task DeleteAsync (int id, IIdentityContext context)
		{
			var model = await Repository.ReadAsync (id);
			if (model == null)
				throw new ServiceDeleteException ();
			await Repository.DeleteAsync (model);
		}

		protected virtual async Task<TDto> Map (TModel model, IIdentityContext context)
		{
			return CommunicationMapper.IsAsync ? 
				await CommunicationMapper.MapAsync (model, context) : 
				CommunicationMapper.Map (model, context);
		}

		protected virtual async Task<TModel> Map (TDto dto, IIdentityContext context)
		{
			if (Validator.IsAsync)
				await Validator.ValidateAsync (dto, context);
			else
				Validator.Validate (dto, context);

			return CommunicationMapper.IsAsync ? 
				await CommunicationMapper.MapAsync (dto, context) : 
				CommunicationMapper.Map (dto, context);
		}
	}

	public class ServiceUpdateException : Exception
	{

	}

	public class ServiceDeleteException : Exception
	{

	}
}

