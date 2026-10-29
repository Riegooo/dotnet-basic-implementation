using Npgsql;
using WebApplication1.Models;

namespace WebApplication1.Repositories;

public class UserRepository
{
    private readonly NpgsqlDataSource _database;

    public UserRepository(NpgsqlDataSource dataSource)
    {
        _database = dataSource;
    }

    public async Task<User> CreateUser(User user)
    {
        const string sql = """
                           INSERT INTO users (username, email, password_hash)
                           VALUES ($1, $2, $3)
                           RETURNING id, username, email;
                           """;

        await using var command = _database.CreateCommand(sql);

        command.Parameters.AddWithValue(user.username);
        command.Parameters.AddWithValue(user.email);
        command.Parameters.AddWithValue(user.passwordHash);
        
        await using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
        {
            throw new InvalidOperationException(
                "The database did not return the created user.");
        }

        return new User
        {
            id = reader.GetInt32(0),
            username = reader.GetString(1),
            email = reader.GetString(2),
        };

    }

    public async Task<List<User>> GetAllUsers()
    {
        const string sql = """
                           SELECT  id, username, email
                           FROM users
                           ORDER BY id;
                           """;
        
        var users = new List<User>();

        await using var command = _database.CreateCommand(sql);
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            users.Add(new User
            {
                id = reader.GetInt32(0),
                username = reader.GetString(1),
                email = reader.GetString(2),
            });
        }

        return users;
    }

    public async Task<User> FindbyUsername(string username)
    {
        const string sql = """
                           SELECT id, username, email, password_hash
                           FROM users
                           WHERE username = $1;
                           """;
        
        await using var command = _database.CreateCommand(sql);
        command.Parameters.AddWithValue(username);
        
        await using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
        {
            return null;
        }

        return new User
        {
            id = reader.GetInt32(0),
            username = reader.GetString(1),
            email = reader.GetString(2),
            passwordHash = reader.GetString(3)
        };
    }
}