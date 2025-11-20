using MiniLibrary.API.Extensions;
using MiniLibrary.Application.Borrowings.Create;
using MiniLibrary.Application.Abstractions.Messaging;
using MiniLibrary.API.Infrastructure;
using SharedKernel;

namespace MiniLibrary.API.Endpoints.Borrowings;

public sealed class Create : IEndpoint
{
    public sealed class CreateBorrowingsRequest
    {
        public Guid MemberId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public List<Guid> BookIds { get; set; } = new();
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("borrowings", async (
                CreateBorrowingsRequest request,
                ICommandHandler<CreateBorrowingsCommand, Guid> handler,
                CancellationToken cancellationToken) =>
            {
                var command = new CreateBorrowingsCommand
                {
                    MemberId =  request.MemberId,
                    BorrowDate = request.BorrowDate,
                    DueDate = request.DueDate,
                    BookIds = request.BookIds
                };

                Result<Guid> result = await handler.Handle(command, cancellationToken);

                return result.Match(
                    onSuccess: id => Results.Created($"borrowings/{id}", id),
                    onFailure: CustomResults.Problem
                );
            })
            .RequireAuthorization("AdminOnly")
            .WithName("CreateBorrowing")
            .WithTags(Tags.Borrowings)
            .WithOpenApi()
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
