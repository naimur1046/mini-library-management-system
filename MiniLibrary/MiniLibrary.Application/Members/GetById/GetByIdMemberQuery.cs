using MiniLibrary.Application.Abstractions.Messaging;
using MiniLibrary.Application.Members.Get;

namespace MiniLibrary.Application.Members.GetById;

public sealed record GetByIdMemberQuery(Guid MemberId) : IQuery<MemberResponse>;