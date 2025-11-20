namespace MiniLibrary.Application.Members.Get;

public class PagedMembersResponse
{
    public List<MemberResponse> Members { get; set; } = [];
    public bool HasMore { get; set; }
}