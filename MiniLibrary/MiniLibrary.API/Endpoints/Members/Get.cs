using MiniLibrary.Application.Members.Get;
using MiniLibrary.Application.Abstractions.Messaging;
using MiniLibrary.API.Extensions;
using MiniLibrary.API.Infrastructure;
using SharedKernel;

namespace MiniLibrary.API.Endpoints.Members;

public class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("members", async (
                Guid? lastMemberId,
                int? size,
                string? direction,
                IQueryHandler<GetMembersQuery, PagedMembersResponse> handler,
                CancellationToken cancellationToken) =>
            {
                var query = new GetMembersQuery(lastMemberId, size, direction);

                Result<PagedMembersResponse> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .RequireAuthorization("UserOrAdmin")
            .WithName("GetMembers")
            .WithTags(Tags.Members)
            .WithOpenApi()
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}