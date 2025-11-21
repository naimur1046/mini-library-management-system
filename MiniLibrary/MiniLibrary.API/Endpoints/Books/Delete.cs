using MiniLibrary.API.Extensions;
using MiniLibrary.Application.Abstractions.Messaging;
using MiniLibrary.API.Infrastructure;
using MiniLibrary.Application.Books.Delete;
using SharedKernel;

namespace MiniLibrary.API.Endpoints.Books;

public class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("books/{id:guid}", async (
                Guid id,
                ICommandHandler<DeleteBookCommand, Guid> handler,
                CancellationToken cancellationToken) =>
            {
                var command = new DeleteBookCommand(id);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(
                    onSuccess: () => Results.NoContent(),
                    onFailure: CustomResults.Problem);
            })
            .RequireAuthorization("AdminOnly")
            .WithName("DeleteBook")
            .WithTags(Tags.Books)
            .WithOpenApi()
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }
}