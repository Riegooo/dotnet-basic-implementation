namespace WebApplication1.Models;

public class UserModel
{
    public int id{ get;set; }
    public string username { get; set; } = "";
    public string email { get; set; } = "";
    public string passwordHash { get; set; } = "";
}