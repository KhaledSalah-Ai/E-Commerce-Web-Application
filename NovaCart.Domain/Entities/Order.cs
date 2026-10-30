namespace NovaCart.Domain.Entities
{
    public class Order : BaseEntity
    {
        public string UserId { get; set; } = string.Empty; // معرف المستخدم صاحب الطلب
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; } = "Pending"; // Pending, Processing, Shipped, Delivered, Cancelled
        public string ShippingAddress { get; set; } = string.Empty;

        // Navigation Property: الطلب الواحد يحتوي على عناصر متعددة
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }

    public class OrderItem : BaseEntity
    {
        // Foreign Key & Navigation Property للطلب
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        // Foreign Key & Navigation Property للمنتج
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}