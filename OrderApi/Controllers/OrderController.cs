using DeveloperSummit.Core.Enums;
using DeveloperSummit.Core.Infrastructure.Http;
using DeveloperSummit.OrderApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace DeveloperSummit.OrderApi.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMainHttpService _mainHttpService;
        private readonly ILogger<OrderController> _logger;
        public OrderController(
            IMainHttpService mainHttpService,
            ILogger<OrderController> logger)
        {
            _mainHttpService = mainHttpService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            return Ok(new { Id = id, OrderNumber = "Order123456", CargoId = 21, ProductId = 2, MarketPlaceId = 14, OrderQuantity = 5 });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequestModel model)
        {
            //validation order
            //..
            _logger.LogInformation("successfuly validation order");

            //check valid cargo
            var cargoResponse = await _mainHttpService.HttpRequest<bool>(HttpServiceEnum.CARGO, $"api/cargos/valid/{model.CargoId}", HttpMethod.Get, null);
            if (!cargoResponse.IsSuccess())
            {
                _logger.LogError("Order failed created because cargo not valid");
                return BadRequest("Order failed created because cargo not valid");
            }
            //create order
            //..
            _logger.LogInformation("Order successfuly created");
            return Created("", model.OrderNumber);
        }

        [HttpPost("packing")]
        public async Task<IActionResult> PackingOrder([FromBody] PackingRequestModel model)
        {
            //others process
            //..
            _logger.LogInformation("successfuly others process order");

            //create invoice
            //..
            var invoiceResponse = await _mainHttpService.HttpRequest<long>(HttpServiceEnum.INVOICE, $"api/invoices", HttpMethod.Post, new { OrderId = model.OrderId });
            if (!invoiceResponse.IsSuccess())
            {
                _logger.LogError("Packing operation failed because invoice not create");
                return BadRequest("Packing operation failed because invoice not create");
            }

            _logger.LogInformation("successfuly packing operation");
            return Ok(model.OrderId);
        }

    }
}
