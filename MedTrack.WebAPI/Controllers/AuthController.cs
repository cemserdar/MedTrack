using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using MedTrack.Application.DTOs;
using MedTrack.Application.Services;
using MedTrack.Domain.Entities;
using MedTrack.Domain.Interfaces;

namespace MedTrack.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenService _tokenService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IJwtTokenService tokenService,
            ILogger<AuthController> logger)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _logger = logger;
        }

        /// <summary>
        /// Kullanıcı girişi ve JWT token üretimi
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = await _userRepository.GetByUsernameAsync(request.Username);
                if (user == null || !user.IsActive)
                {
                    _logger.LogWarning("Geçersiz kullanıcı adı denemesi: {Username}", request.Username);
                    return Unauthorized(new { message = "Kullanıcı adı veya şifre hatalı" });
                }

                if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
                {
                    _logger.LogWarning("Hatalı şifre denemesi: {Username}", request.Username);
                    return Unauthorized(new { message = "Kullanıcı adı veya şifre hatalı" });
                }

                var token = _tokenService.GenerateToken(user.Id.ToString(), user.Username, user.Role);

                _logger.LogInformation("Kullanıcı {Username} ({Role}) başarıyla giriş yaptı", user.Username, user.Role);

                return Ok(new AuthResponseDto
                {
                    Success = true,
                    Token = token,
                    Username = user.Username,
                    Role = user.Role,
                    ExpiresIn = "60 minutes",
                    TokenType = "Bearer"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Giriş işlemi sırasında hata oluştu: {Username}", request.Username);
                return StatusCode(500, new { message = "Giriş yapılırken sunucu hatası oluştu" });
            }
        }

        /// <summary>
        /// Yeni kullanıcı kaydı (Doktor, Personel veya Hasta)
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existingUser = await _userRepository.GetByUsernameAsync(request.Username);
                if (existingUser != null)
                {
                    return BadRequest(new { message = "Bu kullanıcı adı zaten kullanılmaktadır." });
                }

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Username = request.Username,
                    Email = request.Email,
                    PasswordHash = _passwordHasher.HashPassword(request.Password),
                    Role = string.IsNullOrWhiteSpace(request.Role) ? "Doctor" : request.Role,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _userRepository.AddAsync(user);

                var token = _tokenService.GenerateToken(user.Id.ToString(), user.Username, user.Role);

                _logger.LogInformation("Yeni kullanıcı kaydedildi: {Username} ({Role})", user.Username, user.Role);

                return StatusCode(201, new AuthResponseDto
                {
                    Success = true,
                    Token = token,
                    Username = user.Username,
                    Role = user.Role,
                    ExpiresIn = "60 minutes",
                    TokenType = "Bearer"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kayıt işlemi sırasında hata oluştu: {Username}", request.Username);
                return StatusCode(500, new { message = "Kayıt işlemi sırasında sunucu hatası oluştu" });
            }
        }

        /// <summary>
        /// Mevcut JWT token ile oturum yenileme
        /// </summary>
        [Authorize]
        [HttpPost("refresh")]
        public IActionResult RefreshToken()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                var userNameClaim = User.FindFirst(ClaimTypes.Name);
                var roleClaim = User.FindFirst(ClaimTypes.Role);

                if (userIdClaim == null || userNameClaim == null)
                {
                    return Unauthorized(new { message = "Geçersiz veya süresi dolmuş token" });
                }

                var role = roleClaim?.Value ?? "User";
                var token = _tokenService.GenerateToken(
                    userIdClaim.Value,
                    userNameClaim.Value,
                    role
                );

                return Ok(new AuthResponseDto
                {
                    Success = true,
                    Token = token,
                    Username = userNameClaim.Value,
                    Role = role,
                    ExpiresIn = "60 minutes",
                    TokenType = "Bearer"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token yenileme hatası");
                return StatusCode(500, new { message = "Token yenilenirken sunucu hatası oluştu" });
            }
        }
    }
}
