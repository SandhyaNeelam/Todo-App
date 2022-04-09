using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FullStack_ToDo.DTOs;

public record TodoAppDTO
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }
}

public record TodoAppCreateDTO
{
    [JsonPropertyName("user_id")]
    [Required]
    public int UserId { get; set; }

    [JsonPropertyName("title")]
    [Required]
    public string Title { get; set; }

    [JsonPropertyName("description")]
    [Required]
    public string Description { get; set; }
}

public record TodoAppUpdateDTO
{
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}