using System.ComponentModel.DataAnnotations;

namespace Yugen.HomeBudget.Shared.Models.Authentication
{
    public class RegisterRequest
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match!")]
        public string PasswordConfirm { get; set; } = string.Empty;
    }
}