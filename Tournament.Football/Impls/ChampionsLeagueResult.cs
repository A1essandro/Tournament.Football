using System.Linq;

namespace Tournament.Football;

public class ChampionsLeagueResult
{

    public ChampionsLeagueResult(ChampionsLeague stage) => Stage = stage;

    public bool IsCompleted => Stage.Stages.Last().IsCompleted;

    public ChampionsLeague Stage { get; }

}
