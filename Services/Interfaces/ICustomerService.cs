using LogiDispatchWeb.Models.DTOs;

namespace LogiDispatchWeb.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<List<CustomerDto>> GetAllCustomersAsync();
    }
}
