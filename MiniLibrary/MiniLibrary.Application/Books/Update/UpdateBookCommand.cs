using MiniLibrary.Application.Abstractions.Messaging;

namespace MiniLibrary.Application.Books.Update;

public class UpdateBookCommand : ICommand<Guid>
{
    public Guid BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int CopiesAvailable { get; set; }
    public int PublishedYear { get; set; }
}