using MiniLibrary.Application.Abstractions.Messaging;

namespace MiniLibrary.Application.Books.Create;

public sealed class CreateBookCommand : ICommand<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int CopiesAvailable { get; set; }
    public int PublishedYear { get; set; }
}
