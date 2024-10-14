using Apollo.Abstractions;

namespace PlanetGame.Contracts.Ministry.Mining;

public record MiningRightsDeniedEvent : IEvent
{
    public long RequesterId { get; init; }
    public long MineId { get; init; }
}