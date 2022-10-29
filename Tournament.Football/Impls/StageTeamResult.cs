using System.Collections.Generic;
using System.Linq;
using Tournament.Football.Contracts;

namespace Tournament.Football
{
    public class StageTeamResult<TStageResult>
    {

        public StageTeamResult(Team team, FootballStageBase<TStageResult> stage)
        {
            Stage = stage;
            Team = team;
        }

        public FootballStageBase<TStageResult> Stage { get; }

        public Team Team { get; }

        public int Points
        {
            get
            {
                var games = GetGamesForTeam().ToArray();
                return games.Count(x => x.Result.Winner == Team) * 3 + games.Count(x => x.Result.IsDraw);
            }
        }

        public int Goals
        {
            get
            {
                var games = GetGamesForTeam().ToArray();
                return games.Where(x => x.ParticipantPlaces.Item1.Participant == Team).Sum(x => x.Result.Points.Item1)
                     + games.Where(x => x.ParticipantPlaces.Item2.Participant == Team).Sum(x => x.Result.Points.Item2);
            }
        }

        public int AwayGoals
        {
            get
            {
                var games = GetGamesForTeam().ToArray();
                return games.Where(x => x.ParticipantPlaces.Item2.Participant == Team).Sum(x => x.Result.Points.Item2);
            }
        }

        public int HomeGoals
        {
            get
            {
                var games = GetGamesForTeam().ToArray();
                return games.Where(x => x.ParticipantPlaces.Item1.Participant == Team).Sum(x => x.Result.Points.Item1);
            }
        }

        public int ConcededGoals
        {
            get
            {
                var games = GetGamesForTeam().ToArray();
                return games.Where(x => x.ParticipantPlaces.Item1.Participant == Team).Sum(x => x.Result.Points.Item2)
                     + games.Where(x => x.ParticipantPlaces.Item2.Participant == Team).Sum(x => x.Result.Points.Item1);
            }
        }

        public int GoalDiff => Goals - ConcededGoals;

        public override string ToString() => $"{Team}\t|{Goals,4}|{ConcededGoals,4}|{Points,4}";

        private IEnumerable<IFootballMatch<Team>> GetGamesForTeam() => Stage.Schedule.Where(x => x.HasResult).Where(x => x.ParticipantPlaces.Item1.Participant == Team
                 || x.ParticipantPlaces.Item2.Participant == Team);

    }
}