namespace WebShopApp.Models.ResponseModels
{
    public class CustomerResponse
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Name { get; set; }
        public decimal TotalSpent { get; set; } = 0;
    }
}