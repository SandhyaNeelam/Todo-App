using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using FullStack_ToDo.Models;

namespace FullStack_ToDo.DTOs;

public record UsersDTO
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }
}
public record UsersCreateDTO
{
    [JsonPropertyName("name")]
    [Required]
    public string Name { get; set; }

    [JsonPropertyName("email")]
    [Required]
    public string Email { get; set; }

    [JsonPropertyName("password")]
    [Required]
    public string Password { get; set; }
}
public record UsersLoginDTO
{
    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }
}