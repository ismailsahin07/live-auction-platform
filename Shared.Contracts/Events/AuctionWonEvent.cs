namespace Shared.Contracts.Events;

public record AuctionWonEvent(
    string AuctionId, 
    string WinnerId, 
    decimal WinningAmount, 
    DateTimeOffset EndDate 
);
