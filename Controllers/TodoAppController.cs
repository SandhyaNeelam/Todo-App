using System.Security.Claims;
using FullStack_ToDo.DTOs;
using FullStack_ToDo.Models;
using FullStack_ToDo.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FullStack_ToDo.Controllers;

[ApiController]
[Route("api/todo-app")]

public class TodoAppController : ControllerBase
{
    private readonly ILogger<TodoAppController> _logger;
    private readonly ITodoAppRepository _todo;


    public TodoAppController(ILogger<TodoAppController> logger, ITodoAppRepository todo)
    {
        _logger = logger;
        _todo = todo;
    }

    [HttpPost]
    public async Task<ActionResult<List<TodoAppDTO>>> CreateTodo([FromBody] TodoAppCreateDTO Data)
    {
        var toCreateTodo = new TodoApp
        {
            UserId = Data.UserId,
            Title = Data.Title.Trim().ToLower(),
            Description = Data.Description.ToLower().Trim()
        };
        var createdTodo = await _todo.Create(toCreateTodo);
        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpGet("AllTodos")]
    [Authorize]
    public async Task<ActionResult<List<TodoApp>>> GetAllTodos()
    {
        var TodoList = await _todo.GetAllTodos();
        return Ok(TodoList);
    }

    [HttpGet("MyTodos")]
    [Authorize]
    public async Task<ActionResult<List<TodoApp>>> GetMyTodos()
    {
        var id = GetCurrentUserId();
        var Todo = await _todo.GetMyTodos(Int32.Parse(id));
        return Ok(Todo);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<TodoAppDTO>> GetById([FromRoute] int id)
    {
        var list = await _todo.GetById(id);
        if (list is null)
            return NotFound("No item is found with given id");
        var dto = list.asDto;
        return Ok(dto);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult> UpdateTodo(int id, [FromBody] TodoAppUpdateDTO Data)
    {
        var existing = await _todo.GetById(id);
        var presentUserId = GetCurrentUserId();
        if (Int32.Parse(presentUserId) != existing.UserId)
            return Unauthorized("You don't have access to update");

        if (existing is null)
            return NotFound("No todos found with given id");


        var toUpdateTodo = existing with
        {
            // Title = Data.Title,
            // Description = Data.Description
            Title = Data.Title ?? existing.Title,
            Description = Data.Description ?? existing.Description
        };
        var didUpdate = await _todo.Update(toUpdateTodo);
        if (!didUpdate)
            return StatusCode(StatusCodes.Status500InternalServerError, "Could not update Todo");

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult> DeleteTodoList(int id)
    {
        var Todo = await _todo.GetById(id);
        var user = GetCurrentUserId();
        if (Int32.Parse(user) != Todo.UserId)
            return Unauthorized("You don't have access to delete");

        if (Todo == null)
            return NotFound("todo not found");

        var didDelete = await _todo.Delete(id);

        if (!didDelete)
            return StatusCode(StatusCodes.Status500InternalServerError, "could not delete todo");
        return Ok("Todo Deleted");
        // var existing = await _todo.GetById(id);
        // if (existing is null)
        //     return NotFound("No list found with given id");
        // await _todo.Delete(id);
        // return NoContent();

    }


    private string GetCurrentUserId()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var userClaim = identity.Claims;
        return (userClaim.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
    }










}