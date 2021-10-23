using DeveloperSummit.Core.Extensions;
using DeveloperSummit.Core.Infrastructure.RabbitMq;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Context;
using System.Threading.Tasks;

namespace DeveloperSummit.CargoApi.Controllers
{
    [ApiController]
    [Route("api/cargos")]
    public class CargoController : Controller
    {
        private readonly ILogger<CargoController> _logger;
        public CargoController(
            IRabbitMqService rabbitMqService,
            ILogger<CargoController> logger)
        {
            _logger = logger;
        }

        [NonAction]
        [CapSubscribe("cargo.notify")]
        public async Task Notify(long orderId, [FromCap] CapHeader header)
        {
            //notify cargo company
            //..
            _logger.CustomLog(header, orderId, "successfuly notification to cargo");
        }

        [HttpGet("valid/{id}")]
        public async Task<ActionResult> Valid(long id)
        {
            if (id % 2 == 0)
            {
                _logger.LogInformation("Cargo is not valid {@id}", id);
                return Ok(false);
            }
            else
            {
                _logger.LogInformation("Cargo is valid {@id}", id);
                return Ok(true);
            }
        }

    }
}