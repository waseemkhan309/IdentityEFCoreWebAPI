using MailKit.Net.Smtp;
using MimeKit;
using Org.BouncyCastle.Tls;

namespace IdentityEFCoreWebAPI.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public EmailService(IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
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
            
            await SendEmailAsync(toEmail, "Email Register ", htmlContent, true);
        }



        public async Task SendAccountCreatedEmailAsync(string toEmail, string firstName, string loginLink)
        {
            string htmlContent = $@"
                <html><body style='font-family: Arial, sans-serif; background-color: #f4f6f8; margin:0; padding:20px;'>
                  <div style='max-width:600px; margin:auto; background:#fff; padding:30px; border-radius:8px;'>
                    <h2 style='color:#333;'>Hello, {firstName}!</h2>
                    <p style='font-size:16px; color:#555;'>Your account has been successfully created and your email is confirmed.</p>
                    <p style='text-align:center;'>
                      <a href='{loginLink}' style='background:#198754; color:#fff; padding:12px 24px; border-radius:6px; text-decoration:none; font-weight:bold;'>Login to Your Account</a>
                    </p>
                    <p style='font-size:12px; color:#999; margin-top:30px;'>&copy; {DateTime.UtcNow.Year} Dot Net Tutorials. All rights reserved.</p>
                  </div>
                </body></html>";


            await SendEmailAsync(toEmail, "Account Created", htmlContent ,true);
        }



        public async Task SendResendConfirmationEmailAsync(string toEmail, string firstName, string confirmationLink) {
            string htmlContent = $@"
                <html><body style='font-family: Arial, sans-serif; background-color: #f4f6f8; margin:0; padding:20px;'>
                  <div style='max-width:600px; margin:auto; background:#fff; padding:30px; border-radius:8px;'>
                    <h2 style='color:#333;'>Hello, {firstName}!</h2>
                    <p style='font-size:16px; color:#555;'>You requested a new email confirmation link. Please confirm your email by clicking the button below.</p>
                    <p style='text-align:center;'>
                      <a href='{confirmationLink}' style='background:#0d6efd; color:#fff; padding:12px 24px; border-radius:6px; text-decoration:none; font-weight:bold;'>Confirm Your Email</a>
                    </p>
                    <p style='font-size:12px; color:#999; margin-top:30px;'>&copy; {DateTime.UtcNow.Year} Dot Net Tutorials. All rights reserved.</p>
                  </div>
                </body></html>";


            await SendEmailAsync(toEmail, "Email Confirmation", htmlContent, true);
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body, bool isBodyHtml = false)
        {
            try
            {
                var smtpServer = _configuration["EmailSettings:SmtpServer"] ?? throw new ArgumentException("EmailSettings:SmptServer value should be null");
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
                var senderEmail = _configuration["EmailSettings:SenderEmail"] ?? throw new ArgumentException("EmailSettings:SendEmail value should be null");
                var senderName = _configuration["EmailSettings:SenderName"] ?? throw new ArgumentException("EmailSettings:SenderName value should be null");
                var password = _configuration["EmailSettings:Password"] ?? throw new ArgumentException("EmailSettings:Password value should be null");
                
                using (var message = new MimeMessage())
                {
                    message.From.Add(new MailboxAddress(senderName, senderEmail));
                    message.To.Add(new MailboxAddress("Recipient", toEmail));
                    message.Subject = subject;
                    
                    if (isBodyHtml)
                    {
                        message.Body = new TextPart("html")
                        {
                            Text = body
                        };
                    }
                    else
                    {
                        message.Body = new TextPart("plain")
                        {
                            Text = body
                        };
                    }

                    using (var client = new SmtpClient())
                    {
                        await client.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);

                        await client.AuthenticateAsync(senderEmail, password);
                        
                        // Send the email
                        await client.SendAsync(message);

                    }
                        _logger.LogInformation($"Email sent to {toEmail} with subject: {subject}");
                }

            }catch(Exception ex)
            {
                //Console.WriteLine($@" Exception Error in Send Email Async Method -- {ex.ToString()}");
                //Console.WriteLine(ex.StackTrace);
                _logger.LogError(ex, $"Failed to send email to {toEmail} with subject: {subject}");
                throw;
            }
    }

}}
