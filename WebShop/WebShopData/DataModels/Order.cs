namespace WebShopData.Models
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CustomerId { get; set; } //strani kljuc
        public Customer? Customer { get; set; } // navigation property - direktan pristup objektu Customer iz Ordera.
        public List<ClothesItem> ClothesItems { get; set; } = new List<ClothesItem>(); // Navigation property za many-to-many relationship
        public decimal TotalPrice { get; set; }
    }
}