using BlazorApp.Shared;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace BlazorApp.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly NpgsqlConnection _connection;

    public TodoController(IConfiguration configuration)
    {
        _configuration = configuration;
        var _ConnectionStrings = _configuration.GetConnectionString("DefaultConnection");
        _connection = new NpgsqlConnection(_ConnectionStrings);
    }

    [HttpGet("all")]
    public async Task<Todo[]> GetRows()
    {
        await _connection.OpenAsync();

        var hi = await _connection.QueryAsync<Todo>("SELECT \"Id\", \"Title\", \"Title\" FROM \"todo\"");

        await _connection.CloseAsync();

        return hi.ToArray();
    }
}
