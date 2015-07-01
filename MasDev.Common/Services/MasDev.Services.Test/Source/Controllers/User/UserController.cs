using MasDev.Services.Test.Communication;
using MasDev.Services.Owin.WebApi;
using System.Web.Http;
using System.Threading.Tasks;
using MasDev.Services.Test.Services;
using MasDev.Services.Modeling;

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
		public override async Task<IHttpActionResult> ReadPagedAsync (int skip, int take)
		{
			return await base.ReadPagedAsync (skip, take);
		}

		[Route (UserEndpoints.Resource)]
		public override async Task<IHttpActionResult> ReadPagedAsync (int id)
		{
			return await base.ReadPagedAsync (id);
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
		public async Task<LoginResult<UserDto>> LoginAsync (LoginRequest request)
		{			
			return await Service.LoginAsync (request.Username, request.Password);
		}
	}
}

