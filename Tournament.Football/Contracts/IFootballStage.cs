using Tournament.Contracts;

namespace Tournament.Football.Contracts;

public interface IFootballStage<TResult, TFootballTeam> : IStage<IFootballMatch<TFootballTeam>, IFootballMatchResult<TFootballTeam>, int, TResult, TFootballTeam>
    where TFootballTeam : IParticipant
{

}
