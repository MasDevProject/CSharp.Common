using MasDev.Data;
using System.Threading.Tasks;
using System;
using AutoMapper;
using MasDev.Common;

namespace MasDev.Services.Modeling
{
	public interface ICommunicationMapper <TDto, TModel>
		where TDto : IEntity
		where TModel : IModel
	{
		bool IsAsync { get; }

		TModel Map (TDto dto, ICallingContext context);

		TDto Map (TModel model, ICallingContext context);

		void MapForUpdate (TDto dto, TModel model, ICallingContext context);

		Task<TModel> MapAsync (TDto dto, ICallingContext context);

		Task<TDto> MapAsync (TModel model, ICallingContext context);

		Task MapForUpdateAsync (TDto dto, TModel model, ICallingContext context);
	}

	public abstract class CommunicationMapper <TDto, TModel> : ICommunicationMapper<TDto, TModel>
		where TDto : IEntity
		where TModel : IModel
	{
		public bool IsAsync { get { return false; } }

		CommunicationMapper<TDto, TModel> This { get { return this; } }

		protected abstract TModel Map (TDto dto, ICallingContext context);

		protected abstract TDto Map (TModel model, ICallingContext context);

		TModel ICommunicationMapper<TDto,TModel>.Map (TDto dto, ICallingContext context)
		{
			return This.Map (dto, context);
		}

		TDto ICommunicationMapper<TDto,TModel>.Map (TModel model, ICallingContext context)
		{
			return This.Map (model, context);
		}

		public virtual void MapForUpdate (TDto dto, TModel model, ICallingContext context)
		{
			Mapper.DynamicMap (dto, model);
		}

		public Task<TModel> MapAsync (TDto dto, ICallingContext context)
		{
			throw new NotSupportedException ("Asyncronous mapping not supported");
		}

		public Task<TDto> MapAsync (TModel model, ICallingContext context)
		{
			throw new NotSupportedException ("Asyncronous mapping not supported");
		}

		public Task MapForUpdateAsync (TDto dto, TModel model, ICallingContext context)
		{
			throw new NotSupportedException ("Asyncronous mapping not supported");
		}
	}

	public abstract class AsyncCommunicationMapper <TDto, TModel> : ICommunicationMapper<TDto, TModel>
		where TDto : IEntity
		where TModel : IModel
	{
		public bool IsAsync { get { return true; } }

		AsyncCommunicationMapper<TDto, TModel> This { get { return this; } }

		protected abstract Task<TModel> MapAsync (TDto dto, ICallingContext context);

		protected abstract Task<TDto> MapAsync (TModel model, ICallingContext context);

		protected abstract Task MapForUpdateAsync (TDto dto, TModel model, ICallingContext context);

		public TModel Map (TDto dto, ICallingContext context)
		{
			throw new NotSupportedException ("Syncronous mapping not supported");
		}

		public TDto Map (TModel model, ICallingContext context)
		{
			throw new NotSupportedException ("Syncronous mapping not supported");
		}

		public void MapForUpdate (TDto dto, TModel model, ICallingContext context)
		{
			throw new NotSupportedException ("Syncronous mapping not supported");
		}

		async Task<TModel> ICommunicationMapper<TDto,TModel>.MapAsync (TDto dto, ICallingContext context)
		{
			return await This.MapAsync (dto, context);
		}

		async Task<TDto> ICommunicationMapper<TDto,TModel>.MapAsync (TModel model, ICallingContext context)
		{
			return await This.MapAsync (model, context);
		}

		async Task ICommunicationMapper<TDto,TModel>.MapForUpdateAsync (TDto dto, TModel model, ICallingContext context)
		{
			await This.MapForUpdateAsync (dto, model, context);
		}
	}
}

