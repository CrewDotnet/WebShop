namespace WebShopApp.Models.RequestModels
{
    public class ClothesItemRequest
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public Guid ClothesTypeId { get; set; }
    }
}