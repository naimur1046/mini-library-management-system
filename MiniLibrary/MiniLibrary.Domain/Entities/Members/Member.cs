using System.Collections;
using MiniLibrary.Domain.Common;

namespace MiniLibrary.Domain.Entities.Members;

public class Member : BaseEntity
{
    private const int MaxActiveBorrowings = 5;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime JoinDate { get; private set; }
    public bool IsActive { get; private set; }
}