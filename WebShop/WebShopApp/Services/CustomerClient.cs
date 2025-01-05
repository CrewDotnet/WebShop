using System.Net.Http;
using System.Text.Json;
using WebShopApp.Models.RequestModels;

namespace WebShopApp.Services
{
    public class CustomerClient
    {
        private readonly HttpClient _httpClient;

        public CustomerClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CustomerRequest>> GetCustomersFromApiAsync()
        {
            var apiUrl = "https://jsonplaceholder.typicode.com/users";

            var response = await _httpClient.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to fetch customers from API: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();

            var customers = JsonSerializer.Deserialize<List<CustomerRequest>>(content);

            return customers ?? new List<CustomerRequest>();
        }
    }
}

