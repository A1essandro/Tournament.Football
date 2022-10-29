using System;
using System.Collections.Generic;
using System.Linq;
using Tournament.Contracts;
using Tournament.Football.Contracts;

namespace Tournament.Football;

public abstract class FootballStageBase<TResult> : IFootballStage<TResult, Team>
{
    public IStage Context { get; }

    public abstract IEnumerable<IStage<IFootballMatch<Team>, IFootballMatchResult<Team>, int, object, Team>> Stages { get; }

    public abstract IEnumerable<IFootballMatch<Team>> Schedule { get; }

    public abstract TResult Result { get; }

    public abstract bool IsCompleted { get; }

    public abstract IReadOnlyList<IParticipantPlace<Team>> ParticipantPlaces { get; }

    public virtual bool IsStarted => Schedule.Any(x => x.HasResult);

    public abstract event Action<IStage> OnCompleted;

    public abstract event Action<IStage> OnStarted;

}
