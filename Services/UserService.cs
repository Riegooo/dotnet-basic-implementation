using Microsoft.AspNetCore.Identity;
using WebApplication1.Models;
using WebApplication1.Dto;
using WebApplication1.Repositories;

namespace WebApplication1.Services;

public class UserService
{
    private readonly UserRepository _userRepository;
    private readonly PasswordHasher<UserModel>  _passwordHasher;
    
    public UserService(
        UserRepository userRepository,
        PasswordHasher<UserModel> passwordHasher)

    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<List<UserModel?>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllUsers();
    }
    
    public async Task<UserModel?> RegisterAsync(RegisterUserDto request)
    {
        if (string.IsNullOrWhiteSpace(request.username) ||
            string.IsNullOrWhiteSpace(request.email) ||
            string.IsNullOrWhiteSpace(request.password))
        {
            return null;
        }

        var user = new UserModel
        {
            username = request.username,
            email = request.email
        };
        
        user.passwordHash = _passwordHasher.HashPassword(
            user,
            request.password
        );

        return await _userRepository.CreateUser(user);
    }

    public async Task<UserModel?> LoginAsync(LoginUserDto request)
    {
        if (String.IsNullOrEmpty(request.username) ||
            String.IsNullOrEmpty(request.password))
        {
            return null;
        }

        var userFind = await _userRepository.FindbyUsername(request.username);
        
        if (userFind is null)
        {
            return null;
        }
    
        var result = _passwordHasher.VerifyHashedPassword(
            userFind,
            userFind.passwordHash,
            request.password
        );
    
        if (result == PasswordVerificationResult.Failed)
        {
            return null;
        }
        
        return userFind;
    }
    
    

}

