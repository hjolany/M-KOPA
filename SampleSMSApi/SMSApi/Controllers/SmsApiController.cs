using Microsoft.AspNetCore.Mvc;
using SMSApi.Boundary.Requests;

namespace SMSApi.Controllers
{
    [Route("api/sms")]
    [ApiController]
    public class SmsApiController : ControllerBase
    {
        private readonly ILogger<SmsApiController> _logger;

        public SmsApiController(ILogger<SmsApiController> logger)
        {
            _logger = logger;
        }

        [HttpPost("send")]
        public Task<IActionResult> Send(MessageDomain domain)
        {
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} request received.");
            var random = new Random();
            return random.Next(10) % 2 == 0 ?
                Task.FromResult<IActionResult>(Ok()) :
                Task.FromResult<IActionResult>(BadRequest("Sample Exception"));
        }

        [HttpPost("resend")]
        public async Task<IActionResult> ReSend(MessageDomain domain)
        {
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} request received again.");
            return Ok();
        }
    }
}
