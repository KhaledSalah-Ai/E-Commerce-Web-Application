namespace NovaCart.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Navigation Property: التصنيف الواحد يحتوي على عدة منتجات
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}