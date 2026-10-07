namespace Shared.Contracts.Events
{
    public record AuctionWonEvent
    {
        public string AuctionId { get; set; } 
        public string WinnerId { get; set; }
        public decimal WinningAmount { get; set; }
        public DateTimeOffset EndDate { get; set; }
    }
}
