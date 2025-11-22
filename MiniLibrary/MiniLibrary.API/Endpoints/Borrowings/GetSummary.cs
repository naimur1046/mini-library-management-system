using MiniLibrary.Application.Abstractions.Messaging;
using MiniLibrary.Application.Borrowings.GetSummary;
using MiniLibrary.API.Extensions;
using MiniLibrary.API.Infrastructure;
using MiniLibrary.SharedKernel;

namespace MiniLibrary.API.Endpoints.Borrowings;

internal sealed class GetSummary : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("borrowings/summary", async (
            DateTime startDate,
            DateTime endDate,
            IQueryHandler<GetBorrowingSummaryQuery, BorrowingSummaryResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetBorrowingSummaryQuery(startDate, endDate);

            Result<BorrowingSummaryResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .RequireAuthorization("UserOrAdmin")
        .WithName("GetBorrowingSummary")
        .WithTags(Tags.Borrowings)
        .WithOpenApi()
        .Produces<BorrowingSummaryResponse>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
