using LogiDispatchWeb.Models.DTOs;

namespace LogiDispatchWeb.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllProductsAsync();
    }
}
