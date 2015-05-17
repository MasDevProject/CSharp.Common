using MasDev.Services.Test.Communication;
using MasDev.Services.Owin.WebApi;
using System.Web.Http;
using System.Threading.Tasks;
using MasDev.Services.Test.Services;

namespace MasDev.Services.Test
{
	public class UserController : CrudApiController<UserDto, IUserService>
	{
		#region Crud

		[Route (UserEndpoints.Base)]
		public override async Task<IHttpActionResult> CreateAsync (UserDto dto)
		{
			return await base.CreateAsync (dto);
		}

		[Route (UserEndpoints.Base)]
		public override async Task<IHttpActionResult> ReadAsync (int skip, int take)
		{
			return await base.ReadAsync (skip, take);
		}

		[Route (UserEndpoints.Resource)]
		public override async Task<IHttpActionResult> ReadAsync (int id)
		{
			return await base.ReadAsync (id);
		}

		[Route (UserEndpoints.Resource)]
		public override async Task<IHttpActionResult> UpdateAsync (UserDto dto)
		{
			return await base.UpdateAsync (dto);
		}

		[Route (UserEndpoints.Resource)]
		public override async Task<IHttpActionResult> DeleteAsync (int id)
		{
			return await base.DeleteAsync (id);
		}

		#endregion

		[HttpPost]
		[Route (UserEndpoints.Login)]
		public async Task<IHttpActionResult> LoginAsync (LoginRequest request)
		{
			if (request == null)
				return BadRequest ();
			
			var result = await Service.LoginAsync (request.Username, request.Password, IdentityContext);
			if (result == null)
				return BadRequest ();
			return Ok (result);
		}
	}
}

