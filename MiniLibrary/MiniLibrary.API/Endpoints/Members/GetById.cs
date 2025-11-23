using MiniLibrary.Application.Abstractions.Messaging;
using MiniLibrary.Application.Members.Get;
using MiniLibrary.Application.Members.GetById;
using MiniLibrary.API.Extensions;
using MiniLibrary.API.Infrastructure;
using MiniLibrary.SharedKernel;

namespace MiniLibrary.API.Endpoints.Members;

public class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("members/{id:guid}", async (
                Guid id,
                IQueryHandler<GetByIdMemberQuery, MemberResponse> handler,
                CancellationToken cancellationToken) =>
            {
                var query = new GetByIdMemberQuery(id);

                Result<MemberResponse> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .RequireAuthorization("UserOrAdmin")
            .WithName("GetMemberById")
            .WithTags(Tags.Members)
            .WithOpenApi()
            .Produces<MemberResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }
}