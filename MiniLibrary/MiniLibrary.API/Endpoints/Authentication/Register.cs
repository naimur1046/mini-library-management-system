using Domain.Users;
using MiniLibrary.Application.Abstractions.Messaging;
using MiniLibrary.Application.Users.Register;
using MiniLibrary.API.Extensions;
using MiniLibrary.API.Infrastructure;
using SharedKernel;

namespace MiniLibrary.API.Endpoints.Authentication;

internal sealed class Register : IEndpoint
{
    public sealed class Request
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public Role Role { get; set; } = Role.User;
    }

    public sealed class Response
    {
        public Guid Id { get; set; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("authentication/register", async (
            Request request,
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
                onSuccess: id => Results.Created($"/api/v1/users/{id}", new Response { Id = id }),
                onFailure: CustomResults.Problem);
        })
        .WithName("Register")
        .WithTags(Tags.Authentication)
        .WithOpenApi()
        .Produces<Response>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status409Conflict);
    }
}
