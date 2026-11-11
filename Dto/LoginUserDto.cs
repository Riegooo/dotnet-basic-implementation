using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Dto;

public class LoginUserDto
{
    [Required]
    [StringLength(30, MinimumLength = 3)]
    public string username { get; set; } = "";
    [Required]
    [MinLength(8)]
    public string password { get; set; } = "";
}