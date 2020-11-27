using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TestTravelPavlo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AgenciesController : ControllerBase
    {

        private readonly ILogger<AgenciesController> _logger;

        public AgenciesController(ILogger<AgenciesController> logger)
        {
            _logger = logger;
        }

    }
}
