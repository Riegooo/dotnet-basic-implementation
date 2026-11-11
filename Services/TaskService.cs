using Microsoft.AspNetCore.Antiforgery;
using WebApplication1.Dto;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Services;

public class TaskService
{
    private readonly TaskRepository _taskRepository;
    public TaskService(TaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<List<TaskModel?>> GetAllTaskAsync()
    {
        return await _taskRepository.GetAllTasks();
    }

    public async Task<TaskModel?> CreateTaskAsync(CreateTaskDto request)
    {
        if (string.IsNullOrWhiteSpace(request.task_title)|| 
            string.IsNullOrWhiteSpace(request.task_description))
        {
            return null;
        }

        var task = new TaskModel
        {
            task_title = request.task_title,
            task_description = request.task_description,
            user_id = request.user_id
        };

        return await _taskRepository.CreateTask(task);

    }
}

