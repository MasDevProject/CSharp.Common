using MasDev.Services.Modeling;
using System.Threading.Tasks;
using System;

namespace MasDev.Services.Modeling
{
	public interface IValidator<TDto> where TDto : IDto
	{
		void Validate (TDto dto, IContext context);

		Task ValidateAsync (TDto dto, IContext context);

		bool IsAsync { get; }
	}

	public abstract class Validator<TDto> : IValidator<TDto> where TDto : IDto
	{
		protected abstract void Validate (TDto dto, IContext context);

		void IValidator<TDto>.Validate (TDto dto, IContext context)
		{
			(this as Validator<TDto>).Validate (dto, context);
		}

		public Task ValidateAsync (TDto dto, IContext context)
		{
			throw new NotSupportedException ("Asyncronous validation not supported");
		}

		public bool IsAsync { get { return false; } }
	}

	public abstract class AsyncValidator<TDto> : IValidator<TDto> where TDto : IDto
	{
		protected abstract Task ValidateAsync (TDto dto, IContext context);

		public void Validate (TDto dto, IContext context)
		{
			throw new NotSupportedException ("Syncronous validation not supported");
		}

		async Task IValidator<TDto>.ValidateAsync (TDto dto, IContext context)
		{
			await (this as AsyncValidator<TDto>).ValidateAsync (dto, context);
		}

		public bool IsAsync { get { return true; } }
	}
}

