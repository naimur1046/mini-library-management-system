using MiniLibrary.Application.Abstractions.Messaging;
using MiniLibrary.Application.Books.Get;
using MiniLibrary.Application.Books.GetById;
using MiniLibrary.API.Extensions;
using MiniLibrary.API.Infrastructure;
using SharedKernel;

namespace MiniLibrary.API.Endpoints.Books;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("books/{id:guid}", async (
            Guid id,
            IQueryHandler<GetByIdBookQuery, BookResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetByIdBookQuery(id);

            Result<BookResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithName("GetBookById")
        .WithTags(Tags.Books)
        .WithOpenApi()
        .Produces<BookResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}