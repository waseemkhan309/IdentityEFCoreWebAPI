

using System.ComponentModel.DataAnnotations;

namespace IdentityEFCoreWebAPI.DTOs
{
    public class ResendConfirmationEmailDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = null!;
    }
}
