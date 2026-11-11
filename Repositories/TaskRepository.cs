using Npgsql;
using WebApplication1.Models;

namespace WebApplication1.Repositories;

public class TaskRepository
{
    private readonly NpgsqlDataSource _database;
    public TaskRepository(NpgsqlDataSource dataSource)
    {
        _database = dataSource;
    }

    public async Task<TaskModel> CreateTask(TaskModel task)
    {
        const string sql = """
                           INSERT INTO task (task_title, task_description, user_id)
                           VALUES ($1, $2, $3)
                           RETURNING task_title, task_description, user_id;
                           """;

        await using var command = _database.CreateCommand(sql);

        command.Parameters.AddWithValue(task.task_title);
        command.Parameters.AddWithValue(task.task_description);
        command.Parameters.AddWithValue(task.user_id);

        await using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
        {
            throw new InvalidOperationException(
                "The database did not return the created user.");
        }

        return new TaskModel
        {
            task_title = reader.GetString(0),
            task_description = reader.GetString(1),
            user_id = reader.GetInt32(2),
        };
    }

    public async Task<List<TaskModel>> GetAllTasks()
    {
        const string sql = """
                           SELECT task_title, task_description, user_id
                           FROM task;
                           """;

        var tasks = new List<TaskModel>();

        await using var command = _database.CreateCommand(sql);
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            tasks.Add(new TaskModel
            {
                task_title = reader.GetString(0),
                task_description = reader.GetString(1),
                user_id = reader.GetInt32(2),
            });
        }

        return tasks;
    }
}

