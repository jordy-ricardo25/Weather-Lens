using System;
namespace Weather_Lens.Models;

public sealed class User
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string? Role { get; set; }
}
