using System.Security.Cryptography;

namespace BlazorEcommerce.Server.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;

    public AuthService(ApplicationDbContext context)
    {
        _context = context;
    }

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
}