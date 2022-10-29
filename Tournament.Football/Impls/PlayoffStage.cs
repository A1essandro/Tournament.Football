using System;
using System.Collections.Generic;
using System.Linq;
using Tournament.Contracts;
using Tournament.Football.Contracts;
using Tournament.Football.Impls.Internal;

namespace Tournament.Football
{
    public class PlayoffStage : FootballStageBase<PlayoffStageResult>
    {

        private readonly OnStartedInvoker<PlayoffStageResult> _onStartedInvoker;

        public PlayoffStage(params KnockoutStage[] stages)
        {
            Stages = stages;
            Result = new PlayoffStageResult(this);

            foreach (var stage in Stages)
            {
                stage.OnCompleted += g =>
                {
                    if (Stages.All(x => x.IsCompleted))
                    {
                        _isCompleted = true;
                        OnCompleted?.Invoke(this);
                    }
                };
            }

            _onStartedInvoker = new OnStartedInvoker<PlayoffStageResult>(this, () => OnStarted?.Invoke(this));
        }

        public override IEnumerable<IStage<IFootballMatch<Team>, IFootballMatchResult<Team>, int, object, Team>> Stages { get; }

        public override IEnumerable<IFootballMatch<Team>> Schedule => Stages.SelectMany(x => x.Schedule).OrderBy(x => x.Date);

        public override PlayoffStageResult Result { get; }


        private bool _isCompleted = false;
        public override bool IsCompleted => _isCompleted;

        public override IReadOnlyList<IParticipantPlace<Team>> ParticipantPlaces => Stages.SelectMany(x => x.ParticipantPlaces).ToList();

        public override event Action<IStage> OnCompleted;

        public override event Action<IStage> OnStarted;

    }

}