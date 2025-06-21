using LogiDispatchWeb.Models.DTOs;

namespace LogiDispatchWeb.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetAllAsync();
        Task<OrderDto?> GetByIdAsync(Guid id);
        Task<ApiResponse<OrderDto>?> CreateAsync(CreateOrderRequest request);
        Task<ApiResponse<OrderDto>?> UpdateAsync(UpdateOrderRequest request);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> RestoreAsync(Guid id);
        Task<List<OrderDto>> GetDeletedAsync();
    }
}
