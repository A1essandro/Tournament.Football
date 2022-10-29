using Tournament.Contracts;

namespace Tournament.Football.Contracts;

public interface IFootballMatchResult<out TFootballTeam> : IGameResult<int, TFootballTeam>, ICanBeADraw
    where TFootballTeam : IParticipant
{

    IPair<int> Penalties { get; set; }

}
