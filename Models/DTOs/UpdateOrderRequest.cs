namespace LogiDispatchWeb.Models.DTOs
{
    public class UpdateOrderRequest
    {
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public List<OrderItemDto> Items { get; set; } = new();

        public decimal OriginLatitude { get; set; }
        public decimal OriginLongitude { get; set; }

        public decimal DestinationLatitude { get; set; }
        public decimal DestinationLongitude { get; set; }

        public int Status { get; set; }
    }
}
