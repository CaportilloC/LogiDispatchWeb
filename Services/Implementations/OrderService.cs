using LogiDispatchWeb.Models.DTOs;
using LogiDispatchWeb.Services.Interfaces;

namespace LogiDispatchWeb.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IHttpClientFactory clientFactory, ILogger<OrderService> logger)
        {
            _httpClient = clientFactory.CreateClient("ApiClient");
            _logger = logger;
        }

        public async Task<List<OrderDto>> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("Orders/get-all");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<OrderDto>>>();
                return result?.Data ?? new List<OrderDto>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API connection error while fetching orders.");
                return new List<OrderDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching orders.");
                return new List<OrderDto>();
            }
        }

        public async Task<ApiResponse<OrderDto>?> CreateAsync(CreateOrderRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Orders/register", request);

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<OrderDto>>();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Error en creación: {Message}", result?.Message ?? "Error desconocido");
                    return result;
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando la orden.");
                return new ApiResponse<OrderDto> { Succeeded = false, Message = "Error inesperado al crear la orden." };
            }
        }

        public async Task<OrderDto?> GetByIdAsync(Guid id)
        {
            try

            {

                var response = await _httpClient.GetAsync($"Orders/get/{id}");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<OrderDto>>();
                return result?.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching order with ID {OrderId}", id);
                return null;
            }
        }

        public async Task<ApiResponse<OrderDto>?> UpdateAsync(UpdateOrderRequest request)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync("Orders/update", request);

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<OrderDto>>();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Error al actualizar: {Message}", result?.Message ?? "Error desconocido");
                    return result;
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar la orden con ID {OrderId}", request.Id);

                return new ApiResponse<OrderDto>
                {
                    Succeeded = false,
                    Message = "Error inesperado al actualizar la orden."
                };
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"Orders/delete/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting order with ID {OrderId}", id);
                return false;
            }
        }

        public async Task<List<OrderDto>> GetDeletedAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("Orders/deleted");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<OrderDto>>>();
                return result?.Data ?? new List<OrderDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching deleted orders.");
                return new List<OrderDto>();
            }
        }

        public async Task<bool> RestoreAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.PatchAsync($"Orders/restore/{id}", null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error restoring order with ID {OrderId}", id);
                return false;
            }
        }

    }
}
