using MasDev.Data;
using System.Threading.Tasks;
using System;
using AutoMapper;

namespace MasDev.Services.Modeling
{
	public interface ICommunicationMapper <TDto, TModel>
		where TDto : IDto
		where TModel : IModel
	{
		bool IsAsync { get; }

		TModel Map (TDto dto, IContext context);

		TDto Map (TModel model, IContext context);

		void MapForUpdate (TDto dto, TModel model, IContext context);

		Task<TModel> MapAsync (TDto dto, IContext context);

		Task<TDto> MapAsync (TModel model, IContext context);

		Task MapForUpdateAsync (TDto dto, TModel model, IContext context);
	}

	public abstract class CommunicationMapper <TDto, TModel> : ICommunicationMapper<TDto, TModel>
		where TDto : IDto
		where TModel : IModel
	{
		public bool IsAsync { get { return false; } }

		CommunicationMapper<TDto, TModel> This { get { return this; } }

		protected abstract TModel Map (TDto dto, IContext context);

		protected abstract TDto Map (TModel model, IContext context);

		TModel ICommunicationMapper<TDto,TModel>.Map (TDto dto, IContext context)
		{
			return This.Map (dto, context);
		}

		TDto ICommunicationMapper<TDto,TModel>.Map (TModel model, IContext context)
		{
			return This.Map (model, context);
		}

		public virtual void MapForUpdate (TDto dto, TModel model, IContext context)
		{
			Mapper.DynamicMap (dto, model);
		}

		public Task<TModel> MapAsync (TDto dto, IContext context)
		{
			throw new NotSupportedException ("Asyncronous mapping not supported");
		}

		public Task<TDto> MapAsync (TModel model, IContext context)
		{
			throw new NotSupportedException ("Asyncronous mapping not supported");
		}

		public Task MapForUpdateAsync (TDto dto, TModel model, IContext context)
		{
			throw new NotSupportedException ("Asyncronous mapping not supported");
		}
	}

	public abstract class AsyncCommunicationMapper <TDto, TModel> : ICommunicationMapper<TDto, TModel>
		where TDto : IDto
		where TModel : IModel
	{
		public bool IsAsync { get { return true; } }

		AsyncCommunicationMapper<TDto, TModel> This { get { return this; } }

		protected abstract Task<TModel> MapAsync (TDto dto, IContext context);

		protected abstract Task<TDto> MapAsync (TModel model, IContext context);

		protected abstract Task MapForUpdateAsync (TDto dto, TModel model, IContext context);

		public TModel Map (TDto dto, IContext context)
		{
			throw new NotSupportedException ("Syncronous mapping not supported");
		}

		public TDto Map (TModel model, IContext context)
		{
			throw new NotSupportedException ("Syncronous mapping not supported");
		}

		public void MapForUpdate (TDto dto, TModel model, IContext context)
		{
			throw new NotSupportedException ("Syncronous mapping not supported");
		}

		async Task<TModel> ICommunicationMapper<TDto,TModel>.MapAsync (TDto dto, IContext context)
		{
			return await This.MapAsync (dto, context);
		}

		async Task<TDto> ICommunicationMapper<TDto,TModel>.MapAsync (TModel model, IContext context)
		{
			return await This.MapAsync (model, context);
		}

		async Task ICommunicationMapper<TDto,TModel>.MapForUpdateAsync (TDto dto, TModel model, IContext context)
		{
			await This.MapForUpdateAsync (dto, model, context);
		}
	}
}

