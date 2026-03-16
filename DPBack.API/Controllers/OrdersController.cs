using System.Security.Claims;
using DPBack.Application.Contracts;
using DPBack.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DPBack.API.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _service;
        private readonly IPriceCalcService _priceCalcService;
        private Guid GetCurrentUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                throw new UnauthorizedAccessException("No active user found");
            }
            return Guid.Parse(userId);
        }
        public OrdersController(IOrdersService service, IPriceCalcService priceCalcService)
        {
            _service = service;
            _priceCalcService = priceCalcService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Worker")]
        // Use with caution (could be a lot of data)
        public async Task<ActionResult<List<OrderResponseDto>>> GetOrdersAsync()
        {
            var result = await _service.GetAllOrders();
            
            return Ok(result);
        }

        public async Task<ActionResult<List<OrderResponseDto>>> GetOrdersFiltered(OrdersFilteredRequestDto request)
        {
            return Ok();
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<OrderResponseDto>> GetOrderByIdAsync(Guid orderId)
        {
            var userId = GetCurrentUserId();
            var result = await _service.GetOrderById(userId,orderId);
            return Ok(result);
        }
        [HttpPut("{id}/assigned")]
        [Authorize(Roles = "Admin, Worker")]
        public async Task<ActionResult> AssignOrderTo(Guid id, [FromBody] AssignOrderRequest request)
        {
            await _service.AssignToAsync(id, request.AuthorLogin);
            return Ok();
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin, Worker")]
        public async Task<ActionResult> ChangeOrderStatus(Guid id, [FromBody] ChangeOrderStatusRequestDto request)
        {
            var userId = GetCurrentUserId();

            await _service.ChangeStatus(id, userId.ToString(), request.Status);
            return Ok();
        }
      
        [HttpPost]
        public async Task<ActionResult<CreateOrderResponseDto>> CreateOrder([FromBody] CreateOrderRequestDto request)
        {
            var response = await _service.CreateOrder(request);
            return Ok(response);
        }

        [HttpGet("{id}/paymentStatus")]
        public async Task<ActionResult<GetOrderPaymentStatusDto>> GetOrderPaymentStatus(Guid id)
        {
            var userId = GetCurrentUserId();
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var status = await _service.GetOrderStatus(id);
            var response = new GetOrderPaymentStatusDto
                { PaymentStatus = status };
            return Ok(response);
        }
    }
}