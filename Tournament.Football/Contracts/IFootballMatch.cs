using Tournament.Contracts;

namespace Tournament.Football.Contracts;

public interface IFootballMatch<TFootballTeam> : IGame<IFootballMatch<TFootballTeam>, IFootballMatchResult<TFootballTeam>, int, TFootballTeam>
    where TFootballTeam : IParticipant
{

}
