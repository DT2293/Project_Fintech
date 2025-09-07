using FintechApp.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FintechApp.Presentation.Helper
{
    public class PermissionAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (descriptor == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            var controller = descriptor.ControllerName;
            var action = descriptor.ActionName;

            var user = context.HttpContext.User;
            if (!user.Identity?.IsAuthenticated ?? true)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var permissionService = context.HttpContext.RequestServices.GetRequiredService<IPermissionService>();

            var hasPermission = await permissionService.HasPermissionAsync(userId, controller, action);
            if (!hasPermission)
            {
                context.Result = new ForbidResult();
            }
        }
    }

}
