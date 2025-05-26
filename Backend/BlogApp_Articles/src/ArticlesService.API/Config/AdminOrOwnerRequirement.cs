using Microsoft.AspNetCore.Authorization;

namespace ArticlesService.API.Config;

public class AdminOrOwnerRequirement : IAuthorizationRequirement { }