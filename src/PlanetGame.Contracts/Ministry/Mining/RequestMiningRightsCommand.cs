using Apollo.Abstractions;

namespace PlanetGame.Contracts.Ministry.Mining;

public record RequestMiningRightsCommand : ICommand
{
    public long RequesterId { get; init; }
    public long MineId { get; init; }
}