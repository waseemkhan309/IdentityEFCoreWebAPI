using IdentityEFCoreWebAPI.Data;
using IdentityEFCoreWebAPI.DTOs;
using IdentityEFCoreWebAPI.Models;
using IdentityEFCoreWebAPI.Services.EmailService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace IdentityEFCoreWebAPI.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public AccountService(UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager,
                              IEmailService emailService,
                              IConfiguration configuration,
                              ILogger logger
        ){
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterDTO registerDto)
        {
            var user = new ApplicationUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                DateOfBirth = registerDto.DateOfBirth,
                PhoneNumber = registerDto.PhoneNumber,
                isActive = true,
                CreatedOn = DateTime.UtcNow ,
                ModifiedOn = DateTime.UtcNow
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) {
                return result;
            }


            // Assign "User" role by default
            IdentityResult roleAssignedResult = await _userManager.AddToRoleAsync(user, "User");
            if (!roleAssignedResult.Succeeded)
            {
                return roleAssignedResult;
            }

            var token = await GenerateEmailConfirmationTokenAsync(user);

            var baseUrl = _configuration["AppSettings:BaseUrl"] ?? throw new InvalidOperationException("BaseUrl is not configured.");
            var confirmationLink = $"{baseUrl}/Account/ConfirmEmail?userId={user.Id}&token={token}";

            await _emailService.SendRegistrationEmailAsync(user.Email, user.FirstName, confirmationLink);

            return result;

        }


        public async Task<IdentityResult> ConfirmationEmailAsync(Guid userId, string token)
        {
            if(userId == Guid.Empty && string.IsNullOrEmpty(token))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Invalid token or user ID"
                });
            }

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if(user == null) {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "User not found"
                });
            }

            var decodedBytes = WebEncoders.Base64UrlDecode(token);
            var decodedToken = Encoding.UTF8.GetString(decodedBytes);


            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (result.Succeeded)
            {
                var baseUrl = _configuration["AppSettings:BaseUrl"] ?? throw new InvalidOperationException("BaseUrl is not configured.");
                var loginLink = $"{baseUrl}/Account/Login";

                await _emailService.SendAccountCreatedEmailAsync(user.Email, user.FirstName!, loginLink);
            }

            return result;


        }

        public async Task<ProfileDTO> GetUserProfileByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));
            }

            var user = await _userManager.FindByEmailAsync(email);
            if(user == null)
            {
                throw new ArgumentException("User not found", nameof(email));
            }

            return new ProfileDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName ?? string.Empty,
                Username = user.Email ?? string.Empty,
                Email = user.Email ?? string.Empty,
                PhoneNumber = user.PhoneNumber,
                LastLoggedIn = user.LastLogin,
                CreatedOn = user.CreatedOn,
                DateOfBirth = user.DateOfBirth
            };

        }

        public async Task<SignInResult> LoginUserAsync(LoginDTO loginModel)
        {
            if (loginModel == null && string.IsNullOrWhiteSpace(loginModel.Email))
            {
                throw new ArgumentException("Login argumentes or email should not be empty");
            }


            var resultUser = await _userManager.FindByEmailAsync(loginModel.Email);
            if (resultUser == null) {
                return SignInResult.Failed;
            }

            if(!await _userManager.IsEmailConfirmedAsync(resultUser)) {
                return SignInResult.NotAllowed;
            }

            var signInResult = await _signInManager.PasswordSignInAsync(resultUser.UserName!, loginModel.Password, loginModel.RememberMe, lockoutOnFailure: false);

            if (signInResult.Succeeded)
            {
                resultUser.LastLogin = DateTime.UtcNow;
                await _userManager.UpdateAsync(resultUser);

            }

            return signInResult;
        }

        public async Task LogoutUserAsync()
        {
            await _signInManager.SignOutAsync();
        }

      

        public async Task SendEmailConfirmationAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException(" Email is required", nameof(email));
            }


            var user = await _userManager.FindByEmailAsync(email);

            if(user == null)
            {
                return;
            }
            
            if(!await _userManager.IsEmailConfirmedAsync(user))
            {
                return;
            }


            var token = await GenerateEmailConfirmationTokenAsync(user);

            var baseURL = _configuration["BaseUrl"] ?? throw new InvalidOperationException("Base URl is not configuard");

            var confirmationEmailLink = $"{baseURL}/Account/ConfirmEmail?userId={user.Id}&token={token}";
            await _emailService.SendResendConfirmationEmailAsync(user.Email!, user.FirstName!, confirmationEmailLink);

        }


        // Helper method GenerateEmailConfirmationTokenAsync method
        public async Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user) { 
            if(user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            return encodedToken;
        }
    }
}
