namespace WebShopApp.Models.RequestModels
{
    public class CustomerRequest
    {
        [System.Text.Json.Serialization.JsonPropertyName("name")]
        public required string Name { get; set; }
    }
}