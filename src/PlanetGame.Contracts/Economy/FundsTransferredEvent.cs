using Apollo.Abstractions;

namespace PlanetGame.Contracts.Economy;

public record FundsTransferredEvent : IEvent
{
    public long FromAccountId { get; init; }
    public long ToAccountId { get; init; }
    public decimal Amount { get; init; }
}