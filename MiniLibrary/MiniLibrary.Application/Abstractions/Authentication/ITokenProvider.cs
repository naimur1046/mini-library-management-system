using MiniLibrary.Domain.Users;

namespace MiniLibrary.Application.Abstractions.Authentication;

public interface ITokenProvider
{
    string GenerateToken(Guid userId, string email, Role role);
}
