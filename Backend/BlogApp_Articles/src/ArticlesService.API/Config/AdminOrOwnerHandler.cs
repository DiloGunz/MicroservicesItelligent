using ArticlesService.Application.Models.Queries.ArticleQueries;
using ArticlesService.SharedKernel.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ArticlesService.API.Config;

public class AdminOrOwnerHandler : AuthorizationHandler<AdminOrOwnerRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ISender _mediator;

    public AdminOrOwnerHandler(IHttpContextAccessor httpContextAccessor, ISender mediator)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        AdminOrOwnerRequirement requirement)
    {
        var user = context.User;
        var httpContext = _httpContextAccessor.HttpContext;

        if (user.IsInRole(RoleConstants.Admin))
        {
            context.Succeed(requirement);
            return;
        }

        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return;

        var parseUserId = Convert.ToInt64(userId);

        var routeData = httpContext?.Request.RouteValues;

        if (routeData != null && routeData.TryGetValue("id", out var resourceId))
        {
            var query = new GetArticleByIdQuery()
            {
                Id = Convert.ToInt64(resourceId)
            };

            var data = await _mediator.Send(query);
            if (data.IsError)
            {
                return;
            }
            if (data.Value.CreatedBy == parseUserId)
            {
                context.Succeed(requirement);
                return;
            }
        }
    }
}