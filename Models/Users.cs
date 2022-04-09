using FullStack_ToDo.DTOs;

namespace FullStack_ToDo.Models;

public record Users
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }


    public UsersDTO asDto => new UsersDTO
    {
        Id= Id,
        Name = Name,
        Email = Email,
        Password = Password,
    };
}