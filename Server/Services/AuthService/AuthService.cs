using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace BlazorEcommerce.Server.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _config;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(
        ApplicationDbContext context,
        IConfiguration config,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _config = config;
        _httpContextAccessor = httpContextAccessor;
    }

    public int GetUserId() => int.Parse(
        _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

    public string GetUserEmail() =>
        _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);

    public async Task<ServiceResponse<int>> Register(User user, string password)
    {
        if (await UserExists(user.Email))
            return new ServiceResponse<int> {Success = false, Message = "User already registered."};

        byte[] passwordHash;
        byte[] passwordSalt;

        CreatePasswordHash(password, out passwordHash, out passwordSalt);

        user.PasswordHash = passwordHash;
        user.PasswordSalt =passwordSalt;
        user.DateCreated = user.DateCreated.ToUniversalTime();

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new ServiceResponse<int> { Data = user.Id, Message = "Register Success!"};
    }

    public async Task<ServiceResponse<string>> Login(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower().Equals(email.ToLower()));

        var response = new ServiceResponse<string>();

        if (user is null)
        {
            response.Success = false;
            response.Message = "User not found";
        }
        else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
        {
            response.Success = false;
            response.Message = "Wrong password.";
        }
        else
        {
            response.Data = CreateJsonWebToken(user);
            response.Message = "Token issued";
        }

        return response;
    }

    public async Task<bool> UserExists(string email)
    {
        return await
            _context.Users.AnyAsync(u => u.Email.ToLower().Equals(email.ToLower()));
    }

    public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var compareHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return passwordHash.SequenceEqual(compareHash);
        }
    }

    public string CreateJsonWebToken(User user)
    {
        List<Claim> claims = new List<Claim> {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Email),
        };

        var key = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
        
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now + TimeSpan.FromDays(3),
            signingCredentials: credentials
        );

        var Jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return Jwt;
    }

    public async Task<ServiceResponse<bool>> ChangePassword(int userId, string newPassword)
    {
        var user = await _context.Users.FindAsync(userId);

        if (user is null)
            return new ServiceResponse<bool> {
                Success = false,
                Message = "User not found"
            };
        
        byte[] passwordSalt;
        byte[] passwordHash;

        CreatePasswordHash(newPassword, out passwordHash, out passwordSalt);

        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        await _context.SaveChangesAsync();

        return new ServiceResponse<bool> {
            Data = true,
            Message = "Password changed successfully."
        };
        
    }
}