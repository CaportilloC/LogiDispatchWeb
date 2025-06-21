using LogiDispatchWeb.Models.DTOs;
using LogiDispatchWeb.Services.Interfaces;

namespace LogiDispatchWeb.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IHttpClientFactory factory, ILogger<ProductService> logger)
        {
            _httpClient = factory.CreateClient("ApiClient");
            _logger = logger;
        }

        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("Products/get-all");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<ProductDto>>>();
                return result?.Data ?? new List<ProductDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching products.");
                return new List<ProductDto>();
            }
        }
    }
}
