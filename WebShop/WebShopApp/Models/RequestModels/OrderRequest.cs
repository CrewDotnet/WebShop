namespace WebShopApp.Models.RequestModels
{
    public class OrderRequest
    {
        public Guid CustomerId { get; set; } //strani kljuc
        public List<Guid> ClothesItemsId { get; set; } = new List<Guid>();
    }
}