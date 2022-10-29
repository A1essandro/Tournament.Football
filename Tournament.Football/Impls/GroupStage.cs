using System;
using System.Collections.Generic;
using System.Linq;
using Tournament.Contracts;
using Tournament.Football.Contracts;
using Tournament.Football.Impls.Internal;

namespace Tournament.Football
{

    public sealed class GroupStage : FootballStageBase<GroupStageResult>
    {

        private readonly OnStartedInvoker<GroupStageResult> _onStartedInvoker;
        private object _completedEventLocker = new object();

        public GroupStage(string name, Team t1, Team t2, Team t3, Team t4, DateTime startDate)
            : this(name, startDate)
        {
            ParticipantPlaces[0].Participant = t1;
            ParticipantPlaces[1].Participant = t2;
            ParticipantPlaces[2].Participant = t3;
            ParticipantPlaces[3].Participant = t4;
        }

        public GroupStage(string name, DateTime startDate)
        {
            Name = name;
            var participants = new[] { new ParticipantPlace(), new ParticipantPlace(), new ParticipantPlace(), new ParticipantPlace() };
            ParticipantPlaces = participants;

            Schedule = CreateSchedule(startDate, participants);
            Schedule.Last().OnCompleted += game =>
            {
                OnCompleted?.Invoke(this);
            };

            _onStartedInvoker = new OnStartedInvoker<GroupStageResult>(this, () => OnStarted?.Invoke(this));
            Result = new GroupStageResult(this);
        }

        public override IEnumerable<IStage<IFootballMatch<Team>, IFootballMatchResult<Team>, int, object, Team>> Stages { get; }

        public override IEnumerable<FootballMatch> Schedule { get; }

        public override GroupStageResult Result { get; }

        public override IReadOnlyList<IParticipantPlace<Team>> ParticipantPlaces { get; }

        public override bool IsCompleted => Schedule.All(x => x.HasResult);

        public string Name { get; }

        public override event Action<IStage> OnCompleted;
        public override event Action<IStage> OnStarted;

        private FootballMatch[] CreateSchedule(DateTime startDate, ParticipantPlace[] participants)
        {
            var twoWeeks = TimeSpan.FromDays(14);

            return new[]
            {
                new FootballMatch(participants[0], participants[1], startDate.Add(twoWeeks * 0), this),
                new FootballMatch(participants[2], participants[3], startDate.Add(twoWeeks * 0), this),

                new FootballMatch(participants[2], participants[0], startDate.Add(twoWeeks * 1), this),
                new FootballMatch(participants[3], participants[1], startDate.Add(twoWeeks * 1), this),

                new FootballMatch(participants[0], participants[3], startDate.Add(twoWeeks * 2), this),
                new FootballMatch(participants[1], participants[2], startDate.Add(twoWeeks * 2), this),

                new FootballMatch(participants[3], participants[0], startDate.Add(twoWeeks * 3), this),
                new FootballMatch(participants[2], participants[1], startDate.Add(twoWeeks * 3), this),

                new FootballMatch(participants[1], participants[0], startDate.Add(twoWeeks * 4), this),
                new FootballMatch(participants[3], participants[2], startDate.Add(twoWeeks * 4), this),

                new FootballMatch(participants[0], participants[2], startDate.Add(twoWeeks * 5), this),
                new FootballMatch(participants[1], participants[3], startDate.Add(twoWeeks * 5), this),
            };
        }

    }
}