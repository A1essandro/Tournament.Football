using System;
using Tournament.Contracts;
using Tournament.Football.Contracts;

namespace Tournament.Football;

public class FootballMatch : IFootballMatch<Team>
{

    public FootballMatch(IParticipantPlace<Team> participant1, IParticipantPlace<Team> participant2, DateTime date, IStage stage)
    {
        ParticipantPlaces = Pair<IParticipantPlace<Team>>.Create(participant1, participant2);
        Date = date;
        Context = stage;
    }

    public DateTime Date { get; set; }

    public bool HasResult => Result != null;

    private IFootballMatchResult<Team> _result;
    public IFootballMatchResult<Team> Result
    {
        get
        {
            return _result;
        }
        set
        {
            _result = value;
            OnResultSet?.Invoke(this);
            OnCompleted?.Invoke(this);
        }
    }

    public IPair<IParticipantPlace<Team>> ParticipantPlaces { get; }

    public IStage Context { get; }

    public bool IsCompleted => throw new NotImplementedException();

    public event Action<IFootballMatch<Team>> OnResultSet;

    public event Action<IFootballMatch<Team>> OnCompleted;
}