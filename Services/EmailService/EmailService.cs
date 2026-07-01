namespace IdentityEFCoreWebAPI.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendRegistrationEmailAsync(string toEmail, string firstName, string confirmationLink) {
            string htmlContent = $@"
                <html>
                    <body style='font-family: Arial, sans-serif; background-color: #f4f6f8; margin:0; padding:20px;'>
                          <div style='max-width:600px; margin:auto; background:#fff; padding:30px; border-radius:8px;'>
                            <h2 style='color:#333;'>Welcome, {firstName}!</h2>
                            <p style='font-size:16px; color:#555;'>Thank you for registering. Please confirm your email by clicking the button below.</p>
                            <p style='text-align:center;'>
                              <a href='{confirmationLink}' style='background:#0d6efd; color:#fff; padding:12px 24px; border-radius:6px; text-decoration:none; font-weight:bold;'>Confirm Your Email</a>
                            </p>
                            <p style='font-size:12px; color:#999; margin-top:30px;'>&copy; {DateTime.UtcNow.Year} Dot Net Tutorials. All rights reserved.</p>
                          </div>
                    </body>
                </html>";

            await 
        }
    }
}
