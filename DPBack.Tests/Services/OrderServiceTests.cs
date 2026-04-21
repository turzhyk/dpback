using System.Text.Json;
using DPBack.Application.Abstractions;
using DPBack.Application.Contracts;
using DPBack.Application.Services;
using DPBack.Domain.Enums;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DPBack.Tests.Services;

public class OrderServiceTests
{
    private readonly Mock<IOrdersRepository> _mockRepository;
    private readonly Mock<ILogger<OrdersService>> _mockLogger;
    private readonly Mock<IPaymentService> _mockPaymentService;
    private readonly Mock<IPriceCalcService> _mockCalculator;
    private readonly IOrdersService _service;

    public OrderServiceTests()
    {
        _mockRepository = new Mock<IOrdersRepository>();
        _mockLogger = new Mock<ILogger<OrdersService>>();
        _mockCalculator = new Mock<IPriceCalcService>();
        _mockPaymentService = new Mock<IPaymentService>();
        _service = new OrdersService(_mockRepository.Object, _mockPaymentService.Object, _mockCalculator.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task CreateOrder_ShouldReturnResponseDto()
    {
        var orderDto = new CreateOrderRequestDto("Test order", Guid.NewGuid(), new List<OrderItemRequest>()
        {
            new OrderItemRequest(1, OrderItemType.Businesscard, new JsonElement())
        });

        _mockPaymentService.Setup(x =>
            x.CreatePayment(It.IsAny<string>(), It.IsAny<decimal>())).ReturnsAsync("link");
        _mockCalculator.Setup(x =>
            x.Calculate(It.IsAny<OrderItemRequest>())).Returns(10m);

        var result = await _service.CreateOrder(Guid.NewGuid(), orderDto, CancellationToken.None);
        
        Assert.NotEmpty(result.PaymentUrl);
        Assert.NotEqual(Guid.Empty,result.OrderId);
    }
}