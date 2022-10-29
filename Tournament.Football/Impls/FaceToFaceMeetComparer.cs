using System;
using System.Collections.Generic;
using System.Linq;
using Tournament.Contracts;
using Tournament.Football.Contracts;

namespace Tournament.Football;

public class FaceToFaceMeetComparer<TStageResult> : IComparer<StageTeamResult<TStageResult>>
{

    public FaceToFaceMeetComparer(IStage<IFootballMatch<Team>, IFootballMatchResult<Team>, int, object, Team> stage, bool awayGoalRule = true)
    {
        Stage = stage;
        AwayGoalRule = awayGoalRule;
    }

    public IStage<IFootballMatch<Team>, IFootballMatchResult<Team>, int, object, Team> Stage { get; }
    public bool AwayGoalRule { get; }

    public int Compare(StageTeamResult<TStageResult> x, StageTeamResult<TStageResult> y)
    {
        var games = Stage.Schedule.Where(g => (g.ParticipantPlaces.Item1.Participant == x.Team && g.ParticipantPlaces.Item2.Participant == y.Team)
            || (g.ParticipantPlaces.Item1.Participant == y.Team && g.ParticipantPlaces.Item2.Participant == x.Team)).ToArray();

        if (games.Last()?.Result?.Penalties?.Item1 != games.Last()?.Result?.Penalties?.Item2)
        {
            if (games.Last()?.Result?.Penalties.Item1 > games.Last()?.Result?.Penalties.Item2)
                return -1;
            return 1;
        }

        var meetsStage = new MeetsStage(games);
        var res = new[] { x, y }.Select(t => new StageTeamResult<TStageResult>(t.Team, meetsStage)).ToArray();

        if (res[0].Points == res[1].Points && res[0].GoalDiff == res[1].GoalDiff)
        {
            if (!AwayGoalRule || res[0].AwayGoals == res[1].AwayGoals)
                return 0;
        }

        var ordering = res.OrderByDescending(x => x.Points)
            .ThenByDescending(x => x.GoalDiff);

        if (AwayGoalRule)
            ordering = ordering.ThenByDescending(x => x.AwayGoals);

        var first = ordering.First();

        if (first.Team == x.Team)
            return -1;

        return 1;
    }

    private class MeetsStage : FootballStageBase<TStageResult>
    {

        public MeetsStage(ICollection<IFootballMatch<Team>> games) => Schedule = games;

        public override IEnumerable<IFootballMatch<Team>> Schedule { get; }

        public override TStageResult Result => default;

        public override bool IsCompleted => false;

        public override IReadOnlyList<IParticipantPlace<Team>> ParticipantPlaces => null;

        public override IEnumerable<IStage<IFootballMatch<Team>, IFootballMatchResult<Team>, int, object, Team>> Stages { get; }

        public override event Action<IStage> OnCompleted;

        public override event Action<IStage> OnStarted;

    }

}
