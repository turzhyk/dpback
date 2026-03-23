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
        private Guid GetCurrentUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                throw new UnauthorizedAccessException("No active user found");
            }
            return Guid.Parse(userId);
        }
        public OrdersController(IOrdersService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Worker")]
        // Use with caution (could be a lot of data)
        public async Task<ActionResult<List<OrderResponseDto>>> GetOrdersAsync(CancellationToken cToken)
        {
            var result = await _service.GetAllOrders(cToken);
            
            return Ok(result);
        }
        [HttpPost(("paged"))]
        [Authorize]
        public async Task<ActionResult<List<OrderResponseDto>>> GetOrdersFiltered(OrdersFilteredRequestDto request,CancellationToken cToken)
        {
            var respose = await _service.GetOrdersFiltered(request, cToken);
            return Ok(respose);
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<OrderResponseDto>> GetOrderByIdAsync(Guid orderId, CancellationToken cToken)
        {
            var userId = GetCurrentUserId();
            var result = await _service.GetOrderById(userId,orderId, cToken);
            return Ok(result);
        }
        [HttpPut("{id}/assigned")]
        [Authorize(Roles = "Admin, Worker")]
        public async Task<ActionResult> AssignOrderTo(Guid id, [FromBody] AssignOrderRequest request, CancellationToken cToken)
        {
            await _service.AssignToAsync(id, request.AuthorLogin, cToken);
            return Ok();
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin, Worker")]
        public async Task<ActionResult> ChangeOrderStatus(Guid id, [FromBody] ChangeOrderStatusRequestDto request, CancellationToken cToken)
        {
            var userId = GetCurrentUserId();

            await _service.ChangeStatus(id, userId.ToString(), request.Status, cToken);
            return Ok();
        }
      
        [HttpPost]
        public async Task<ActionResult<CreateOrderResponseDto>> CreateOrder([FromBody] CreateOrderRequestDto request, CancellationToken cToken)
        {
            // var userId = GetCurrentUserId();
            var response = await _service.CreateOrder(request, cToken);
            return Ok(response);
        }

        [HttpGet("{id}/paymentStatus")]
        public async Task<ActionResult<GetOrderPaymentStatusDto>> GetOrderPaymentStatus(Guid id, CancellationToken cToken)
        {
            // var userId = GetCurrentUserId();
            // var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var status = await _service.GetPaymentStatus(id, cToken);
            var response = new GetOrderPaymentStatusDto
                { PaymentStatus = status };
            return Ok(response);
        }
    }
}