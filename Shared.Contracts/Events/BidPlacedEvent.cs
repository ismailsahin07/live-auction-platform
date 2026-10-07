namespace Shared.Contracts.Events
{
    public record BidPlacedEvent
    {
        public string BidId { get; set; } = Guid.NewGuid().ToString();
        public string AuctionId { get; set; }
        public string BidderId { get; set; }
        public decimal BidAmount { get; set; }           
        public DateTimeOffset BidDate { get; set; }
    }
}
