using MiniLibrary.Application.Abstractions.Messaging;
using MiniLibrary.Application.Borrowings.Return;
using MiniLibrary.API.Infrastructure;
using MiniLibrary.API.Extensions;

namespace MiniLibrary.API.Endpoints.Borrowings;

internal sealed class ReturnBook : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("borrowings/{borrowId}/return/{bookId}", async (
            Guid borrowId,
            Guid bookId,
            ICommandHandler<ReturnBookCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new ReturnBookCommand
            {
                BorrowId = borrowId,
                BookId = bookId
            };

            var result = await handler.Handle(command, cancellationToken);

            return result.Match(
                onSuccess: id => Results.Ok(id),
                onFailure: CustomResults.Problem
            );
        })
        .RequireAuthorization("UserOrAdmin")
        .WithName("ReturnBorrowing")
        .WithTags(Tags.Borrowings)
        .WithOpenApi()
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }

}