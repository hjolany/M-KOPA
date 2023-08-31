using Microsoft.AspNetCore.Mvc;
using SMSMicroService.Entities.Domains;
using SMSMicroService.Gateway.Interface;
using SMSMicroService.Helpers;
using SMSMicroService.Infrastructures.Enums;

namespace SMSMicroService.Controllers
{
    [ApiController]
    [Route("api/v1/queue/")]
    [Produces("application/json")]
    public class QueueApiController : ControllerBase
    {
        private readonly IMessageQueueGateway<MessageDomain> _messageQueueGateway;
        private readonly IMessageTableGateway _messageGateway;

        public QueueApiController(IMessageQueueGateway<MessageDomain> rabbitMainMessageQueueGateway,
            IMessageTableGateway messageGateway)
        {
            _messageQueueGateway = rabbitMainMessageQueueGateway;
            _messageGateway = messageGateway;
        }


        [HttpPost("send")]
        public IActionResult Send(MessageDomain domain)
        {
            for (int i = 0; i < int.Parse(AppConfig.Get("Dummy:Count")); i++)
            {
                _messageQueueGateway.EnQueue(domain);
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
        public async Task<IActionResult> GetSuccessCount([FromQuery] ESuccessTime successTime = ESuccessTime.First)
        {
            var data = await _messageGateway.GetAll(p => 
                p.RetryCount == (successTime == ESuccessTime.First ? 1 : (successTime == ESuccessTime.Second ? 2 : p.RetryCount)) &&
                p.Status == EStatus.Success);
            return Ok(data.Count());
        }

        [HttpGet("count/failed")]
        public async Task<IActionResult> GetFailedCount()
        {
            var data = await _messageGateway.GetAll(p => p.Status == EStatus.Failed);
            return Ok(data.Count());
        }

        [HttpGet("success")]
        public async Task<IActionResult> GetSuccess([FromQuery] ESuccessTime successTime = ESuccessTime.First)
        {
            var data = await _messageGateway.GetAll(p =>

                p.RetryCount == (successTime == ESuccessTime.First ? 1 : (successTime == ESuccessTime.Second ? 2 : p.RetryCount)) &&
                p.Status == EStatus.Success);
            return Ok(data.ToList());
        }

        [HttpGet("failed")]
        public async Task<IActionResult> GetFailed()
        {
            var data = await _messageGateway.GetAll(p => p.Status == EStatus.Failed);
            return Ok(data.ToList());
        }

        [HttpGet("consumer/count")]
        public async Task<IActionResult> GetCount()
        {
            var cnt = await _messageQueueGateway.ConsumerCount().ConfigureAwait(false);
            return Ok(cnt);
        }
    }
}
