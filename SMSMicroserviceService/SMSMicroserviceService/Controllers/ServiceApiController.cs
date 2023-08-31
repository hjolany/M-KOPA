using Microsoft.AspNetCore.Mvc;
using SMSMicroService.Entities.Domains;
using SMSMicroService.Gateway;
using SMSMicroService.Services;
using SMSMicroService.UseCases;

namespace SMSMicroService.Controllers
{
    [ApiController]
    [Route("api/service")]
    public class ServiceApiController : ControllerBase
    {
        public ServiceApiController()
        {

        }

        [HttpPost("main/start")]
        public async Task<IActionResult> StartMainService(IServiceCollection serviceCollection)
        {
            serviceCollection.AddHostedService<MainQueueService>();
            return NoContent();
        }

        [HttpPost("main/stop")]
        public async Task<IActionResult> StopMainService()
        {
            //_applicationLifetime.StopApplication();
            //await _mainQueueService.StopAsync(CancellationToken.None);
            return NoContent();
        }
    }
}
