using Dapper;
using FullStack_ToDo.Models;

namespace FullStack_ToDo.Repositories;

public interface IUsersRepository
{
    Task<Users> Create(Users Item);
    Task<Users> GetById(int Id);
    Task<Users> GetByEmail(string Email);
    // Task<List<Users>> GetUsers();
    // Task<bool> Update(Users Item);
    Task<Users> CreateLogin(Users Item);

}

public class UsersRepository : BaseRepository, IUsersRepository
{
    public UsersRepository(IConfiguration config) : base(config)
    {

    }

    public async Task<Users> Create(Users Item)
    {
        var query = $@"INSERT INTO public.users(name, email, password) VALUES(@Name, @Email, @Password) RETURNING * ";
        using (var connection = NewConnection)
        {
            var res = await connection.QuerySingleOrDefaultAsync<Users>(query, Item);
            return res;

        }
    }

    public async Task<Users> CreateLogin(Users Item)
    {
        var query = $@"INSERT INTO public.users(email, password) VALUES(@Email, @Password) RETURNING * ";
        using (var connection = NewConnection)
        {
            var res = await connection.QuerySingleOrDefaultAsync<Users>(query, Item);
            return res;

        }

    }

    public async Task<Users> GetById(int Id)
    {
        var query = $@"SELECT * FROM users WHERE id = @Id";
        using (var connection = NewConnection)
            return await connection.QuerySingleOrDefaultAsync<Users>(query, new { Id });
    }

    public async Task<Users> GetByEmail(string Email)
    {
         var query = $@"SELECT * FROM users WHERE email= @Email";
        using (var connection = NewConnection)
            return await connection.QuerySingleOrDefaultAsync<Users>(query, new { Email });

    }

    // public async Task<List<Users>> GetUsers()
    // {
    //     var query = $@"SELECT * FROM users";
    //     var connection = NewConnection;
    //     var response = (await connection.QueryAsync<Users>(query)).AsList();
    //     return response;
    // }



    // public async Task<bool> Update(Users Item)
    // {
    //     var query = $@"UPDATE public.users SET email=@Email, password= @Password WHERE id = @Id";
    //     using (var connection = NewConnection)
    //     {
    //         var rowCount = await connection.ExecuteAsync(query, Item);
    //         return rowCount == 1;

    //     }
    // }
}