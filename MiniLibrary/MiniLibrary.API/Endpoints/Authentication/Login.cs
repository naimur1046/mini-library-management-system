using MiniLibrary.Application.Abstractions.Messaging;
using MiniLibrary.Application.Users.Login;
using MiniLibrary.API.Extensions;
using MiniLibrary.API.Infrastructure;
using SharedKernel;

namespace MiniLibrary.API.Endpoints.Authentication;

internal sealed class Login : IEndpoint
{
    public sealed class Request
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public sealed class Response
    {
        public string Token { get; set; } = string.Empty;
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("authentication/login", async (
            Request request,
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
                onSuccess: token => Results.Ok(new Response { Token = token }),
                onFailure: CustomResults.Problem);
        })
        .WithName("Login")
        .WithTags(Tags.Authentication)
        .WithOpenApi()
        .Produces<Response>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status401Unauthorized);
    }
}