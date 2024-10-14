using Apollo.Abstractions;

namespace PlanetGame.Contracts.Ministry.Mining;

public record MiningRightsRequestedEvent : IEvent
{
    public long RequesterId { get; init; }
    public long MineId { get; init; }
}