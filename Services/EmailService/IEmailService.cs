namespace IdentityEFCoreWebAPI.Services.EmailService
{
    public interface IEmailService
    {
        Task SendRegistrationEmailAsync(string toEmail, string firstName, string confirmationLink);
        Task SendAccountCreatedEmailAsync(string toEmail, string firstName, string loginLink);
        Task SendResendConfirmationEmailAsync(string toEmail, string firstName, string confirmationLink);
    }
}
