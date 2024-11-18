using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        if (await _context.Users.AnyAsync(u => u.Email == user.Email))
        {
            return BadRequest(new { message = "El correo electrónico ya está registrado." });
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
        user.CreatedAt = DateTime.UtcNow;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Usuario registrado exitosamente." });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return Unauthorized(new { message = "Correo o contraseña incorrectos." });
        }

        var token = GenerateJwtToken(user);

        var session = new Session
        {
            UserId = user.Id,
            SessionToken = token,
            IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
            UserAgent = Request.Headers["User-Agent"].ToString(),
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddHours(2),
            IsActive = true
        };

        _context.Sessions.Add(session);
        await _context.SaveChangesAsync();

        return Ok(new { token });
    }

    [HttpPost("send-otp")]
    public async Task<IActionResult> SendOtp([FromQuery] string emailOrPhone)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u =>
            u.Email == emailOrPhone || u.PhoneNumber == emailOrPhone);

        if (user == null)
        {
            return NotFound(new { message = "Usuario no encontrado." });
        }

        var otpCode = new Random().Next(100000, 999999).ToString();
        user.OtpCode = otpCode;
        user.OtpExpiration = DateTime.UtcNow.AddMinutes(5);

        await _context.SaveChangesAsync();

        return Ok(new { message = "OTP enviado correctamente." });
    }

    [HttpPost("validate-otp")]
    public async Task<IActionResult> ValidateOtp([FromBody] OtpValidationRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null)
        {
            return NotFound(new { message = "Usuario no encontrado." });
        }

        if (user.OtpCode != request.OtpCode || user.OtpExpiration < DateTime.UtcNow)
        {
            return BadRequest(new { message = "OTP inválido o expirado." });
        }

        user.IsVerified = true;
        user.OtpCode = null;
        user.OtpExpiration = null;
        await _context.SaveChangesAsync();

        return Ok(new { message = "OTP validado correctamente." });
    }

    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TuClaveSecretaSuperSegura"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim("userId", user.Id.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: "TuAplicacion",
            audience: "TuAplicacion",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
