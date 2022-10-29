using System;
using System.Collections.Generic;
using System.Linq;
using Tournament.Contracts;
using Tournament.Football.Contracts;
using Tournament.Football.Impls.Internal;

namespace Tournament.Football;

public class GroupsStage : FootballStageBase<GroupsStageResult>
{

    private readonly OnStartedInvoker<GroupsStageResult> _onStartedInvoker;
    private object _completedEventLocker = new object();

    public GroupsStage(params GroupStage[] groups)
    {
        Stages = groups;
        Result = new GroupsStageResult(this);
        foreach (var stage in Stages)
        {
            stage.OnCompleted += g =>
            {
                lock (_completedEventLocker)
                {
                    if (Stages.All(x => x.IsCompleted))
                        OnCompleted?.Invoke(this);
                }
            };
        }

        _onStartedInvoker = new OnStartedInvoker<GroupsStageResult>(this, () => OnStarted?.Invoke(this));
    }

    public override IEnumerable<IStage<IFootballMatch<Team>, IFootballMatchResult<Team>, int, object, Team>> Stages { get; }

    public override IEnumerable<IFootballMatch<Team>> Schedule => Stages.SelectMany(x => x.Schedule).OrderBy(x => x.Date);

    public override GroupsStageResult Result { get; }

    public override bool IsCompleted => Schedule.All(x => x.HasResult);

    public override IReadOnlyList<IParticipantPlace<Team>> ParticipantPlaces => Stages.SelectMany(x => x.ParticipantPlaces).ToList();

    public override event Action<IStage> OnCompleted;

    public override event Action<IStage> OnStarted;

}
