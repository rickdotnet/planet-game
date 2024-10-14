using Apollo.Abstractions;

namespace PlanetGame.Contracts.Ministry.Mining;

public record MiningRightsGrantedEvent : IEvent
{
    public long RequesterId { get; init; }
    public long MineId { get; init; }
}