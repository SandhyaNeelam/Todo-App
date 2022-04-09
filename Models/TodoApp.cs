using FullStack_ToDo.DTOs;

namespace FullStack_ToDo.Models;
 public record TodoApp
 {
     public int Id { get; set; }
     public int UserId { get; set; }
     public string Title { get; set; }
     public string Description { get; set; }
     public DateTimeOffset CreatedAt { get; set; }



     public TodoAppDTO asDto => new TodoAppDTO
    {
        Id= Id,
        UserId= UserId,
        Title= Title,
        Description= Description
    };
 }

