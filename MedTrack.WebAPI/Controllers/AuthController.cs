using Microsoft.AspNetCore.Mvc;
using MedTrack.Application.Services;

namespace MedTrack.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtTokenService _tokenService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IJwtTokenService tokenService, ILogger<AuthController> logger)
        {
            _tokenService = tokenService;
            _logger = logger;
        }

        /// <summary>
        /// Login and get JWT token
        /// </summary>
        /// <param name="loginRequest">Username and password</param>
        /// <returns>JWT token</returns>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                if (string.IsNullOrEmpty(loginRequest?.Username) || string.IsNullOrEmpty(loginRequest?.Password))
                {
                    return BadRequest(new { message = "Username and password are required" });
                }

                // TODO: Validate credentials against user database
                // For now, accept any non-empty credentials for demo purposes
                if (loginRequest.Username.Length < 3 || loginRequest.Password.Length < 6)
                {
                    return Unauthorized(new { message = "Invalid credentials" });
                }

                // Generate token (userId = username for demo)
                var token = _tokenService.GenerateToken(loginRequest.Username, loginRequest.Username, "User");

                _logger.LogInformation($"User {loginRequest.Username} logged in successfully");

                return Ok(new
                {
                    success = true,
                    token = token,
                    expiresIn = "60 minutes",
                    tokenType = "Bearer"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Login error: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred during login" });
            }
        }

        /// <summary>
        /// Refresh JWT token
        /// </summary>
        /// <returns>New JWT token</returns>
        [HttpPost("refresh")]
        public IActionResult RefreshToken()
        {
            try
            {
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                var userNameClaim = User.FindFirst(System.Security.Claims.ClaimTypes.Name);
                var roleClaim = User.FindFirst(System.Security.Claims.ClaimTypes.Role);

                if (userIdClaim == null || userNameClaim == null)
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                var token = _tokenService.GenerateToken(
                    userIdClaim.Value,
                    userNameClaim.Value,
                    roleClaim?.Value ?? "User"
                );

                return Ok(new
                {
                    success = true,
                    token = token,
                    expiresIn = "60 minutes",
                    tokenType = "Bearer"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Token refresh error: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred during token refresh" });
            }
        }
    }

    public class LoginRequest
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
