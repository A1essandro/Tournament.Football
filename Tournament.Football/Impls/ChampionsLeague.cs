using System;
using System.Collections.Generic;
using System.Linq;
using Tournament.Contracts;
using Tournament.Football.Contracts;
using Tournament.Football.Impls.Internal;

namespace Tournament.Football;

public class ChampionsLeague : FootballStageBase<ChampionsLeagueResult>
{

    private readonly OnStartedInvoker<ChampionsLeagueResult> _onStartedInvoker;

    public ChampionsLeague(Action<IEnumerable<GroupStage>> groupsDraw, Action<IEnumerable<KnockoutStage>, IEnumerable<Team>> playoffDraw)
    {
        var groupsStage = new GroupsStage(
            new GroupStage("A", DateTime.Today),
            new GroupStage("B", DateTime.Today.AddDays(1)),
            new GroupStage("C", DateTime.Today),
            new GroupStage("D", DateTime.Today.AddDays(1)));
        var quater = new PlayoffStage(
            new KnockoutStage(DateTime.Today.AddDays(90)),
            new KnockoutStage(DateTime.Today.AddDays(90)),
            new KnockoutStage(DateTime.Today.AddDays(91)),
            new KnockoutStage(DateTime.Today.AddDays(91)));
        var semi = new PlayoffStage(
            new KnockoutStage(DateTime.Today.AddDays(110)),
            new KnockoutStage(DateTime.Today.AddDays(111)));
        var final = new FinalStage(DateTime.Today);

        Stages = new List<IStage<IFootballMatch<Team>, IFootballMatchResult<Team>, int, object, Team>>
            {
                groupsStage,
                quater,
                semi,
                final
            };

        groupsStage.OnCompleted += stage =>
        {
            PlayoffDraw(quater.Stages.OfType<KnockoutStage>(), (stage as GroupsStage).Result.MadeIt);
        };
        quater.OnCompleted += stage =>
        {
            PlayoffDraw(semi.Stages.OfType<KnockoutStage>(), (stage as PlayoffStage).Result.MadeIt);
        };
        semi.OnCompleted += stage =>
        {
            var playoffStage = stage as PlayoffStage;
            if (!playoffStage.IsCompleted)
            {
                throw new Exception();
            }
            var participantPlaces = final.ParticipantPlaces.ToArray();
            participantPlaces[0].Participant = playoffStage.Result.MadeIt.First();
            participantPlaces[1].Participant = playoffStage.Result.MadeIt.Last();
        };
        final.OnCompleted += stage =>
        {
            OnCompleted?.Invoke(this);
        };

        groupsDraw(Stages.First().Stages.OfType<GroupStage>());
        _onStartedInvoker = new OnStartedInvoker<ChampionsLeagueResult>(this, () => OnStarted?.Invoke(this));
        Result = new ChampionsLeagueResult(this);
        PlayoffDraw = playoffDraw;
    }

    public override IEnumerable<IStage<IFootballMatch<Team>, IFootballMatchResult<Team>, int, object, Team>> Stages { get; }

    public override IEnumerable<IFootballMatch<Team>> Schedule
    {
        get
        {
            foreach (var stage in Stages)
            {
                foreach (var game in stage.Schedule)
                {
                    yield return game;
                }
            }
        }
    }

    public override ChampionsLeagueResult Result { get; }

    public override bool IsCompleted => Result.IsCompleted;

    public override IReadOnlyList<IParticipantPlace<Team>> ParticipantPlaces => throw new System.NotImplementedException();

    public Action<IEnumerable<KnockoutStage>, IEnumerable<Team>> PlayoffDraw { get; }

    public override event Action<IStage> OnCompleted;

    public override event Action<IStage> OnStarted;

}
