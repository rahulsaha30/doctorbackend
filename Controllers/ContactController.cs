using doctor.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace doctor.Controllers
{
    [ApiController]
    [Route("api/email")]
    public class ContactController : ControllerBase
    {
        private readonly EmailSettings _emailSettings;

        public ContactController(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail([FromBody] ContactRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Message))
            {
                return BadRequest(new { success = false, error = "All fields required" });
            }

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var mail = new MailMessage
                {
                    From = new MailAddress(_emailSettings.Email),
                    Subject = $"New Message from {request.Name}",
                    Body = $@"
Name: {request.Name}
Email: {request.Email}

Message:
{request.Message}
"
                };

                mail.To.Add(_emailSettings.Email);

                var smtp = new SmtpClient(_emailSettings.Host, _emailSettings.Port)
                {
                    Credentials = new NetworkCredential(
                        _emailSettings.Email,
                        _emailSettings.Password
                    ),
                    EnableSsl = true,
                    UseDefaultCredentials = false
                };

                await smtp.SendMailAsync(mail);

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    error = ex.Message
                });
            }
        }
    }
}