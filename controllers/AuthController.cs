using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    public AuthController(IConfiguration configuration){
        _config = configuration;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] UserLogin login)
    {
        var user = Authenticate(login);
        if (user != null) {
            var token = GenerateToken(user);
            return Ok(new {token});
        }

        return Unauthorized();
    }

    private User? Authenticate(UserLogin login)
    {
        // NO HAGAS ESTO EN CASA
        if (login.Username == "admin" && login.Password == "password") 
        {
            return new User {
                Username = "admin",
                email = "admin@gmail.com",
                role = "admin"
            };
        }

        return null;
    }

    private string GenerateToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:key"])
        );

        var credentials = new SigningCredentials(securityKey,
            SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Username),
            new Claim(ClaimTypes.Email, user.email),
            new Claim(ClaimTypes.Role, user.role)
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}