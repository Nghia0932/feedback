using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }
        private bool VerifyPassword(string password, string passwordHash)
    {
        // Use BCrypt to verify the password
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
    private string HashPassword(string password)
    {
        // Use BCrypt to hash the password
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    [HttpPost("login4admin")]
    public async Task<IActionResult> Login([FromBody] Login4Admin request)
    {
        var user = await _context.User
            .SingleOrDefaultAsync(u => u.email == request.Email);
        if (user == null || !VerifyPassword(request.Password,  user.passwordhash))
        {
          
            return Unauthorized();
        }
        var token = GenerateJwtToken(user);
        return Ok(new { Token = token });
    }

    [HttpPost("register4admin")]
    public async Task<IActionResult> Register([FromBody] Register4Admin request)
    {
        if (await _context.User.AnyAsync(u => u.email == request.Email))
        {
            return BadRequest("Username or Email already exists.");
        }
        var user = new User
        {
            username = request.Username,
            email = request.Email,
            passwordhash = HashPassword(request.Password)
        };
        _context.User.Add(user);
        await _context.SaveChangesAsync();
        return Ok("User registered successfully.");
    }
     [HttpPost("update4admin")]
    public async Task<IActionResult> Update([FromBody] Update4Admin request)
    {
    // Check if the user with the provided email exists
        var user = await _context.User.SingleOrDefaultAsync(u => u.email == request.Email);
        if (user == null)
        {
            return BadRequest("Email does not exist.");
        }
        // Update user details
        if (!string.IsNullOrEmpty(request.Username))
        {
            user.username = request.Username;
        }

        if (!string.IsNullOrEmpty(request.Password))
        {
            user.passwordhash = HashPassword(request.Password);
        }

        // Save changes to the database
        _context.User.Update(user);
        await _context.SaveChangesAsync();

        return Ok("Update successful.");
    }


    private string GenerateJwtToken(User user)
    {
        var secretKey = _configuration["Jwt:Key"];
        if (string.IsNullOrEmpty(secretKey))
        {
            throw new InvalidOperationException("JWT secret key is not configured.");
        }
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.username ?? "UnknownUser"),
                new Claim(ClaimTypes.Email, user.email ?? "Unknown")
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = creds
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

public class Login4Admin
{
    public  string? Username { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
}

public class Register4Admin
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
}
public class Update4Admin {
    public string? Username { get; set;}
    public string? Password { get; set;}
    public required string Email {get; set;}
}