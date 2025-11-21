using MiniLibrary.Application.Abstractions.Messaging;

namespace MiniLibrary.Application.Books.Get;

public sealed record GetBooksQuery(
    Guid? LastBookId,
    int? Size,
    string? Direction) : IQuery<PagedBooksResponse>;
