using LogiDispatchWeb.Models.DTOs;
using LogiDispatchWeb.Services.Interfaces;

namespace LogiDispatchWeb.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(IHttpClientFactory factory, ILogger<CustomerService> logger)
        {
            _httpClient = factory.CreateClient("ApiClient");
            _logger = logger;
        }

        public async Task<List<CustomerDto>> GetAllCustomersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("Customers/get-all");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<CustomerDto>>>();
                return result?.Data ?? new List<CustomerDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching customers.");
                return new List<CustomerDto>();
            }
        }
    }

}
