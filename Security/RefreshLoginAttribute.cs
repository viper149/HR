using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HRMS.Security
{
    public class RefreshLoginAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await context.HttpContext.RefreshLoginAsync();
            await next();
        }
    }
}
