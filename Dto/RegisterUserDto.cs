using System.ComponentModel.DataAnnotations;
namespace WebApplication1.Dto;

public class RegisterUserDto
{
    public int id { get; set; }
    
    [Required]
    [StringLength(30, MinimumLength = 3)]
    public string username { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    public string email { get; set; } = string.Empty;
    
    [Required]
    [MinLength(8)]
    public string password { get; set; } = string.Empty;
}