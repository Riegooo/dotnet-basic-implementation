using System.ComponentModel.DataAnnotations;

namespace  WebApplication1.Dto;

public class CreateTaskDto
{
    [Required] 
    public string task_title { get; set; } = string.Empty;

    [Required] 
    public string task_description { get; set; } = string.Empty;

    [Required] 
    public int user_id { get; set; } = 0;

}
