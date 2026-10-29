using Microsoft.AspNetCore.Identity;
using WebApplication1.Models;
using WebApplication1.Dto;
using WebApplication1.Repositories;

namespace WebApplication1.Services;

public class UserService
{
    private readonly UserRepository _userRepository;
    private readonly PasswordHasher<User>  _passwordHasher;
    
    public UserService(
        UserRepository userRepository,
        PasswordHasher<User> passwordHasher)

    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<List<User?>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllUsers();
    }
    
    public async Task<User?> RegisterAsync(RegisterUserDto request)
    {
        // Check if the fields are empty
        if (string.IsNullOrWhiteSpace(request.username) ||
            string.IsNullOrWhiteSpace(request.email) ||
            string.IsNullOrWhiteSpace(request.password))
        {
            return null;
        }
        
        // set the data in user
        var user = new User
        {
            username = request.username,
            email = request.email
        };
        
        // convert the password into hash password
        user.passwordHash = _passwordHasher.HashPassword(
            user,
            request.password
        );

        // put the data in users Database
        return await _userRepository.CreateUser(user);
    }

    public async Task<User?> LoginAsync(LoginUserDto request)
    {
        if (String.IsNullOrEmpty(request.username) ||
            String.IsNullOrEmpty(request.password))
        {
            return null;
        }

        // Find the username if existing
        var userFind = await _userRepository.FindbyUsername(request.username);
        
        // checks if user is null
        if (userFind is null)
        {
            return null;
        }
    
        // verify or comparing the password AND hash password
        var result = _passwordHasher.VerifyHashedPassword(
            userFind,
            userFind.passwordHash,
            request.password
        );
    
        // validating if password verification is failed
        if (result == PasswordVerificationResult.Failed)
        {
            return null;
        }
        
        return userFind;
    }

}

