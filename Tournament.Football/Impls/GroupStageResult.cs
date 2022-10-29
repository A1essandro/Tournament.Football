using System.Collections.Generic;
using System.Linq;

namespace Tournament.Football;

public class GroupStageResult
{

    public GroupStageResult(FootballStageBase<GroupStageResult> stage)
    {
        Stage = stage;

    }

    public FootballStageBase<GroupStageResult> Stage { get; }

    public Team[] MadeItToCL => ParticipantResults.Select(x => x.Team).Take(2).OfType<Team>().ToArray();

    public Team MadeItToLE => ParticipantResults.Select(x => x.Team).Skip(2).OfType<Team>().First();

    public IEnumerable<StageTeamResult<GroupStageResult>> ParticipantResults
    {
        get
        {
            var teamResults = Stage.ParticipantPlaces.Select(t => new StageTeamResult<GroupStageResult>(t.Participant, Stage));
            return teamResults.OrderByDescending(x => x.Points)
                .ThenBy(x => x, new FaceToFaceMeetComparer<GroupStageResult>(Stage))
                .ThenByDescending(x => x.GoalDiff)
                .ThenByDescending(x => x.Goals)
                .ThenBy(x => x.ConcededGoals)
                .ToArray();
        }
    }

    public bool IsCompleted => Stage.IsCompleted;
}
