using System;
using System.Collections.Generic;
using System.Linq;
using Tournament.Contracts;
using Tournament.Football.Contracts;

namespace Tournament.Football;

public class FinalStage : FootballStageBase<KnockoutStageResult>
{

    public FinalStage(DateTime date)
    {
        var participants = new[] { new ParticipantPlace(), new ParticipantPlace() };
        ParticipantPlaces = participants;

        var game = new FootballMatch(participants[0], participants[1], date, this);
        Schedule = new[] { game };

        Result = new KnockoutStageResult(this, awayGoalRule: false);

        game.OnCompleted += g =>
        {
            OnStarted?.Invoke(this);
            if (Result.MadeIt != null)
                OnCompleted?.Invoke(this);
            else
            {
                (g.Result as FootballGameResult).SetPenalties();
                System.Console.WriteLine($"Penalties! {g.Result.Penalties.Item1}:{g.Result.Penalties.Item2}");
            }

            Result.MadeIt.Rating += 0.2;
            Result.MadeIt.Winner++;
        };
    }

    public FinalStage(Team t1, Team t2, DateTime date) : this(date)
    {
        var participants = ParticipantPlaces.ToArray();
        participants[0].Participant = t1;
        participants[1].Participant = t2;
    }

    public override IEnumerable<IStage<IFootballMatch<Team>, IFootballMatchResult<Team>, int, object, Team>> Stages { get; }

    public override IEnumerable<FootballMatch> Schedule { get; }

    public override KnockoutStageResult Result { get; }

    public override bool IsCompleted => Schedule.All(x => x.HasResult);

    public override IReadOnlyList<IParticipantPlace<Team>> ParticipantPlaces { get; }

    public override event Action<IStage> OnCompleted;

    public override event Action<IStage> OnStarted;
}
