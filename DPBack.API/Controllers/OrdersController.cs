
using System.Security.Claims;
using DPBack.Application.Contracts;
using DPBack.Application.Interfaces;

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

        public OrdersController(IOrdersService service, IPriceCalcService priceCalcService)
        {
            _service = service;
            _priceCalcService = priceCalcService;
        }
        
        [HttpGet("get")]
        public async Task<ActionResult<List<OrdersResponse>>> GetOrdersAsync()
        {
            var orders = await _service.GetAllOrders();
            var response = orders.Select(o =>
                new OrdersResponse
                {
                    id = o.Id,
                    OrderNumber = o.OrderNumber,
                    Desc = o.Description,
                    Price = o.TotalPrice,
                    Items = o.Items.Select(i => new OrderItemResponse
                    {
                        Quantity = i.Quantity,
                        Type = i.Type,
                        Options = i.Options
                    }).ToList(),
                    History = o.History.Select(h => new OrderHistoryElementResponse
                    {
                        Status = h.Status.ToString(),
                        ChangedAt = h.ChangedAt,
                        AuthorId = h.AuthorLogin
                    }).ToList(),
                    AssignedTo = o.AssignedTo,
                    CreatedAt = o.CreatedAt,
                    IsSuspended = o.IsSuspended,
                    Status = o.Status,
                    PaymentStatus = o.PaymentStatus
                });
            return Ok(response);
        }

        [HttpPost("{id}/assign")]
        [Authorize]
        public async Task<ActionResult> AssignOrderTo(Guid id, [FromBody] AssignOrderRequest request)
        {
            await _service.AssignToAsync(id, request.AuthorLogin);
            return Ok();
        }
        [HttpPost("{id}/setStatus")]
        [Authorize]
        public async Task<ActionResult> ChangeOrderStatus(Guid id,[FromBody] ChangeOrderStatusRequestDto request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                throw new Exception("no active user found");
            }
            await _service.ChangeStatus(id, userId, request.Status);
            return Ok();
        }

        [HttpPost("getPrice")]
        public async Task<ActionResult<PriceResultDto>> GetPricePerUnit([FromBody] GetPriceDto request)
        {
           return  await _priceCalcService.CalculatePrice(request);
        }
        [HttpPost("create")]
        public async Task<ActionResult<Guid>> CreateOrder([FromBody] OrdersRequest request)
        {
            var response = await _service.CreateOrder(request);
            return Ok(response);
        }
    }
}