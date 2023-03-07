using System.Threading.Tasks;
using DenimERP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DenimERP.Security
{
    public static class HttpContextExtensions
    {
        public static async Task RefreshLoginAsync(this HttpContext context)
        {
            if (context.User == null)
                return;

            // The example uses base class, IdentityUser, yours may be called ApplicationUser if you have added any extra fields to the model
            var userManager = context.RequestServices
                .GetRequiredService<UserManager<ApplicationUser>>();
            var signInManager = context.RequestServices
                .GetRequiredService<SignInManager<ApplicationUser>>();

            var user = await userManager.GetUserAsync(context.User);

            if (signInManager.IsSignedIn(context.User))
            {
                await userManager.UpdateAsync(user);
                await signInManager.RefreshSignInAsync(user);
                await userManager.UpdateSecurityStampAsync(user);
            }
        }
    }
}
