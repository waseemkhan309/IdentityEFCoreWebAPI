

using IdentityEFCoreWebAPI.DTOs;
using IdentityEFCoreWebAPI.Models;
using IdentityEFCoreWebAPI.Services.Account;
using IdentityEFCoreWebAPI.Services.EmailService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityEFCoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService accountService, IEmailService emailService, UserManager<ApplicationUser> userManager ,ILogger<AccountController> logger) { 
            _accountService = accountService;
            _emailService = emailService;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpPost("/register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDTO registerDTO)
        {
            var IsUserExist = await _userManager.FindByEmailAsync(registerDTO.Email);

            if(IsUserExist != null)
            {
                return BadRequest("Please login user already exist");
            }

            var registerUser = await _accountService.RegisterUserAsync(registerDTO);
            //await _emailService.SendRegistrationEmailAsync(userEmail,"Waseem","https://google.com");

            if (registerUser != null)
            {
                return Ok(new { success = true, Message = "Email sent successfully!!", data = registerUser });
            }

            return BadRequest(new { success = false, error = "Registration failed" });
        }
    }
}
