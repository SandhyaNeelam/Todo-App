using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FullStack_ToDo.DTOs;
using FullStack_ToDo.Models;
using FullStack_ToDo.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace FullStack_ToDo.Controllers;

[ApiController]
[Route("api/users")]

public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly IUsersRepository _users;
    private IConfiguration _config;


    public UsersController(ILogger<UsersController> logger, IUsersRepository users, IConfiguration config)
    {
        _logger = logger;
        _users = users;
        _config = config;
    }

    [HttpPost]
    public async Task<ActionResult<List<UsersDTO>>> CreateUser([FromBody] UsersCreateDTO Data)
    {
        var toCreateUser = new Users
        {
            Name = Data.Name.Trim(),
            Email = Data.Email.ToLower().Trim(),
            Password = Data.Password.ToLower().Trim()
        };
        var createdUser = await _users.Create(toCreateUser);
        return StatusCode(StatusCodes.Status201Created);
    }

    // [HttpGet]
    // public async Task<ActionResult<List<UsersDTO>>> GetAllUsers()
    // {
    //     var usersList = await _users.GetUsers();
    //     var dtoList = usersList.Select(x => x.asDto);
    //     return Ok(dtoList);
    // }



    [HttpGet("{id}")]
    public async Task<ActionResult<UsersDTO>> GetById([FromRoute] int id)
    {
        var users = await _users.GetById(id);
        if (users is null)
            return NotFound("No users is found with given id");
        var dto = users.asDto;
        return Ok(dto);
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<ActionResult<UsersDTO>> Login([FromBody] UsersLogin usersLogin)
    {
        var user = await _users.GetByEmail(usersLogin.Email);
        if (user == null)
            return NotFound("No user found");
        if (user.Password != usersLogin.Password)
            return Unauthorized("Invalid Password");

        // var user = Authenticate(usersLogin);
        // if (user != null)
        // {
        var token = Generate(user);
        return Ok(token);
        // }
        // return NotFound("User not found");
    }

    private string Generate(Users users)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, users.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, users.Name),
            new Claim(ClaimTypes.Email, users.Email),
        };
        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
        _config["Jwt:Audience"],
        claims,
        expires: DateTime.Now.AddMinutes(15),
        signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // private Users Authenticate(UsersLogin usersLogin)
    // {
    // var currentUser = userConstants.Users.FirstOrDefault(o => o.Email.ToLower() == usersLogin.Email.ToLower() && o.Password == usersLogin.Password);
    // if(currentUser != null)
    // {
    //     return currentUser;
    // }
    //     return null;
    // }





}












// [HttpPut("{id}")]
// public async Task<ActionResult> UpdateUser([FromRoute] int id, [FromBody] UsersUpdateDTO Data)
// {
//     var existing = await _users.GetById(id);
//     if (existing is null)
//         return NotFound("No user found with given id");

//     var toUpdateUser = existing with
//     {
//         Email = Data.Email?.ToLower().Trim() ?? existing.Email,
//         Password = Data.Password?.ToLower().Trim() ?? existing.Password
//     };
//     var didUpdate = await _users.Update(toUpdateUser);
//     if (!didUpdate)
//         return StatusCode(StatusCodes.Status500InternalServerError, "Could not update user");

//     return NoContent();
// }




