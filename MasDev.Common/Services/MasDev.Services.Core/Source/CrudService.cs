using MasDev.Services.Modeling;
using MasDev.Data;
using MasDev.Patterns.Injection;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;
using MasDev.Services.DataAccess;

namespace MasDev.Services
{
	public class CrudService<TDto, TModel, TRepository> : ICrudService<TDto>
		where TDto: class, IDto
		where TModel : class, IModel, new()
		where TRepository : class, IRepository<TModel>
	{
		#region Properties

		protected TRepository Repository { get { return Injector.Resolve<TRepository> (); } }

		protected ICommunicationMapper<TDto, TModel> CommunicationMapper { get { return Injector.Resolve <ICommunicationMapper<TDto, TModel>> (); } }

		protected IConsistencyValidator<TDto> ConsistencyValidator { get { return Injector.Resolve<IConsistencyValidator<TDto>> (); } }

		protected IDataAccessValidator<TDto> DataAccessValidator { get { return Injector.Resolve<IDataAccessValidator<TDto>> (); } }

		#endregion

		protected virtual IQueryable<TModel> Query (IIdentityContext context)
		{
			return Repository.Query;
		}

		public virtual async Task<TDto> CreateAsync (TDto dto, IIdentityContext context)
		{
			await ValidateDataAccessAsync (dto, context);
			await ValidateConsistencyAsync (dto, context);
			var model = await MapAsync (dto, context);
			await Repository.CreateAsync (model);
			return await MapAsync (model, context);
		}

		public virtual async Task<IList<TDto>> ReadAsync (int skip, int take, IIdentityContext context)
		{
			var models = await Query (context).Skip (skip).Take (take).ToListAsync ();

			if (!models.Any ())
				return Enumerable.Empty<TDto> ().ToList ();

			var conversionTasks = models.Select (m => MapAsync (m, context)).ToList ();
			await Task.WhenAll (conversionTasks);

			var dtos = conversionTasks.Select (t => t.Result).ToList ();

			if (DataAccessValidator != null) {
				var tmpDtos = new List<TDto> ();
				foreach (var dto in dtos) {
					if (await CanAccessAsync (dto, context))
						tmpDtos.Add (dto);
				}
				dtos = tmpDtos;
			}

			return dtos;
		}

		public virtual async Task<TDto> ReadAsync (int id, IIdentityContext context)
		{
			await ValidateDataAccessAsync (id, context);

			var model = await Repository.ReadAsync (id);
			if (model == null)
				throw new ServiceReadException (id);

			return await MapAsync (model, context);
		}

		public virtual async Task<TDto> UpdateAsync (TDto dto, IIdentityContext context)
		{
			await ValidateDataAccessAsync (dto, context);
			await ValidateConsistencyAsync (dto, context);

			var model = await Repository.ReadAsync (dto.Id);
			if (model == null)
				throw new ServiceReadException (dto.Id);

			if (CommunicationMapper.IsAsync)
				await CommunicationMapper.MapForUpdateAsync (dto, model, context);
			else
				CommunicationMapper.MapForUpdate (dto, model, context);
			
			await Repository.UpdateAsync (model);
			return await MapAsync (model, context);
		}

		public virtual async Task DeleteAsync (int id, IIdentityContext context)
		{
			await ValidateDataAccessAsync (id, context);

			var model = await Repository.ReadAsync (id);
			if (model == null)
				throw new ServiceReadException (id);
			
			await Repository.DeleteAsync (model);
		}

		#region Protected methods

		protected virtual async Task<TDto> MapAsync (TModel model, IIdentityContext context)
		{
			return CommunicationMapper.IsAsync ? 
				await CommunicationMapper.MapAsync (model, context) : 
				CommunicationMapper.Map (model, context);
		}

		protected virtual async Task<TModel> MapAsync (TDto dto, IIdentityContext context)
		{
			if (ConsistencyValidator.IsAsync)
				await ConsistencyValidator.ValidateAsync (dto, context);
			else
				ConsistencyValidator.Validate (dto, context);

			return CommunicationMapper.IsAsync ? 
				await CommunicationMapper.MapAsync (dto, context) : 
				CommunicationMapper.Map (dto, context);
		}

		protected virtual async Task ValidateConsistencyAsync (TDto dto, IIdentityContext context)
		{
			if (ConsistencyValidator == null)
				return;

			if (ConsistencyValidator.IsAsync)
				await ConsistencyValidator.ValidateAsync (dto, context);
			else
				ConsistencyValidator.Validate (dto, context);
		}

		protected virtual async Task<bool> CanAccessAsync (TDto dto, IIdentityContext context)
		{
			if (DataAccessValidator == null)
				return true;

			return DataAccessValidator.IsAsync ? 
				await DataAccessValidator.CanAccessAsync (dto, context) : 
				DataAccessValidator.CanAccess (dto, context);
		}

		protected virtual async Task<bool> CanAccessAsync (int id, IIdentityContext context)
		{
			if (DataAccessValidator == null)
				return true;

			return DataAccessValidator.IsAsync ? 
				await DataAccessValidator.CanAccessAsync (id, context) : 
				DataAccessValidator.CanAccess (id, context);
		}

		protected async Task ValidateDataAccessAsync (TDto dto, IIdentityContext context)
		{
			if (!await CanAccessAsync (dto, context))
				throw new DataAccessAuthorizationException ();
		}

		protected async Task ValidateDataAccessAsync (int id, IIdentityContext context)
		{
			if (!await CanAccessAsync (id, context))
				throw new DataAccessAuthorizationException ();
		}

		#endregion
	}

	public class ServiceReadException : Exception
	{
		public ServiceReadException (int id) : base (string.Format ("Model with id {0} was not stored", id))
		{
			
		}
	}
}

