using CQRS.Application;
using CQRS.ReadModel;
using Microsoft.AspNetCore.Mvc;

namespace CQRS.Api.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly CommandHandler _commandHandler;
        private readonly IOrderReadRepository _orderReadRepository;

        public OrderController(CommandHandler commandHandler, IOrderReadRepository orderReadRepository)
        {
            _commandHandler = commandHandler;
            _orderReadRepository = orderReadRepository;
        }

        [HttpPost]
        public IActionResult CreateOrder([FromBody] CreateOrderCommand command)
        {
            _commandHandler.Handle(command);
            return Ok("Order Created");
        }

        [HttpPost("{orderId}/items")]
        public IActionResult AddItem(Guid orderId, [FromBody] AddItemCommand command)
        {
            command.OrderId = orderId;
            _commandHandler.Handle(command);
            return Ok("Item Added");
        }

        [HttpPost("{orderId}/submit")]
        public IActionResult SubmitOrder(Guid orderId)
        {
            var command = new SubmitOrderCommand { OrderId = orderId };
            _commandHandler.Handle(command);
            return Ok("Order submitted");
        }

        [HttpPost("{orderId}/ship")]
        public IActionResult ShipOrder(Guid orderId)
        {
            var command = new ShipOrderCommand { OrderId = orderId };
            _commandHandler.Handle(command);
            return Ok("Order Shipped");
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder(Guid orderId)
        {
            var order = _orderReadRepository.Get(orderId);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = _orderReadRepository.GetAllAsync();
            return Ok(orders);
        }

        [HttpGet("history/{orderId}")]
        public async Task<IActionResult> GetOrderHistory(Guid orderId)
        {
            var orders = _commandHandler.GetHistory(orderId);
            return Ok(orders);
        }

    }
}
