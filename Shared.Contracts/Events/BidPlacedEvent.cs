namespace Shared.Contracts.Events;

public record BidPlacedEvent(
    string BidId,
    string AuctionId,
    string BidderId,
    decimal BidAmount,
    DateTimeOffset BidDate
);
