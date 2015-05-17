using MasDev.Services.Modeling;
using System.Threading.Tasks;
using System;

namespace MasDev.Services.Modeling
{
	public interface IConsistencyValidator<TDto> where TDto : IDto
	{
		void Validate (TDto dto, IIdentityContext context);

		Task ValidateAsync (TDto dto, IIdentityContext context);

		bool IsAsync { get; }
	}

	public abstract class ConsistencyValidator<TDto> : IConsistencyValidator<TDto> where TDto : IDto
	{
		protected abstract void Validate (TDto dto, IIdentityContext context);

		void IConsistencyValidator<TDto>.Validate (TDto dto, IIdentityContext context)
		{
			(this as ConsistencyValidator<TDto>).Validate (dto, context);
		}

		public Task ValidateAsync (TDto dto, IIdentityContext context)
		{
			throw new NotSupportedException ("Asyncronous validation not supported");
		}

		public bool IsAsync { get { return false; } }
	}

	public abstract class AsyncConsistencyValidator<TDto> : IConsistencyValidator<TDto> where TDto : IDto
	{
		protected abstract Task ValidateAsync (TDto dto, IIdentityContext context);

		public void Validate (TDto dto, IIdentityContext context)
		{
			throw new NotSupportedException ("Syncronous validation not supported");
		}

		async Task IConsistencyValidator<TDto>.ValidateAsync (TDto dto, IIdentityContext context)
		{
			await (this as AsyncConsistencyValidator<TDto>).ValidateAsync (dto, context);
		}

		public bool IsAsync { get { return true; } }
	}
}

