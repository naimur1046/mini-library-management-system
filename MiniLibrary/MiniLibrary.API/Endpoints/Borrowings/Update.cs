
namespace MiniLibrary.API.Endpoints.Borrowings;

public sealed class Update : IEndpoint
{
    public sealed class UpdateBorrowingsRequest
    {
        public Guid MemberId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public List<Guid> BookIds { get; set; } = new();
    }
    
    public sealed class  UpdateBookResponse
    {
        public Guid Id { get; set; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        
    }
}