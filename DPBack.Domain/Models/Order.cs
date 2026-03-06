using System;
using System.Collections.Generic;
using System.Text;

namespace DPBack.Domain.Models
{
    public enum OrderStatus
    {
        New, // заказ создан
        InProgress, // оператор работает
        Produced, // продукция изготовлена
        Packing, // упаковка
        ReadyForShipping, // готов к отправке
        InDelivery, // у курьера
        Done, // доставлено
        Cancelled // отменено
    }

    public enum OrderPaymentStatus
    {
        Waiting,
        Paid,
        Cancelled
    }

    public class Order
    {
        public Order(Guid id, 
            int orderNumber, 
            string description,
            decimal price,
            List<OrderItem> items,
            string assignedTo,
            DateTime createdAt,
            bool isSuspended,
            OrderStatus status,
            OrderPaymentStatus paymentStatus,
            List<OrderHistoryElement> history)
        {
            Id = id;
            OrderNumber = orderNumber;
            Description = description;
            TotalPrice = price;
            AssignedTo = assignedTo;
            CreatedAt = createdAt;
            Items = items;
            IsSuspended = isSuspended;
            Status = status;
            PaymentStatus = paymentStatus;
            History = history;
        }

        public Guid Id { get; }
        public int OrderNumber { get; }
        public string Description { get; }

        public decimal TotalPrice { get; }
        public string AssignedTo { get; }

        public bool IsSuspended { get; }
        public OrderStatus Status { get; }
        public OrderPaymentStatus PaymentStatus { get; }

        public DateTime CreatedAt { get; }
        public List<OrderItem> Items { get; }

        public List<OrderHistoryElement> History { get; set; }
            = new();

        public void ChangeStatus(string status, string author)
        {
            // History.Add(new OrderHistoryElement(
            //     status,
            //     author,
            //     DateTime.UtcNow,
            //     
            // ));
        }

        public static (Order Order, string Error) Create(Guid id, int number, string description, decimal price,
            List<OrderItem> items, string assignedTo,
            DateTime createdAt, bool suspended, OrderStatus status, OrderPaymentStatus paymentStatus,
            List<OrderHistoryElement> history)
        {
            var error = string.Empty;
            var order = new Order(id, number, description, price, items, assignedTo, createdAt, suspended, status,
                paymentStatus, history);
            return (order, error);
        }
    }
}