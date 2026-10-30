namespace NovaCart.Application.DTOs
{
    // DTO لعرض عنصر الطلب للعميل
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }

    // DTO لعرض الطلب كاملاً
    public class OrderDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; } = string.Empty;
        public string ShippingAddress { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new();
    }

    // DTO لاستقبال طلب الشراء الجديد من العميل (POST)
    public class CreateOrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class CreateOrderDto
    {
        
        public string ShippingAddress { get; set; } = string.Empty;
        public List<CreateOrderItemDto> OrderItems { get; set; } = new();
    }
}