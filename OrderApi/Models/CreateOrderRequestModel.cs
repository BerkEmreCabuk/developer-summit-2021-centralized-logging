namespace DeveloperSummit.OrderApi.Models
{
    public class CreateOrderRequestModel
    {
        public long UserId { get; set; }
        public long MarketPlaceId { get; set; }
        public long ProductId { get; set; }
        public long CargoId { get; set; }
        public string OrderNumber { get; set; }
        public decimal Quantity { get; set; }
        public string Address { get; set; }
    }
}
