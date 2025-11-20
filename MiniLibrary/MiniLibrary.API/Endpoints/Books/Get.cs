using MiniLibrary.Application.Abstractions.Messaging;
using MiniLibrary.Application.Books.Get;
using MiniLibrary.API.Extensions;
using MiniLibrary.API.Infrastructure;
using SharedKernel;

namespace MiniLibrary.API.Endpoints.Books;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("books", async (
            Guid? lastBookId,
            int? size,
            string? direction,
            IQueryHandler<GetBooksQuery, PagedBooksResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetBooksQuery(lastBookId, size, direction);

            Result<PagedBooksResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithName("GetBooks")
        .WithTags(Tags.Books)
        .WithOpenApi()
        .Produces<PagedBooksResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}