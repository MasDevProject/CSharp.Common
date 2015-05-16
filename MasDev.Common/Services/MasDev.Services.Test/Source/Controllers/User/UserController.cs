using System;
using MasDev.Services.Test.Communication;
using MasDev.Services.Owin.WebApi;
using System.Web.Http;
using System.Threading.Tasks;

namespace MasDev.Services.Test
{
	public class UserController : CrudApiController<UserDto>
	{
		[Route (UserEndpoints.Create)]
		public override async Task<IHttpActionResult> CreateAsync (UserDto dto)
		{
			return await base.CreateAsync (dto);
		}

		[Route (UserEndpoints.ReadMany)]
		public override async Task<IHttpActionResult> ReadAsync (int skip, int take)
		{
			return await base.ReadAsync (skip, take);
		}

		[Route (UserEndpoints.Read)]
		public override async Task<IHttpActionResult> ReadAsync (int id)
		{
			return await base.ReadAsync (id);
		}

		[Route (UserEndpoints.Update)]
		public override async Task<IHttpActionResult> UpdateAsync (UserDto dto)
		{
			return await base.UpdateAsync (dto);
		}

		[Route (UserEndpoints.Delete)]
		public override async Task<IHttpActionResult> DeleteAsync (int id)
		{
			return await base.DeleteAsync (id);
		}
	}
}

