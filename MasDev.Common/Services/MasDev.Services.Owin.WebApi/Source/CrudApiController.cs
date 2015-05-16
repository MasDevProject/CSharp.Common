using System;
using System.Web.Http;
using MasDev.Patterns.Injection;
using System.Threading.Tasks;
using MasDev.Services.Modeling;

namespace MasDev.Services.Owin.WebApi
{
	public class CrudApiController<TDto> : BaseApiController where TDto : class, IDto
	{
		protected readonly ICrudService<TDto> Service;

		public CrudApiController ()
		{
			Service = Injector.Resolve<ICrudService<TDto>> ();
		}

		[HttpPost]
		public virtual async Task<IHttpActionResult> CreateAsync (TDto dto)
		{
			dto = await Service.CreateAsync (dto, IdentityContext);
			return Created (dto);
		}

		[HttpGet]
		public virtual async Task<IHttpActionResult> ReadAsync (int skip, int take)
		{
			return Ok (await Service.ReadAsync (skip, take, IdentityContext));
		}

		[HttpGet]
		public virtual async Task<IHttpActionResult> ReadAsync (int id)
		{
			return Ok (await Service.ReadAsync (id, IdentityContext));
		}

		[HttpPut]
		public virtual async Task<IHttpActionResult> UpdateAsync (TDto dto)
		{
			return Ok (await Service.UpdateAsync (dto, IdentityContext));
		}

		[HttpDelete]
		public virtual async Task<IHttpActionResult> DeleteAsync (int id)
		{
			await Service.DeleteAsync (id, IdentityContext);
			return Ok ();
		}

		protected IHttpActionResult Created (TDto dto)
		{
			return Created (new Uri (Request.RequestUri, dto.Id.ToString ()), dto);
		}
	}
}

