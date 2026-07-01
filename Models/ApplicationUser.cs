using Microsoft.AspNetCore.Identity;

namespace IdentityEFCoreWebAPI.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = null!;
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool isActive { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public virtual List<Address>? Addresses { get; set; }
    }
}
