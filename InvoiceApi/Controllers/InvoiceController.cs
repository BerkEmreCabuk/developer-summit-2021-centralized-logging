using DeveloperSummit.Core.Infrastructure.RabbitMq;
using DeveloperSummit.InvoiceApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DeveloperSummit.InvoiceApi.Controllers
{
    [ApiController]
    [Route("api/invoices")]
    public class InvoiceController : Controller
    {
        private readonly IRabbitMqService _rabbitMqService;
        private readonly ILogger<InvoiceController> _logger;
        public InvoiceController(
            IRabbitMqService rabbitMqService,
            ILogger<InvoiceController> logger)
        {
            _rabbitMqService = rabbitMqService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateInvoiceRequestModel model)
        {
            //create invoice
            //..
            if (model.OrderId % 2 == 0)
            {
                _logger.LogError("Invoice not created becasuse is dublicated");
                return BadRequest(0);
            }
            else
            {
                _logger.LogInformation("Invoice successfuly create");
            }

            await _rabbitMqService.PublishAsync("cargo.notify", model.OrderId);
            return Created("", 1);
        }
    }
}