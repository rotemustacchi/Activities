using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Infrastructure.Security
{
    public class IsHostRequirement: IAuthorizationRequirement
    {
    }

    public class IsHostRequirementHandler(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : AuthorizationHandler<IsHostRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsHostRequirement requirement)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return;

            var httpContext = contextAccessor.HttpContext;
            if (httpContext?.GetRouteValue("id") is not string activityId) return;

            var attendee = await dbContext.ActivityAttendees
                .AsNoTracking()
                .SingleOrDefaultAsync(x=>x.UserId==userId && x.ActivityId == activityId);

            if(attendee==null) return;
            
            if(attendee.IsHost) context.Succeed(requirement);
        }
    }
}
