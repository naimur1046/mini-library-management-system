using MiniLibrary.Application.Abstractions.Messaging;
using MiniLibrary.Application.Books.Get;

namespace MiniLibrary.Application.Books.GetById;

public sealed record GetByIdBookQuery(Guid BookId) : IQuery<BookResponse>;