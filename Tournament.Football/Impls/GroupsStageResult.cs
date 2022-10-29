using System.Collections.Generic;
using System.Linq;

namespace Tournament.Football;

public class GroupsStageResult
{

    public GroupsStageResult(GroupsStage stage)
    {
        Stage = stage;
    }

    public IEnumerable<Team> MadeIt => Stage.Stages.Select(x => x.Result as GroupStageResult).SelectMany(x => x.MadeItToCL).OfType<Team>();

    public IEnumerable<Team> MadeItLE => Stage.Stages.Select(x => x.Result as GroupStageResult).Select(x => x.MadeItToLE).OfType<Team>();

    public GroupsStage Stage { get; }

}
