using Microsoft.AspNetCore.Mvc;
using SMSMicroService.Entities.Domains;
using SMSMicroService.Gateway.Interface;
using SMSMicroService.Infrastructures.Enums;

namespace SMSMicroService.Controllers
{
    [ApiController]
    [Route("api/v1/sms")]
    [Produces("application/json")]
    public class SmsServiceApiController : ControllerBase
    {
        private readonly IRabbitMainMessageQueueGateway<MessageDomain> _rabbitMainMessageQueueGateway;
        private readonly IMessageGateway _messageGateway;
         
        public SmsServiceApiController(IRabbitMainMessageQueueGateway<MessageDomain> rabbitMainMessageQueueGateway,
            IMessageGateway messageGateway)
        {
            _rabbitMainMessageQueueGateway = rabbitMainMessageQueueGateway;
            _messageGateway = messageGateway;
        }


        [HttpPost("queue/send")]
        public IActionResult Send(MessageDomain domain)
        {
            for (int i = 0; i < 5000; i++)
            {
                _rabbitMainMessageQueueGateway.EnQueue(domain);
            }
            return StatusCode((int)StatusCodes.Status201Created, domain);
        }

        [HttpGet("count/all")]
        public async Task<IActionResult> GetAllDb()
        {
            var data = await _messageGateway.GetAll();
            return Ok(data.Count());
        }

        [HttpGet("count/success")]
        public async Task<IActionResult> GetSuccessCount()
        {
            var data = await _messageGateway.GetAll(p => p.Status == EStatus.Success);
            return Ok(data.Count());
        }

        [HttpGet("count/failed")]
        public async Task<IActionResult> GetFailedCount()
        {
            var data = await _messageGateway.GetAll(p => p.Status == EStatus.Failed);
            return Ok(data.Count());
        }

        [HttpGet("success")]
        public async Task<IActionResult> GetSuccess()
        {
            var data = await _messageGateway.GetAll(p => p.Status == EStatus.Success);
            return Ok(data.ToList());
        }

        [HttpGet("failed")]
        public async Task<IActionResult> GetFailed()
        {
            var data = await _messageGateway.GetAll(p => p.Status == EStatus.Failed);
            return Ok(data.ToList());
        }
    }
}
