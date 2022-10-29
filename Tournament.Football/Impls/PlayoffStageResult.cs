using System.Collections.Generic;
using System.Linq;

namespace Tournament.Football;

public class PlayoffStageResult
{

    public PlayoffStageResult(PlayoffStage stage)
    {
        Stage = stage;
    }

    public PlayoffStage Stage { get; }

    public IEnumerable<Team> MadeIt => Stage.Stages.Select(x => x.Result).OfType<KnockoutStageResult>().Select(x => x.MadeIt);

}
