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
                Console.WriteLine("HttpContext is null");
                context.Fail();
                return;
            }

            var routeData = httpContext.GetRouteData();
            var controller = routeData.Values["controller"]?.ToString();
            var action = routeData.Values["action"]?.ToString();

            Console.WriteLine($"[DEBUG] Controller: {controller}, Action: {action}");

            if (controller == null || action == null)
            {
                Console.WriteLine("[DEBUG] Controller or Action is null");
                context.Fail();
                return;
            }

            var permission = await _apiPermissionRepo.GetPermissionAsync(controller, action);

            if (permission == null)
            {
                Console.WriteLine($"[DEBUG] No permission found in DB for {controller}.{action}");
                context.Fail();
                return;
            }

            Console.WriteLine($"[DEBUG] Permission from DB: {permission.Name}");

            var userPermissions = httpContext.User.FindAll("permissions").Select(c => c.Value).ToList();

            Console.WriteLine("[DEBUG] User permissions from JWT:");
            foreach (var p in userPermissions)
                Console.WriteLine($" - {p}");

            if (userPermissions.Contains(permission.Name))
            {
                Console.WriteLine("[DEBUG] Permission match! Authorization succeeded.");
                context.Succeed(requirement);
            }
            else
            {
                Console.WriteLine("[DEBUG] Permission mismatch! Authorization failed.");
                context.Fail();
            }
        }

    }

}
