namespace WebShopApp.Models.ResponseModels
{
    public class CustomerResponse
    {
        //public List<Customer> Customers {get; set;} = new List<Customer>();
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Name { get; set; }
        public decimal TotalSpent { get; set; } = 0;
    }
}