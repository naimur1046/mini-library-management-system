using MiniLibrary.API.Extensions;
using MiniLibrary.API.Infrastructure;
using MiniLibrary.Application.Abstractions.Messaging;
using MiniLibrary.Application.Members.Delete;
using MiniLibrary.SharedKernel;

namespace MiniLibrary.API.Endpoints.Members;

public class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("members/{id:guid}", async (
                Guid id,
                ICommandHandler<DeleteMemberCommand, Guid> handler,
                CancellationToken cancellationToken) =>
            {
                var command = new DeleteMemberCommand(id);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(
                    onSuccess: () => Results.NoContent(),
                    onFailure: CustomResults.Problem);
            })
            .RequireAuthorization("AdminOnly")
            .WithName("DeleteMember")
            .WithTags(Tags.Members)
            .WithOpenApi()
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }
}