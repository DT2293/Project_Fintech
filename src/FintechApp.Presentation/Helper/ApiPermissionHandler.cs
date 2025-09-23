using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FintechApp.Domain.Entities;
using FintechApp.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace FintechApp.Application.Interfaces
{

    public class ApiPermissionHandler : AuthorizationHandler<IAuthorizationRequirement>
    {
        private readonly IApiPermissionRepository _apiPermissionRepo;

        public ApiPermissionHandler(IApiPermissionRepository apiPermissionRepo)
        {
            _apiPermissionRepo = apiPermissionRepo;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            IAuthorizationRequirement requirement)
        {
            var httpContext = (context.Resource as DefaultHttpContext)
                              ?? (context.Resource as AuthorizationFilterContext)?.HttpContext;

            if (httpContext == null)
            {
                context.Fail();
                return;
            }

            var routeData = httpContext.GetRouteData();
            var controller = routeData.Values["controller"]?.ToString();
            var action = routeData.Values["action"]?.ToString();

            if (controller == null || action == null)
            {
                context.Fail();
                return;
            }

            var permission = await _apiPermissionRepo.GetPermissionAsync(controller, action);
            if (permission == null)
            {
                context.Fail();
                return;
            }

            var userPermissions = httpContext.User.FindAll("permissions").Select(c => c.Value);

            if (userPermissions.Contains(permission.Name))
                context.Succeed(requirement);
            else
                context.Fail();
        }
    }

}
