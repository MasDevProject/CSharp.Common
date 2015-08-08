
using MasDev.Threading;
using System.Threading.Tasks;

namespace MasDev.Services.Auth
{
    public static class AuthorizationManagerExtensions
    {
        public static void Authorize(this IAuthorizationManager authManager, int? minimumRoles = null)
        {
            SingleThreadSynchronizationContext.Run(async () => await authManager.AuthorizeAsync(minimumRoles));
        }

        public static async Task<bool> IsAuthorizedAsync(this IAuthorizationManager authManager, int? minimumRoles = null)
        {
            try
            {
                await authManager.AuthorizeAsync(minimumRoles);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsAuthorized(this IAuthorizationManager authManager, int? minimumRoles = null)
        {
            try
            {
                authManager.Authorize(minimumRoles);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
