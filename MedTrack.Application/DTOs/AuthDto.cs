using System.ComponentModel.DataAnnotations;

namespace MedTrack.Application.DTOs
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunludur")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre zorunludur")]
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterRequestDto
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunludur")]
        [MinLength(3, ErrorMessage = "Kullanıcı adı en az 3 karakter olmalıdır")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre zorunludur")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta adresi zorunludur")]
        [EmailAddress(ErrorMessage = "Geçersiz e-posta formatı")]
        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = "Doctor"; // Doctor, Admin, Staff, Patient
    }

    public class AuthResponseDto
    {
        public bool Success { get; set; }
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string ExpiresIn { get; set; } = "60 minutes";
        public string TokenType { get; set; } = "Bearer";
    }
}
