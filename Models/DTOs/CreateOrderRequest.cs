namespace LogiDispatchWeb.Models.DTOs
{
    public class CreateOrderRequest
    {
        public Guid CustomerId { get; set; }
        public List<CreateOrderItemDto> Items { get; set; } = new();
        public decimal OriginLatitude { get; set; }
        public decimal OriginLongitude { get; set; }
        public decimal DestinationLatitude { get; set; }
        public decimal DestinationLongitude { get; set; }
    }

    public class CreateOrderItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
