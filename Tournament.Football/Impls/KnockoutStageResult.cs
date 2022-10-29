using System.Linq;

namespace Tournament.Football;

public class KnockoutStageResult
{

    public KnockoutStageResult(FootballStageBase<KnockoutStageResult> stage, bool awayGoalRule = true)
    {
        Stage = stage;
        AwayGoleRule = awayGoalRule;
    }

    public FootballStageBase<KnockoutStageResult> Stage { get; }
    public bool AwayGoleRule { get; }

    public Team MadeIt
    {
        get
        {
            var teamResults = Stage.ParticipantPlaces.Select(t => new StageTeamResult<KnockoutStageResult>(t.Participant, Stage)).ToArray();
            var compare = new FaceToFaceMeetComparer<KnockoutStageResult>(Stage, AwayGoleRule).Compare(teamResults[0], teamResults[1]);

            if (compare == 0)
                return null;

            return teamResults
                .OrderBy(x => x, new FaceToFaceMeetComparer<KnockoutStageResult>(Stage, AwayGoleRule))
                .First().Team;
        }
    }

    public bool IsCompleted => Stage.Schedule.All(x => x.HasResult) && MadeIt != null;

}
