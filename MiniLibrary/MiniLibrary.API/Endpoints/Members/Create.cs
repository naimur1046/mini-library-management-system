using MiniLibrary.Application.Abstractions.Messaging;
using MiniLibrary.Application.Members.Create;
using MiniLibrary.API.Extensions;
using MiniLibrary.API.Infrastructure;
using SharedKernel;

namespace MiniLibrary.API.Endpoints.Members;

internal sealed class Create : IEndpoint
{
    public sealed class Request
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime JoinDate { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public sealed class Response
    {
        public Guid Id { get; set; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("members", async (
                Request request,
                ICommandHandler<CreateMemberCommand, Guid> handler,
                CancellationToken cancellationToken) =>
            {
                var command = new CreateMemberCommand
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    Phone = request.Phone,
                    JoinDate = request.JoinDate,
                    IsActive = request.IsActive
                };

                Result<Guid> result = await handler.Handle(command, cancellationToken);

                return result.Match(
                    onSuccess: id => Results.Created($"/api/v1/members/{id}", new Response { Id = id }),
                    onFailure: CustomResults.Problem);
            })
            .RequireAuthorization("AdminOnly")
            .WithName("CreateMember")
            .WithTags(Tags.Members)
            .WithOpenApi()
            .Produces<Response>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status409Conflict);
    }
}