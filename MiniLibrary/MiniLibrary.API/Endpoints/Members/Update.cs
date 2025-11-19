using MiniLibrary.API.Extensions;
using MiniLibrary.Application.Members.Update;
using MiniLibrary.Application.Abstractions.Messaging;
using MiniLibrary.API.Infrastructure;
using SharedKernel;

namespace MiniLibrary.API.Endpoints.Members;

public class Update
{
    public sealed class UpdateMemberRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime JoinDate { get; set; }
        public bool IsActive { get; set; }

    }
    
    public sealed class UpdateMemberResponse
    {
        public Guid Id { get; set; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("members/{id:guid}", async (
                Guid id,
                UpdateMemberRequest request,
                ICommandHandler<UpdateMemberCommand, Guid> handler,
                CancellationToken cancellationToken) =>
                    {
                        var command = new UpdateMemberCommand
                        {
                            MemberId = id,
                            FullName = request.FullName,
                            Email = request.Email,
                            Phone = request.Phone,
                            JoinDate = request.JoinDate,
                            IsActive = request.IsActive
                        };

                        Result<Guid> result = await handler.Handle(command, cancellationToken);

                        return result.Match(
                            onSuccess: id => Results.Created($"/api/v1/members/{id}", new UpdateMemberResponse { Id = id }),
                            onFailure: CustomResults.Problem);
                    })
                    .RequireAuthorization("AdminOnly")
                    .WithName("UpdateMember")
                    .WithTags(Tags.Members)
                    .WithOpenApi()
                    .Produces(StatusCodes.Status204NoContent)
                    .ProducesValidationProblem()
                    .ProducesProblem(StatusCodes.Status404NotFound)
                    .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}