using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    // Inyectar el servicio de autenticación
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    // Método para el login
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel model)
    {
        if (_authService.ValidateUser(model.Username, model.Password))
        {
            return Ok(new { message = "Login successful" });
        }
        return Unauthorized();
    }
}

// Modelo para el login
public class LoginModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}
