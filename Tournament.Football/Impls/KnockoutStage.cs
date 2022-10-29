using System;
using System.Collections.Generic;
using System.Linq;
using Tournament.Contracts;
using Tournament.Football.Contracts;
using Tournament.Football.Impls.Internal;

namespace Tournament.Football
{
    public class KnockoutStage : FootballStageBase<KnockoutStageResult>
    {

        private readonly OnStartedInvoker<KnockoutStageResult> _onStartedInvoker;

        public KnockoutStage(Team t1, Team t2, DateTime date) : this(date)
        {
            ParticipantPlaces[0].Participant = t1;
            ParticipantPlaces[1].Participant = t2;
        }

        public KnockoutStage(DateTime date)
        {
            var participants = new[] { new ParticipantPlace(), new ParticipantPlace() };
            ParticipantPlaces = participants;

            var secondGame = new FootballMatch(participants[1], participants[0], date.AddDays(7), this);
            secondGame.OnResultSet += g =>
            {
                if (!Result.IsCompleted)
                {
                    (g.Result as FootballGameResult).SetPenalties();
                    System.Console.WriteLine($"Penalties! {g.Result.Penalties.Item1}:{g.Result.Penalties.Item2}");
                }
            };
            secondGame.OnCompleted += g =>
            {
                OnCompleted?.Invoke(this);
            };

            Schedule = new[]{
                new FootballMatch(participants[0], participants[1], date, this),
                secondGame
            };

            _onStartedInvoker = new OnStartedInvoker<KnockoutStageResult>(this, () => OnStarted?.Invoke(this));

            Result = new KnockoutStageResult(this);
        }


        public override IEnumerable<IStage<IFootballMatch<Team>, IFootballMatchResult<Team>, int, object, Team>> Stages { get; }

        public override IEnumerable<FootballMatch> Schedule { get; }

        public override KnockoutStageResult Result { get; }

        public override bool IsCompleted => Schedule.All(x => x.HasResult);

        public override IReadOnlyList<IParticipantPlace<Team>> ParticipantPlaces { get; }

        public override event Action<IStage> OnCompleted;

        public override event Action<IStage> OnStarted;

    }

}