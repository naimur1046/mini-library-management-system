using MiniLibrary.Application.Abstractions.Messaging;

namespace MiniLibrary.Application.Borrowings.GetOverdue;

public sealed record GetOverdueBorrowingsQuery : IQuery<List<OverdueBorrowingResponse>>;
