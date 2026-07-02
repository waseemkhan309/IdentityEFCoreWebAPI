

namespace IdentityEFCoreWebAPI.DTOs
{
    public class ProfileDTO
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }    
        public DateTime? DateOfBirth { get; set; }
        
        public string? PhoneNumber { get; set; }
        public DateTime? LastLoggedIn { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
