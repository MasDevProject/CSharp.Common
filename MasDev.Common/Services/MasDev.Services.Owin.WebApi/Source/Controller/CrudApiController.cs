using System;
using System.Web.Http;
using System.Threading.Tasks;
using MasDev.Common;

namespace MasDev.Services.Owin.WebApi
{
	public class CrudApiController<TDto, TCrudService> : ServiceApiController<TCrudService>
		where TDto : class, IEntity
		where TCrudService : class, ICrudService<TDto>
	{
		[HttpPost]
		public virtual async Task<IHttpActionResult> CreateAsync (TDto dto)
		{
			dto = await Service.CreateAsync (dto);
			return Created (dto);
		}

		[HttpGet]
		public virtual async Task<IHttpActionResult> ReadPagedAsync (int skip, int take)
		{
			return Ok (await Service.ReadPagedAsync (skip, take));
		}

		[HttpGet]
		public virtual async Task<IHttpActionResult> ReadPagedAsync (int id)
		{
			return Ok (await Service.ReadAsync (id));
		}

		[HttpPut]
		public virtual async Task<IHttpActionResult> UpdateAsync (TDto dto)
		{
			return Ok (await Service.UpdateAsync (dto));
		}

		[HttpDelete]
		public virtual async Task<IHttpActionResult> DeleteAsync (int id)
		{
			await Service.DeleteAsync (id);
			return Ok ();
		}

		protected IHttpActionResult Created (TDto dto)
		{
			return Created (new Uri (Request.RequestUri, dto.Id.ToString ()), dto);
		}
	}
}

