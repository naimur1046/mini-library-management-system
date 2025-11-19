using MiniLibrary.Application.Abstractions.Messaging;
using MiniLibrary.API.Extensions;
using MiniLibrary.Application.Books.Update;
using MiniLibrary.API.Infrastructure;
using SharedKernel;

namespace MiniLibrary.API.Endpoints.Books;

internal sealed class Update : IEndpoint
{
    public sealed class UpdateBookRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int CopiesAvailable { get; set; }
        public int PublishedYear { get; set; }
    }
    
    public sealed class  UpdateBookResponse
    {
        public Guid Id { get; set; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("books/{id:guid}", async (
                Guid id,
                UpdateBookRequest request,
                ICommandHandler<UpdateBookCommand, Guid> handler,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdateBookCommand
                {
                    BookId = id,
                    Title = request.Title,
                    Author = request.Author,
                    ISBN = request.ISBN,
                    Category = request.Category,
                    CopiesAvailable = request.CopiesAvailable,
                    PublishedYear = request.PublishedYear
                };

                Result<Guid> result = await handler.Handle(command, cancellationToken);

                return result.Match(
                    onSuccess: id => Results.Created($"/api/v1/books/{id}", new UpdateBookResponse { Id = id }),
                    onFailure: CustomResults.Problem);
            })
            .RequireAuthorization("AdminOnly")
            .WithName("UpdateBook")
            .WithTags(Tags.Books)
            .WithOpenApi()
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}