using MiniLibrary.Application.Abstractions.Messaging;

namespace MiniLibrary.Application.Borrowings.GetSummary;

public sealed record GetBorrowingSummaryQuery(
    DateTime StartDate,
    DateTime EndDate) : IQuery<BorrowingSummaryResponse>;
