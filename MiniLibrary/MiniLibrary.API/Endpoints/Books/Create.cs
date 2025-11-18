using MiniLibrary.Application.Abstractions.Messaging;
using MiniLibrary.Application.Books.Create;
using MiniLibrary.API.Extensions;
using MiniLibrary.API.Infrastructure;
using SharedKernel;

namespace MiniLibrary.API.Endpoints.Books;

internal sealed class Create : IEndpoint
{
    public sealed class Request
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int CopiesAvailable { get; set; }
        public int PublishedYear { get; set; }
    }

    public sealed class Response
    {
        public Guid Id { get; set; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("books", async (
            Request request,
            ICommandHandler<CreateBookCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateBookCommand
            {
                Title = request.Title,
                Author = request.Author,
                ISBN = request.ISBN,
                Category = request.Category,
                CopiesAvailable = request.CopiesAvailable,
                PublishedYear = request.PublishedYear
            };

            Result<Guid> result = await handler.Handle(command, cancellationToken);

            return result.Match(
                onSuccess: id => Results.Created($"/api/v1/books/{id}", new Response { Id = id }),
                onFailure: CustomResults.Problem);
        })
        .WithName("CreateBook")
        .WithTags(Tags.Books)
        .WithOpenApi()
        .Produces<Response>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status409Conflict);
    }
}
