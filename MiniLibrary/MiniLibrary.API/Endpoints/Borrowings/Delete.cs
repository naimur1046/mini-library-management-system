using MiniLibrary.API.Extensions;
using MiniLibrary.Application.Borrowings.Delete;
using MiniLibrary.Application.Abstractions.Messaging;
using MiniLibrary.API.Infrastructure;
using SharedKernel;

namespace MiniLibrary.API.Endpoints.Borrowings;

public sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("borrowings/{id:guid}", async (
                Guid id,
                ICommandHandler<DeleteBorrowingsCommand, Guid> handler,
                CancellationToken cancellationToken) =>
            {
                var command = new DeleteBorrowingsCommand(id);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(
                    onSuccess: () => Results.NoContent(),
                    onFailure: CustomResults.Problem);
            })
            .RequireAuthorization("AdminOnly")
            .WithName("DeleteBorrowings")
            .WithTags(Tags.Borrowings)
            .WithOpenApi()
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }
}