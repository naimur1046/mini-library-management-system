using MiniLibrary.Application.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;

namespace MiniLibrary.Infrastructure.Authentication;

internal sealed class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId =>
        _httpContextAccessor
            .HttpContext?
            .User
            .GetUserId() ??
        throw new ApplicationException("User context is unavailable");

    public string Email =>
        _httpContextAccessor
            .HttpContext?
            .User
            .GetEmail() ??
        throw new ApplicationException("User email is unavailable");
}
