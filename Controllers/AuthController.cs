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

    [HttpPost("login4admin")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _context.User
            .SingleOrDefaultAsync(u => u.username == request.Username || u.email == request.Email);

        if (user == null || !VerifyPassword(request.Password, user.passwordhash))
        {
            return Unauthorized();
        }

        var token = GenerateJwtToken(user);

        return Ok(new { Token = token });
    }

    private bool VerifyPassword(string? password, string? passwordHash)
    {
        // Thêm logic kiểm tra mật khẩu
        return password == passwordHash; // Thay thế bằng kiểm tra hash thực tế
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

public class LoginRequest
{
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
}
