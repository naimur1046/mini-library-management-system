using MiniLibrary.Application.Abstractions.Messaging;

namespace MiniLibrary.Application.Books.Update;

public class UpdateBookCommand : ICommand<Guid>
{
    public Guid BookId { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? ISBN { get; set; }
    public string? Category { get; set; }
    public int? CopiesAvailable { get; set; }
    public int? PublishedYear { get; set; }
}