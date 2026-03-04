
using DPBack.Application.Contracts;
using DPBack.Application.Interfaces;
using DPBack.Domain.Models;
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

        // [HttpGet("byId")]
        // public async Task<ActionResult<OrdersResponse>> GetOrderByIdAsync()
        // {
        //     
        // }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<OrdersResponse>>> GetOrdersAsync()
        {
            var orders = await _service.GetAllOrders();
            var response = orders.Select(o =>
                new OrdersResponse
                {
                    id = o.Id,
                    Desc = o.Descriprion,
                    Price = o.TotalPrice,
                    Items = o.Items.Select(i => new OrderItemResponse
                    {
                        Quantity = i.Quantity,
                        Type = i.Type,
                        Options = i.Options
                    }).ToList(),
                    History = o.History.Select(h => new OrderHistoryElementResponse
                    {
                        Status = h.Status,
                        ChangedAt = h.ChangedAt,
                        AuthorId = h.AuthorLogin
                    }).ToList(),
                    AssignedTo = o.AssignedTo,
                    CreatedAt = o.CreatedAt,
                    Status = o.Status,
                    PaymentStatus = o.PaymentStatus
                });
            return Ok(response);
        }

        [HttpPost("assign")]
        [Authorize]
        public async Task<ActionResult> AssignOrderTo([FromBody] AssignOrderRequest request)
        {
            await _service.AssignToAsync(request.OrderId, request.AuthorLogin);
            return Ok();
        }

        [HttpPost("price")]
        public async Task<ActionResult<PriceResultDto>> GetPricePerUnit([FromBody] GetPriceDto request)
        {
           return  await _priceCalcService.CalculatePrice(request);
        }
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateOrder([FromBody] OrdersRequest request)
        {
            var items = request.Items.Select(i => new OrderItem
            {
                Id = Guid.NewGuid(),
                Quantity = i.Quantity,
                Type = i.Type,
                Options = i.Options,
            }).ToList();
            var (order, error) = Order.Create(
                Guid.NewGuid(),
                request.Desc,
                request.Price,
                items,
                "",
                DateTime.UtcNow,
                status: OrderStatus.New,
                paymentStatus: OrderPaymentStatus.Waiting,
                null
            );
            await _service.CreateOrder(order);
            return Ok(order.Id);
        }
    }
}