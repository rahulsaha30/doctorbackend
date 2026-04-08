using doctor.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace clinicdoctor.Controllers
{
    [ApiController]
    [Route("api/email")]
    public class EmailController : ControllerBase
    {
        private readonly EmailSettings _emailSettings;

        public EmailController(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail([FromBody] ContactRequest request)
        {
            try
            {
                var client = new HttpClient();

                // ✅ Add API key properly
                client.DefaultRequestHeaders.Remove("api-key");
                client.DefaultRequestHeaders.Add("api-key", _emailSettings.Password?.Trim());

                // ✅ Proper JSON (NO manual string)
                var emailData = new
                {
                    sender = new { email = _emailSettings.Email },
                    to = new[]
                    {
                        new { email = _emailSettings.Email }
                    },
                    subject = $"New Message from {request.Name}",
                    htmlContent = $@"
                        <p><b>Name:</b> {request.Name}</p>
                        <p><b>Email:</b> {request.Email}</p>
                        <p><b>Message:</b> {request.Message}</p>
                    "
                };

                var json = JsonSerializer.Serialize(emailData);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(
                    "https://api.brevo.com/v3/smtp/email",
                    content
                );

                var result = await response.Content.ReadAsStringAsync();

                // ✅ Return full debug info (useful now)
                return Ok(new
                {
                    success = response.IsSuccessStatusCode,
                    brevoResponse = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    error = ex.Message,
                    inner = ex.InnerException?.Message
                });
            }
        }
    }
}