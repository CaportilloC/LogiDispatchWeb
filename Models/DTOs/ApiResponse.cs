namespace LogiDispatchWeb.Models.DTOs
{
    public class ApiResponse<T>
    {
        public bool Succeeded { get; set; }
        public string? Message { get; set; }
        public List<string> Errors { get; set; } = new();
        public T? Data { get; set; }
    }
}
