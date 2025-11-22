using MiniLibrary.Application.Abstractions.Messaging;
using MiniLibrary.Application.Users.Login;
using MiniLibrary.API.Extensions;
using MiniLibrary.API.Infrastructure;
using MiniLibrary.SharedKernel;

namespace MiniLibrary.API.Endpoints.Authentication;

internal sealed class Login : IEndpoint
{
    public sealed class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public sealed class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("authentication/login", async (
            LoginRequest request,
            ICommandHandler<LoginCommand, string> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new LoginCommand
            {
                Email = request.Email,
                Password = request.Password
            };

            Result<string> result = await handler.Handle(command, cancellationToken);

            return result.Match(
                onSuccess: token => Results.Ok(new LoginResponse { Token = token }),
                onFailure: CustomResults.Problem);
        })
        .WithName("Login")
        .WithTags(Tags.Authentication)
        .WithOpenApi()
        .Produces<LoginResponse>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status401Unauthorized);
    }
}