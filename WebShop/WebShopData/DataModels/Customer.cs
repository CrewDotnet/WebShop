namespace WebShopData.Models
{
    public class Customer
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Name { get; set; }
        public decimal TotalSpent { get; set; } = 0;
        public bool HasDiscount { get; set; } = false;
        public int OrdersCount { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>(); // navigation property - pristup porudzbinama preko kupca
    }
}