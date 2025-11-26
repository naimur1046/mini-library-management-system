using MiniLibrary.Domain.Users;
using MiniLibrary.Application.Abstractions.Messaging;
using MiniLibrary.Application.Users.Register;
using MiniLibrary.API.Extensions;
using MiniLibrary.API.Infrastructure;
using MiniLibrary.SharedKernel;

namespace MiniLibrary.API.Endpoints.Authentication;

internal sealed class Register : IEndpoint
{
    private sealed class RegisterRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public Role Role { get; set; } = Role.User;
    }

    private sealed class RegisterResponse
    {
        public Guid Id { get; set; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("authentication/register", async (
            RegisterRequest request,
            ICommandHandler<RegisterCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new RegisterCommand
            {
                FullName = request.FullName,
                Email = request.Email,
                Password = request.Password,
                Role = request.Role
            };

            Result<Guid> result = await handler.Handle(command, cancellationToken);

            return result.Match(
                onSuccess: id => Results.Created($"/api/v1/users/{id}", new RegisterResponse { Id = id }),
                onFailure: CustomResults.Problem);
        })
        .RequireAuthorization(policy => policy.RequireRole("Admin"))
        .WithName("Register")
        .WithTags(Tags.Authentication)
        .WithOpenApi()
        .Produces<RegisterResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status409Conflict)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden);
    }
}
