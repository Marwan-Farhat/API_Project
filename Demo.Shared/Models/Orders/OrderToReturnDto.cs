using Demo.Shared.Models.Common;

namespace Demo.Shared.Models.Orders
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }
        public required string BuyerEmail { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } 
        public required AddressDto ShippingAddress { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public string PaymentIntentId { get; set; }

        // Navigational Properties Foreign Keys
        public int? DeliveryMethodId { get; set; }

        // Navigational Properties
        public string? DeliveryMethod { get; set; }
        public virtual required ICollection<OrderItemDto> Items { get; set; }
    }
}
