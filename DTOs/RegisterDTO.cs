

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IdentityEFCoreWebAPI.Data
{
    public class RegisterDTO
    {
        [Required]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "First Name cannot be longer than 50 characters.")]
        public string FirstName { get; set; } = null!;

        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage ="Last Name cannot be longer than 50 characters.")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Email Id is required")]
        [EmailAddress(ErrorMessage ="Invalid Email Address")]
        public string Email { get; set; } = null!;

        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime? DateOfBirth { get; set; }


        [Required(ErrorMessage ="PhoneNumber is required")]
        [Phone(ErrorMessage ="Please enter a valid Phone Number")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = null!;


        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength =6, ErrorMessage ="Password must be at least 6 characters.")]
        public string Password { get; set; } = null!;


        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="Password and confirmation password do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = null!;











    }
}
