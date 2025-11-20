using MiniLibrary.Application.Abstractions.Messaging;

namespace MiniLibrary.Application.Members.Get;

public sealed record GetMembersQuery(
    Guid? LastMemberId,
    int? Size,
    string? Direction) : IQuery<PagedMembersResponse>;