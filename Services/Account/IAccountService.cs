using IdentityEFCoreWebAPI.Data;
using IdentityEFCoreWebAPI.DTOs;
using Microsoft.AspNetCore.Identity;

namespace IdentityEFCoreWebAPI.Services.Account
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterUserAsync(RegisterDTO register);
        Task<IdentityResult> ConfirmationEmailAsync(Guid userId, string token);

        Task<SignInResult> LoginUserAsync(LoginDTO loginmodel);

        Task LogoutUserAsync();
        Task SendEmailConfirmationAsync(string email);

        Task<ProfileDTO> GetUserProfileByEmailAsync(string email);
    }
}
