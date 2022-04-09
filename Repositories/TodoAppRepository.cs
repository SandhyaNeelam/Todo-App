using Dapper;
using FullStack_ToDo.Models;

namespace FullStack_ToDo.Repositories;

public interface ITodoAppRepository
{
    Task<TodoApp> Create(TodoApp Item);
    Task<TodoApp> GetById(int Id);
    Task<List<TodoApp>> GetAllTodos();
    Task<List<TodoApp>> GetMyTodos(int UserId);
    Task<bool> Update(TodoApp Item);
    Task<bool> Delete(int Id);
}

public class TodoAppRepository : BaseRepository, ITodoAppRepository
{
    public TodoAppRepository(IConfiguration config) : base(config)
    {

    }

    public async Task<TodoApp> Create(TodoApp Item)
    {
        var query = $@"INSERT INTO public.todo_app(user_id, title, description) VALUES(@UserId, @Title, @Description) RETURNING * ";
        using (var connection = NewConnection)
        {
            var res = await connection.QuerySingleOrDefaultAsync<TodoApp>(query, Item);
            return res;

        }
    }

    public async Task<bool> Delete(int Id)
    {
        var query = $@"DELETE FROM public.todo_app WHERE id = @Id;";

        using (var connection = NewConnection)
        {
            var resp = await connection.ExecuteAsync(query, new { Id });
            return resp > 0;
        }
    }

    public async Task<TodoApp> GetById(int Id)
    {
        var query = $@"SELECT * FROM todo_app WHERE id = @Id";
        using (var connection = NewConnection)
            return await connection.QuerySingleOrDefaultAsync<TodoApp>(query, new { Id });
    }

    public async Task<List<TodoApp>> GetAllTodos()
    {
        var query = $@"SELECT * FROM todo_app";
        var connection = NewConnection;
        var response = (await connection.QueryAsync<TodoApp>(query)).AsList();
        return response;
    }

    public async Task<List<TodoApp>> GetMyTodos(int UserId)
    {
        // var query = $@"SELECT * FROM todo_app WHERE user_id= @UserId";
        var query = $@"SELECT * FROM todo_app WHERE user_id= @UserId";
        List<TodoApp> response;
        using (var connection = NewConnection)
            return (await connection.QueryAsync<TodoApp>(query, new { UserId })).ToList();
    }


    public async Task<bool> Update(TodoApp Item)
    {
        var query = $@"UPDATE public.todo_app SET title=@Title, description= @Description WHERE id = @Id";
        using (var connection = NewConnection)
        {
            var rowCount = await connection.ExecuteAsync(query, Item);
            return rowCount == 1;

        }
    }
}